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

namespace FaceAnalysis
{
    public class FaceProcessor
    {
        private const int BUFFER_LIMIT = 10000;
        private IList<IVideoSource> sources;
        private readonly BufferBlock<string> searchBuffer = new BufferBlock<string>(new DataflowBlockOptions { BoundedCapacity = BUFFER_LIMIT });
        private readonly BroadcastBlock<byte[]> buffer = new BroadcastBlock<byte[]>(item => item);
        private static readonly FaceApiCalls faceApiCalls = new FaceApiCalls(new HttpClientWrapper());
        private static readonly CancellationTokenSource tokenSource = new CancellationTokenSource();
        private static readonly SearchResultHandler resultHandler = new SearchResultHandler(tokenSource.Token);
        private readonly Task searchTask;
        public event EventHandler<FrameProcessedEventArgs> FrameProcessed;

        public FaceProcessor(IList<IVideoSource> sources)
        {
            foreach (var source in sources)
                source.NewFrame += QueueFrame;
            this.sources = sources;
            searchTask = Task.Run(() => FaceSearch());
            Task.Run(() => ProcessFrameAsync());
        }
   
        public FaceProcessor(IVideoSource sources) : this(new List<IVideoSource> { sources }) { }

        /// <summary>
        /// "Completes" the processing:
        /// unsubscribes from sources
        /// search process - tells the class to complete the tokens currently in buffer and not to take any more.
        /// </summary>
        public async void Complete()
        {
            foreach (var source in sources)
                source.NewFrame -= QueueFrame;
            searchBuffer.Complete();
            await searchTask;
            tokenSource.Cancel();
        }

        /// <summary>
        /// Analyses the frame currently in buffer (makes an API call, etc)
        /// Adds any found faces to a buffer for face search.
        /// </summary>
        /// <returns>List of face rectangles from frame</returns>
        public async Task<List<Rectangle>> GetRectanglesFromFrame()
        {
            FrameAnalysisJSON result = await ProcessFrame(await buffer.ReceiveAsync());
            if (result == default(FrameAnalysisJSON))
                return null;
            foreach (Face face in result.Faces)
                await searchBuffer.SendAsync(face.Face_token);
            return (from face in result.Faces select (Rectangle)face.Face_rectangle).ToList();
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
            lock (e)
                bitmap = new Bitmap(e.Frame);
            await buffer.SendAsync(HelperMethods.ImageToByte(bitmap));
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
        /// Task to run frame processing
        /// </summary>
        /// <returns></returns>
        private async Task ProcessFrameAsync()
        {
            while (await buffer.OutputAvailableAsync() && !tokenSource.IsCancellationRequested)
            {
                var faceRectangles = await GetRectanglesFromFrame();
                OnProcessingCompletion(new FrameProcessedEventArgs { FaceRectangles = faceRectangles } );
            }
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
        public IList<Rectangle> FaceRectangles { get; set; }
    }
}

