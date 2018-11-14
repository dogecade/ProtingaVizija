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
using Objects.CameraProperties;

namespace FaceAnalysis
{
    public class FaceProcessor
    {
        private const int MAX_SOURCES = 9;
        private const int BUFFER_LIMIT = 10000;
        private ConcurrentDictionary<ProcessableVideoSource, BroadcastBlock<SourceBitmapPair>> sources =
            new ConcurrentDictionary<ProcessableVideoSource, BroadcastBlock<SourceBitmapPair>>();
        private bool actionRunning = false;
        private readonly BufferBlock<string> searchBuffer = new BufferBlock<string>(new DataflowBlockOptions { BoundedCapacity = BUFFER_LIMIT });
        //TODO: split this one into two blocks, joinBlock then (tuple useless when converting.)
        private readonly TransformBlock<Tuple<Dictionary<ProcessableVideoSource, Rectangle>, Bitmap>, Tuple<Dictionary<ProcessableVideoSource, Rectangle>, byte[]>> byteArrayTransformBlock;
        private readonly TransformBlock<SourceBitmapPair[], Tuple<Dictionary<ProcessableVideoSource, Rectangle>, Bitmap>> manyPicturesTransformBlock;
        private readonly ActionBlock<Tuple<Dictionary<ProcessableVideoSource, Rectangle>, byte[]>> actionBlock;
        private readonly IPropagatorBlock<SourceBitmapPair, SourceBitmapPair[]> batchBlock;
        private static readonly FaceApiCalls faceApiCalls = new FaceApiCalls(new HttpClientWrapper());
        private static readonly CancellationTokenSource tokenSource = new CancellationTokenSource();
        private static readonly SearchResultHandler resultHandler = new SearchResultHandler(tokenSource.Token);
        private readonly Task searchTask;
        public event EventHandler<FrameProcessedEventArgs> FrameProcessed;
        private CameraProperties CameraProperties;

        public FaceProcessor(CameraProperties cameraProperties = null)
        {
            CameraProperties = cameraProperties;

            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };
            var blockOptions = new ExecutionDataflowBlockOptions { BoundedCapacity = 1 };

            //initialiase blocks.
            byteArrayTransformBlock = new TransformBlock<Tuple<Dictionary<ProcessableVideoSource, Rectangle>, Bitmap>, Tuple<Dictionary<ProcessableVideoSource, Rectangle>, byte[]>>(tuple =>
                new Tuple<Dictionary<ProcessableVideoSource, Rectangle>, byte[]>(tuple.Item1, HelperMethods.ImageToByte(tuple.Item2)), blockOptions);
            manyPicturesTransformBlock = new TransformBlock<SourceBitmapPair[], Tuple<Dictionary<ProcessableVideoSource, Rectangle>, Bitmap>>(pairs =>
            {

                return HelperMethods.ProcessImages(pairs.Select(pair => (Tuple<ProcessableVideoSource, Bitmap>)pair));
            });
            actionBlock =
                new ActionBlock<Tuple<Dictionary<ProcessableVideoSource, Rectangle>, byte[]>>
                (
                    async tuple => await RectanglesFromFrame(tuple),
                    blockOptions
                );
            batchBlock = BlockFactory.CreateDistinctConditionalBatchBlock(delegate
            {
                return !actionRunning;
            });

            //establish links between them.
            batchBlock.LinkTo(manyPicturesTransformBlock, linkOptions);
            manyPicturesTransformBlock.LinkTo(byteArrayTransformBlock, linkOptions);
            byteArrayTransformBlock.LinkTo(actionBlock, linkOptions);

            //start search task.
            searchTask = Task.Run(() => FaceSearch());
        }

        public FaceProcessor(IList<ProcessableVideoSource> sources, CameraProperties cameraProperties = null) : this(cameraProperties)
        {
            foreach (var source in sources)
                AddSource(source);
        }

        public FaceProcessor(ProcessableVideoSource source, CameraProperties cameraProperties = null) : this(cameraProperties)
        {
            AddSource(source);
        }

        public void AddSource(ProcessableVideoSource source)
        {
            if (source?.Stream == null)
                throw new ArgumentException("Source and its stream cannot be null");
            if (sources.Count + 1 > MAX_SOURCES * MAX_SOURCES)
                throw new ArgumentException(string.Format("Number of sources exceeds limit ({0})", MAX_SOURCES));
            var broadcastBlock = new BroadcastBlock<SourceBitmapPair>(item => item);
            broadcastBlock.LinkTo(batchBlock);
            sources.AddOrUpdate
            (
                key: source,
                addValue: broadcastBlock,
                updateValueFactory: (key, value) => value
            );
            source.Stream.NewFrame += QueueFrame;
            FrameProcessed += source.UpdateRectangles;
        }

        public void RemoveSource(ProcessableVideoSource source)
        {
            var key = sources.Keys.Where(k => k == source).FirstOrDefault();
            if (key == null)
                return;
            key.Stream.NewFrame -= QueueFrame;
            FrameProcessed -= key.UpdateRectangles;
            sources[key].Complete();
            sources.TryRemove(key, out _);
        }

        public async void Complete()
        {
            foreach (var source in sources.Keys)
                RemoveSource(source);
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
        public async Task RectanglesFromFrame(Tuple<Dictionary<ProcessableVideoSource, Rectangle>, byte[]> tuple)
        {
            actionRunning = true;
            FrameAnalysisJSON result = await ProcessFrame(tuple.Item2);
            Dictionary<ProcessableVideoSource, List<Rectangle>> results = new Dictionary<ProcessableVideoSource, List<Rectangle>>();
            if (result == default(FrameAnalysisJSON))
            {
                actionRunning = false;
                return;
            }
            foreach (Face face in result.Faces)
            {
                await searchBuffer.SendAsync(face.Face_token);
                foreach (var pair in tuple.Item1)
                    if (pair.Value.Contains(face.Face_rectangle))
                    {
                        if (!results.TryGetValue(pair.Key, out _))
                            results[pair.Key] = new List<Rectangle>();
                        var rectangle = (Rectangle)face.Face_rectangle;
                        rectangle.Location = new Point(0, 0);
                        results[pair.Key].Add(face.Face_rectangle);
                    }

            }

            var faceRectangles = from face in result.Faces select (Rectangle)face.Face_rectangle;
            OnProcessingCompletion(new FrameProcessedEventArgs { RectangleDictionary = results });
            actionRunning = false;
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
                        resultHandler.HandleSearchResult(CameraProperties,result);
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
            await sources[key].SendAsync(new SourceBitmapPair(key, bitmap));
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
    internal class SourceBitmapPair : IDisposable
    {
        public SourceBitmapPair(ProcessableVideoSource source, Bitmap bitmap)
        {
            Source = source;
            Bitmap = bitmap;
        }

        public ProcessableVideoSource Source { get; }
        public Bitmap Bitmap { get; set; }
        public void Dispose() => Bitmap?.Dispose();

        public static implicit operator Tuple<ProcessableVideoSource, Bitmap>(SourceBitmapPair pair) =>
            new Tuple<ProcessableVideoSource, Bitmap>(pair.Source, pair.Bitmap);
    }

    public class FrameProcessedEventArgs : EventArgs
    {
        public Dictionary<ProcessableVideoSource, List<Rectangle>> RectangleDictionary { get; set; }
    }
}

