using System;
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
        private SynchronizedCollection<ProcessableVideoSource> sources = new SynchronizedCollection<ProcessableVideoSource>();
        private bool processingRunning = false;
        private readonly BroadcastBlock<Tuple<IDictionary<ProcessableVideoSource, Rectangle>, FrameAnalysisJSON>> broadcastBlock;
        private readonly ActionBlock<string> searchActionBlock;
        private readonly ActionBlock<Tuple<IDictionary<ProcessableVideoSource, Rectangle>, FrameAnalysisJSON>> faceRectanglesBlock;
        private readonly TransformManyBlock<Tuple<IDictionary<ProcessableVideoSource, Rectangle>, FrameAnalysisJSON>, string> faceTokenBlock;
        private readonly BufferBlock<string> searchBufferBlock = new BufferBlock<string>(new DataflowBlockOptions { BoundedCapacity = BUFFER_LIMIT });
        private readonly TransformBlock<IDictionary<ProcessableVideoSource, Bitmap>,
                                        Tuple<IDictionary<ProcessableVideoSource, Rectangle>, FrameAnalysisJSON>> manyPicturesAnalysisBlock;
        private readonly IPropagatorBlock<Tuple<ProcessableVideoSource, Bitmap>, IDictionary<ProcessableVideoSource, Bitmap>> batchBlock;
        private static readonly FaceApiCalls faceApiCalls = new FaceApiCalls(new HttpClientWrapper());
        private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();
        private readonly SearchResultHandler resultHandler;
        public event EventHandler<FrameProcessedEventArgs> FrameProcessed;

        public FaceProcessor(CameraProperties cameraProperties = null)
        {

            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };
            var blockOptions = new ExecutionDataflowBlockOptions { BoundedCapacity = 1 };

            //initialiase blocks.
            searchActionBlock = new ActionBlock<string>
            (
                faceToken => FaceSearch(faceToken),
                //this value can be changed to adjust how many API search requests are being sent.
                new ExecutionDataflowBlockOptions { BoundedCapacity = 1 }
            );
            faceRectanglesBlock = new ActionBlock<Tuple<IDictionary<ProcessableVideoSource, Rectangle>, FrameAnalysisJSON>>
            (
                tuple => RectanglesFromAnalysis(tuple)
            );
            faceTokenBlock = new TransformManyBlock<Tuple<IDictionary<ProcessableVideoSource, Rectangle>, FrameAnalysisJSON>, string>
            (
                tuple => tuple.Item2.Faces.Select(face => face.Face_token)
            );
            manyPicturesAnalysisBlock =
                new TransformBlock<IDictionary<ProcessableVideoSource, Bitmap>,
                                   Tuple<IDictionary<ProcessableVideoSource, Rectangle>, FrameAnalysisJSON>>
            (async dict => {
                processingRunning = true;
                var tuple = HelperMethods.ProcessImages(dict);
                var processedFrame = await ProcessFrame(tuple.Item2);
                processingRunning = false;
                return new Tuple<IDictionary<ProcessableVideoSource, Rectangle>, FrameAnalysisJSON>(tuple.Item1, processedFrame);
            });
            broadcastBlock = new BroadcastBlock<Tuple<IDictionary<ProcessableVideoSource, Rectangle>, FrameAnalysisJSON>>(item => item);
            batchBlock = BlockFactory.CreateConditionalDictionaryBlock<ProcessableVideoSource, Bitmap>(delegate
            {
                return !processingRunning;
            });

            /*establish links between them.
              batchBlock => manyPicturesAnalysisBlock => broadcastBlock => faceTokenBlock
                                                                        => searchBufferBlock => searchActionBlock
            */
            batchBlock.LinkTo(manyPicturesAnalysisBlock, linkOptions);
            manyPicturesAnalysisBlock.LinkTo(broadcastBlock, linkOptions, item => item != null);
            manyPicturesAnalysisBlock.LinkTo(DataflowBlock.NullTarget<Tuple<IDictionary<ProcessableVideoSource, Rectangle>, FrameAnalysisJSON>>());
            broadcastBlock.LinkTo(faceTokenBlock, linkOptions);
            broadcastBlock.LinkTo(faceRectanglesBlock, linkOptions);
            faceTokenBlock.LinkTo(searchBufferBlock, linkOptions);
            searchBufferBlock.LinkTo(searchActionBlock, linkOptions);

            //initialise search result handler.
            resultHandler = new SearchResultHandler(tokenSource.Token, cameraProperties);
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
            sources.Add(source);
            source.Stream.NewFrame += QueueFrame;
            FrameProcessed += source.UpdateRectangles;
        }

        public void RemoveSource(ProcessableVideoSource source)
        {
            if (sources.Contains(source))
            {
                source.Stream.NewFrame -= QueueFrame;
                FrameProcessed -= source.UpdateRectangles;
                sources.Remove(source);
            }
        }

        public async void Complete()
        {
            var sourcesCopy = sources.ToArray();
            foreach (var source in sourcesCopy)
                RemoveSource(source);
            batchBlock.Complete();
            searchBufferBlock.Complete();
            await searchActionBlock.Completion;
            tokenSource.Cancel();
        }

        /// <summary>
        /// Analyses the given frame (makes an API call, etc)
        /// Adds any found faces to a buffer for face search.
        /// Raises event that processing is finished (args of which contain the face rectangles)
        /// </summary>
        /// <returns>List of face rectangles from frame</returns>
        private void RectanglesFromAnalysis(Tuple<IDictionary<ProcessableVideoSource, Rectangle>, FrameAnalysisJSON> tuple)
        {
            var dictionary = tuple.Item1;
            var result = tuple.Item2;
            Dictionary<ProcessableVideoSource, List<Rectangle>> results = new Dictionary<ProcessableVideoSource, List<Rectangle>>();
            foreach (var pair in dictionary)
            {
                results[pair.Key] = result.Faces
                    .Where(face => pair.Value.IntersectsWith(face.Face_rectangle))
                    .Select(face =>
                    {
                        var rectangle = (Rectangle)face.Face_rectangle;
                        rectangle.Location = Point.Subtract(pair.Value.Location, (Size) rectangle.Location);
                        return (Rectangle)face.Face_rectangle;
                    })
                    .ToList();
            }

            OnProcessingCompletion(new FrameProcessedEventArgs(results));
        }

        /// <summary>
        /// Task for face search - executes API call, etc.
        /// </summary>
        private async Task FaceSearch(string faceToken)
        {
            while (await searchBufferBlock.OutputAvailableAsync())
            {
                FoundFacesJSON response = await faceApiCalls.SearchFaceInFaceset(Keys.facesetToken, faceToken);
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
            var source = sources.Where(src => src.Stream == sender).FirstOrDefault();
            await batchBlock.SendAsync(new Tuple<ProcessableVideoSource, Bitmap>(source, bitmap));
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

    public class FrameProcessedEventArgs : EventArgs
    {
        public FrameProcessedEventArgs(IDictionary<ProcessableVideoSource, List<Rectangle>> rectangleDictionary)
        {
            RectangleDictionary = rectangleDictionary;
        }

        public IDictionary<ProcessableVideoSource, List<Rectangle>> RectangleDictionary { get; }
    }
}

