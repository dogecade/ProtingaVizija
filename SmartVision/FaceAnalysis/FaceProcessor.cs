using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using AForge.Video;
using Constants;
using Helpers;
using Objects.CameraProperties;

namespace FaceAnalysis
{
    /// <summary>
    /// Face processor that holds ProcessableVideoSources and collects their frames,
    /// Then sends them to the API as a batch. Also handles searching.
    /// </summary>
    public class FaceProcessor
    {
        private const int MAX_SOURCES = 16;
        private const int BUFFER_LIMIT = 10000;
        private ConcurrentDictionary<IVideoSource, ProcessableVideoSource> sources = new ConcurrentDictionary<IVideoSource, ProcessableVideoSource>();
        private readonly BroadcastBlock<(IDictionary<ProcessableVideoSource, Rectangle>, FrameAnalysisJSON)> broadcastBlock;
        private readonly ActionBlock<string> searchActionBlock;
        private readonly ActionBlock<(IDictionary<ProcessableVideoSource, Rectangle>, FrameAnalysisJSON)> faceRectanglesBlock;
        private readonly TransformManyBlock<(IDictionary<ProcessableVideoSource, Rectangle>, FrameAnalysisJSON), string> faceTokenBlock;
        private readonly BufferBlock<string> searchBufferBlock = new BufferBlock<string>(new DataflowBlockOptions { BoundedCapacity = BUFFER_LIMIT });
        private readonly TransformBlock<IDictionary<ProcessableVideoSource, Bitmap>,
                                        (IDictionary<ProcessableVideoSource, Rectangle>, FrameAnalysisJSON)> manyPicturesAnalysisBlock;
        private readonly BatchDictionaryBlock<ProcessableVideoSource, Bitmap> batchBlock;
        private static readonly FaceApiCalls faceApiCalls = new FaceApiCalls(new HttpClientWrapper());
        private readonly SearchResultHandler resultHandler;
        

        /// <summary>
        /// Event that processing is completed.
        /// </summary>
        public event EventHandler<FrameProcessedEventArgs> FrameProcessed;

        /// <summary>
        /// Event that face(s) were detected.
        /// </summary>
        public event EventHandler<FacesDetectedEventArgs> FacesDetected;

        public FaceProcessor(CameraProperties cameraProperties = null)
        {
            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };
            var blockOptions = new ExecutionDataflowBlockOptions { BoundedCapacity = 1 };

            //initialiase blocks.
            searchActionBlock = new ActionBlock<string>
            (
                faceToken => FaceSearch(faceToken),
                //this value can be changed to adjust how many API search requests are being sent.
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1 }
            );
            faceRectanglesBlock = new ActionBlock<(IDictionary<ProcessableVideoSource, Rectangle>, FrameAnalysisJSON)>
            (
                tuple => RectanglesFromAnalysis(tuple.Item1, tuple.Item2)
            );
            faceTokenBlock = new TransformManyBlock<(IDictionary<ProcessableVideoSource, Rectangle>, FrameAnalysisJSON), string>
            (
                tuple => tuple.Item2.Faces.Select(face => face.Face_token)
            );
            manyPicturesAnalysisBlock =
                new TransformBlock<IDictionary<ProcessableVideoSource, Bitmap>,
                                   (IDictionary<ProcessableVideoSource, Rectangle>, FrameAnalysisJSON)>
            (async dict => {
                var (rectangles, image) = HelperMethods.ProcessImages(dict);
                var processedFrame = await ProcessFrame(image);
                await batchBlock.TriggerBatch();
                return (rectangles, processedFrame);
            });
            broadcastBlock = new BroadcastBlock<(IDictionary<ProcessableVideoSource, Rectangle>, FrameAnalysisJSON)>(item => item);
            batchBlock = new BatchDictionaryBlock<ProcessableVideoSource, Bitmap>();

            /*establish links between them.
              batchBlock => manyPicturesAnalysisBlock => broadcastBlock => faceTokenBlock
                                                                        => searchBufferBlock => searchActionBlock
            */
            batchBlock.LinkTo(manyPicturesAnalysisBlock, linkOptions);
            manyPicturesAnalysisBlock.LinkTo(broadcastBlock, linkOptions, item => item.Item2 != null);
            manyPicturesAnalysisBlock.LinkTo(DataflowBlock.NullTarget<(IDictionary<ProcessableVideoSource, Rectangle>, FrameAnalysisJSON)>());
            broadcastBlock.LinkTo(faceTokenBlock, linkOptions);
            broadcastBlock.LinkTo(faceRectanglesBlock, linkOptions);
            faceTokenBlock.LinkTo(searchBufferBlock, linkOptions);
            searchBufferBlock.LinkTo(searchActionBlock, linkOptions);

            //initialise search result handler.
            resultHandler = new SearchResultHandler(cameraProperties);
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

