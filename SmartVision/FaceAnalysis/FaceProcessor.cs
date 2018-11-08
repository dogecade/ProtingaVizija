using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using AForge.Video;
using Constants;
using Helpers;

namespace FaceAnalysis
{
    public class FaceProcessor
    {
        private const int MAX_FRAMES_EDGE = 3;
        private const int BUFFER_LIMIT = 10000;
        private ConcurrentDictionary<ProcessableVideoSource, SourceBlocks> sources = new ConcurrentDictionary<ProcessableVideoSource, SourceBlocks>();
        private bool actionRunning = false;
        private readonly BufferBlock<string> searchBuffer = new BufferBlock<string>(new DataflowBlockOptions { BoundedCapacity = BUFFER_LIMIT });
        private readonly TransformBlock<Tuple<IList<Guid>, Bitmap>, Tuple<IList<Guid>, byte[]>> byteArrayTransformBlock;
        private readonly TransformBlock<GuidBitmapPair[], Tuple<IList<Guid>, Bitmap>> manyPicturesTransformBlock;
        private readonly ActionBlock<Tuple<IList<Guid>, byte[]>> actionBlock;
        private readonly BatchBlock<GuidBitmapPair> batchBlock;
        private static readonly FaceApiCalls faceApiCalls = new FaceApiCalls(new HttpClientWrapper());
        private static readonly CancellationTokenSource tokenSource = new CancellationTokenSource();
        private static readonly SearchResultHandler resultHandler = new SearchResultHandler(tokenSource.Token);
        private readonly Task searchTask;
        public event EventHandler<FrameProcessedEventArgs> FrameProcessed;

        private void RemoveLink(Guid guid)
        {
            var key = sources.Keys.Where(source => source.Id == guid).FirstOrDefault();
            sources[key].Link.Dispose();
        }

        private void EstablishLinks()
        {
            List<ProcessableVideoSource> keys = new List<ProcessableVideoSource>(sources.Keys);
            foreach (var key in keys)
                sources.AddOrUpdate(
                    key,
                    default(SourceBlocks),
                    (oldKey, value) => { value.Link = value.BroadcastBlock.LinkTo(value.Block); return value; }
                );
        }

        public FaceProcessor(IList<ProcessableVideoSource> sources)
        {
            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };
            var blockOptions = new ExecutionDataflowBlockOptions { BoundedCapacity = 1 };


            //initialiase blocks.
            byteArrayTransformBlock = new TransformBlock<Tuple<IList<Guid>, Bitmap>, Tuple<IList<Guid>, byte[]>>(tuple => new Tuple<IList<Guid>, byte[]>(tuple.Item1, HelperMethods.ImageToByte(tuple.Item2)), blockOptions);
            manyPicturesTransformBlock = new TransformBlock<GuidBitmapPair[], Tuple<IList<Guid>, Bitmap>>(tuples => HelperMethods.ProcessImages(tuples));
            actionBlock = new ActionBlock<Tuple<IList<Guid>, byte[]>>(async tuple => await RectanglesFromFrame(tuple.Item2), blockOptions);
            batchBlock = new BatchBlock<GuidBitmapPair>(sources.Count);

            foreach (var source in sources)
            {
                var broadcastBlock = new BroadcastBlock<GuidBitmapPair>(pair =>
                {
                    return pair;
                });
                var transformBlock = new TransformBlock<GuidBitmapPair, GuidBitmapPair>(tuple =>
                {
                    RemoveLink(tuple.Guid);
                    //tuple.Bitmap = HelperMethods.ProcessImage(tuple.Bitmap);
                    return tuple;
                }, blockOptions);
                var blockSet = new SourceBlocks(broadcastBlock, transformBlock, broadcastBlock.LinkTo(transformBlock));
                this.sources.AddOrUpdate(
                    source,
                    blockSet,
                    (key, value) => blockSet
                );
                transformBlock.LinkTo(batchBlock, delegate { return !actionRunning; });
                transformBlock.LinkTo(DataflowBlock.NullTarget<GuidBitmapPair>());
                source.Stream.NewFrame += QueueFrame;
                FrameProcessed += source.UpdateRectangles;
            }

            //establish links between them.
            batchBlock.LinkTo(manyPicturesTransformBlock, linkOptions);
            manyPicturesTransformBlock.LinkTo(byteArrayTransformBlock, linkOptions);
            byteArrayTransformBlock.LinkTo(actionBlock, linkOptions);

            //start search task.
            searchTask = Task.Run(() => FaceSearch());
        }

