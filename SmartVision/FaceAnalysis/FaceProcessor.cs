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
        private bool actionRunning = false;
        private readonly BufferBlock<string> searchBuffer = new BufferBlock<string>(new DataflowBlockOptions { BoundedCapacity = BUFFER_LIMIT });
        //TODO: split this one into two blocks, joinBlock then (tuple useless when converting.)
        private readonly TransformBlock<Tuple<IDictionary<ProcessableVideoSource, Rectangle>, Bitmap>,
                                        Tuple<IDictionary<ProcessableVideoSource, Rectangle>, byte[]>> byteArrayTransformBlock;
        private readonly TransformBlock<IDictionary<ProcessableVideoSource, Bitmap>,
                                        Tuple<IDictionary<ProcessableVideoSource, Rectangle>, Bitmap>> manyPicturesTransformBlock;
        private readonly ActionBlock<Tuple<IDictionary<ProcessableVideoSource, Rectangle>, byte[]>> actionBlock;
        private readonly IPropagatorBlock<Tuple<ProcessableVideoSource, Bitmap>, IDictionary<ProcessableVideoSource, Bitmap>> batchBlock;
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
            byteArrayTransformBlock = new TransformBlock<Tuple<IDictionary<ProcessableVideoSource, Rectangle>, Bitmap>, Tuple<IDictionary<ProcessableVideoSource, Rectangle>, byte[]>>(tuple =>
                new Tuple<IDictionary<ProcessableVideoSource, Rectangle>, byte[]>(tuple.Item1, HelperMethods.ImageToByte(tuple.Item2)), blockOptions);
            manyPicturesTransformBlock = new TransformBlock<IDictionary<ProcessableVideoSource, Bitmap>,
                                                            Tuple<IDictionary<ProcessableVideoSource, Rectangle>, Bitmap>>
                                                            (
                                                                dict => HelperMethods.ProcessImages(dict)
                                                            );
            actionBlock =
                new ActionBlock<Tuple<IDictionary<ProcessableVideoSource, Rectangle>, byte[]>>
                (
                    async tuple => await RectanglesFromFrame(tuple),
                    blockOptions
                );
            batchBlock = BlockFactory.CreateConditionalDictionaryBlock<ProcessableVideoSource, Bitmap>(delegate
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
        private async Task RectanglesFromFrame(Tuple<IDictionary<ProcessableVideoSource, Rectangle>, byte[]> tuple)
        {
            actionRunning = true;
            FrameAnalysisJSON result = await ProcessFrame(tuple.Item2);
            if (result == default(FrameAnalysisJSON))
            {
                actionRunning = false;
                return;
            }
            Dictionary<ProcessableVideoSource, List<Rectangle>> results = new Dictionary<ProcessableVideoSource, List<Rectangle>>();
            foreach (var pair in tuple.Item1)
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
            //TODO: send to search, in a seperate method.

            OnProcessingCompletion(new FrameProcessedEventArgs(results));
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