        /// <summary>
        /// Adds video source to processor
        /// </summary>
        /// <param name="source">Video source to add</param>
        public void AddSource(ProcessableVideoSource source)
        {
            if (source?.Stream == null)
                throw new ArgumentException("Source and its stream cannot be null");
            if (sources.Count + 1 > MAX_SOURCES * MAX_SOURCES)
                throw new ArgumentException(string.Format("Number of sources exceeds limit ({0})", MAX_SOURCES));
            sources[source.Stream] = source;
            source.Stream.NewFrame += QueueFrame;
            FrameProcessed += source.UpdateRectangles;
        }

        /// <summary>
        /// Removes source from processor (if present)
        /// </summary>
        /// <param name="source">Source to remove</param>
        public void RemoveSource(ProcessableVideoSource source)
        {
            if (sources.TryRemove(source.Stream, out _))
            {
                source.Stream.NewFrame -= QueueFrame;
                FrameProcessed -= source.UpdateRectangles;
            }
        }

        /// <summary>
        /// Starts processing - waits until a frame arrives, then triggers the batch.
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            await batchBlock.TriggerBatch();
        }

        /// <summary>
        /// Updates properties of the underlying search result handler
        /// </summary>
        public void UpdateProperties(CameraProperties cameraProperties)
        {
            resultHandler.UpdateProperties(cameraProperties);
        }

        /// <summary>
        /// Updates the keys of the underlying API caller
        /// </summary>
        /// <param name="apiKeySet"></param>
        public void UpdateKeys(ApiKeySet apiKeySet)
        {
            faceApiCalls.ApiKeys = apiKeySet;
        }

        /// <summary>
        /// Completes the processor - completes its blocks, removes all its sources, etc.
        /// </summary>
        public async void Complete()
        {
            foreach (var source in sources.Values)
                RemoveSource(source);
            batchBlock.Complete();
            searchBufferBlock.Complete();
            await searchActionBlock.Completion;
            resultHandler.Complete();
        }

        /// <summary>
        /// Analyses the given frame (makes an API call, etc)
        /// Adds any found faces to a buffer for face search.
        /// Raises event that processing is finished (args of which contain the face rectangles)
        /// </summary>
        /// <returns>List of face rectangles from frame</returns>
        private void RectanglesFromAnalysis(IDictionary<ProcessableVideoSource, Rectangle> sourceRectanglePairs, FrameAnalysisJSON result)
        {
            Dictionary<ProcessableVideoSource, List<Rectangle>> results = new Dictionary<ProcessableVideoSource, List<Rectangle>>();
            foreach (var pair in sourceRectanglePairs)
            {
                results[pair.Key] = result.Faces
                    .Where(face => pair.Value.IntersectsWith(face.Face_rectangle))
                    .Select(face =>
                    {
                        var rectangle = (Rectangle)face.Face_rectangle;
                        rectangle.Location = Point.Subtract(rectangle.Location, (Size)pair.Value.Location);
                        return rectangle;
                    })
                    .ToList();
            }

            OnProcessingCompletion(new FrameProcessedEventArgs(results));

            var sourcesWithFaces = results
                .Where(pair => pair.Value.Count > 0)
                .Select(pair => pair.Key);

            OnFacesDetected(new FacesDetectedEventArgs(sourcesWithFaces));
        }

        /// <summary>
        /// Task for face search - executes API call, etc.
        /// </summary>
        private async Task FaceSearch(string faceToken)
        {
            FoundFacesJSON response = await faceApiCalls.SearchFaceInFaceset(faceToken);
            if (response != null)
                foreach (LikelinessResult result in response.LikelinessConfidences())
                    await resultHandler.HandleSearchResult(result);
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
            var source = (IVideoSource)sender;
            await batchBlock.SendAsync((sources[source], bitmap));
        }

        /// <summary>
        /// Event when processor finishes a frame
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnProcessingCompletion(FrameProcessedEventArgs e)
        {
            FrameProcessed?.Invoke(this, e);           
        }

        protected virtual void OnFacesDetected(FacesDetectedEventArgs e)
        {
            FacesDetected?.Invoke(this, e);
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

    /// <summary>
    /// Event args for event that processor is done processing.
    /// Holds source / rectangle list pairzs in the form of a dictionary.
    /// </summary>
    public class FrameProcessedEventArgs : EventArgs
    {
        public FrameProcessedEventArgs(IDictionary<ProcessableVideoSource, List<Rectangle>> rectangleDictionary)
        {
            RectangleDictionary = rectangleDictionary;
        }

        public IDictionary<ProcessableVideoSource, List<Rectangle>> RectangleDictionary { get; }
    }

    /// <summary>
    /// Event args for detected faces event,
    /// Holds sources where face was detected.
    /// </summary>
    public class FacesDetectedEventArgs : EventArgs
    {
        public IEnumerable<ProcessableVideoSource> Sources { get; }
        public FacesDetectedEventArgs(IEnumerable<ProcessableVideoSource> sources)
        {
            Sources = sources;
        }
    }
}