        public FaceProcessor(ProcessableVideoSource sources) : this(new List<ProcessableVideoSource> { sources }) { }

        public async void Complete()
        {
            foreach (var source in sources.Keys)
            {
                source.Stream.NewFrame -= QueueFrame;
                FrameProcessed -= source.UpdateRectangles;
            }
            batchBlock.Complete();
            searchBuffer.Complete();
            await actionBlock.Completion;
            await searchTask;
            tokenSource.Cancel();
        }

        /// <summary>
        /// Analyses the given frame (makes an API call, etc)
        /// Adds any found faces to a buffer for face search.
        /// Raises event that processing is finished (args of which contain the face rectangles)
        /// </summary>
        /// <returns>List of face rectangles from frame</returns>
        public async Task RectanglesFromFrame(byte[] bitmap)
        {
            actionRunning = true;
            FrameAnalysisJSON result = await ProcessFrame(bitmap);
            if (result == default(FrameAnalysisJSON))
            {
                actionRunning = false;
                EstablishLinks();
                return;
            }
            foreach (Face face in result.Faces)
                await searchBuffer.SendAsync(face.Face_token);
            var faceRectangles = from face in result.Faces select (Rectangle)face.Face_rectangle;
            OnProcessingCompletion(new FrameProcessedEventArgs { FaceRectangles = faceRectangles });
            actionRunning = false;
            EstablishLinks();
        }

        /// <summary>
        /// Main task for face search - executes API call, etc.
        /// </summary>
        private async void FaceSearch()
        {
            while (await searchBuffer.OutputAvailableAsync())
            {
                FoundFacesJSON response = await faceApiCalls.SearchFaceInFaceset(Keys.facesetToken, await searchBuffer.ReceiveAsync());
                if (response != null)
                    foreach (LikelinessResult result in response.LikelinessConfidences())
                        resultHandler.HandleSearchResult(result);
            }
        }

        /// <summary>
        /// Event handler for "giving" a new frame to the processor.
        /// </summary>
        /// <param name="sender">ICapture</param>
        /// <param name="e">Event args</param>
        private async void QueueFrame(object sender, NewFrameEventArgs e)
        {
            Bitmap bitmap;
            lock (sender)
                bitmap = new Bitmap(e.Frame);
            var key = sources.Keys.Where(source => source.Stream == sender).FirstOrDefault();
            await sources[key].BroadcastBlock.SendAsync(new GuidBitmapPair(key.Id, bitmap));
        }

        /// <summary>
        /// Event when processor finishes a frame
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnProcessingCompletion(FrameProcessedEventArgs e)
        {
            FrameProcessed?.Invoke(this, e);
        }

        /// <summary>
        /// Analyses the given byte array
        /// </summary>
        /// <returns>JSON response, null if invalid</returns>
        public static async Task<FrameAnalysisJSON> ProcessFrame(byte[] frameToProcess)
        {
            Debug.WriteLine("Starting processing of frame");
            FrameAnalysisJSON result = await faceApiCalls.AnalyzeFrame(frameToProcess);
            if (result != null)
                Debug.WriteLine(DateTime.Now + " " + result.Faces.Count + " face(s) found in given frame");
            return result;
        }

        /// <summary>
        /// Converts the Bitmap to byte[] and calls ProcessFrame(byte[])
        /// </summary>
        /// <returns>ProcessFrame(byte[])</returns>
        public static async Task<FrameAnalysisJSON> ProcessFrame(Bitmap bitmap)
        {
            return await ProcessFrame(HelperMethods.ImageToByte(bitmap));
        }

    }
    public struct GuidBitmapPair
    {
        public GuidBitmapPair(Guid guid, Bitmap bitmap) : this()
        {
            Guid = guid;
            Bitmap = bitmap;
        }

        public Guid Guid { get; set; }
        public Bitmap Bitmap { get; set; }
    }

    public struct SourceBlocks
    {
        public SourceBlocks(BroadcastBlock<GuidBitmapPair> broadcastBlock, TransformBlock<GuidBitmapPair, GuidBitmapPair> block, IDisposable link) : this()
        {
            BroadcastBlock = broadcastBlock;
            Block = block;
            Link = link;
        }

        public BroadcastBlock<GuidBitmapPair> BroadcastBlock { get; }
        public TransformBlock<GuidBitmapPair, GuidBitmapPair> Block { get; }
        public IDisposable Link { get; set; }
    }

    public class FrameProcessedEventArgs : EventArgs
    {
        public IEnumerable<Rectangle> FaceRectangles { get; set; }
    }
}

