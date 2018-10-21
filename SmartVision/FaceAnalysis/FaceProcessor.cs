using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Emgu.CV;
using Emgu.CV.Structure;

namespace FaceAnalysis
{
    public class FaceProcessor
    {
        private const int BUFFER_LIMIT = 10000;
        private VideoCapture capture;
        private readonly BufferBlock<string> searchBuffer = new BufferBlock<string>(new DataflowBlockOptions { BoundedCapacity = BUFFER_LIMIT });
        private readonly BroadcastBlock<byte[]> buffer = new BroadcastBlock<byte[]>(item => item);
        private static readonly FaceApiCalls faceApiCalls = new FaceApiCalls(new HttpClientWrapper());
        private Task searchTask;


        public FaceProcessor(VideoCapture capture)
        {
            this.capture = capture;
            searchTask = Task.Run(() => FaceSearch());
        }

        /// <summary>
        /// Gets frame camera feed
        /// </summary>
        /// <returns>Image frame (async)</returns>
        public async Task<Image<Bgr, byte>> GetCaptureFrame()
        {
            Image<Bgr, byte> imageFrame = null;
            try
            {
                imageFrame = capture.QueryFrame().ToImage<Bgr, byte>().Clone();
                imageFrame.Bitmap = HelperMethods.ProcessImage(imageFrame.Bitmap);
                await buffer.SendAsync(HelperMethods.ImageToByte(imageFrame.Bitmap));
            }
            catch (NullReferenceException)
            {
                Debug.WriteLine("No new frame available in camera feed");

            }
            return imageFrame;
        }

        /// <summary>
        /// Gets whether the buffer still has a frame to process.
        /// </summary>
        /// <returns>Buffer has frame (async)</returns>
        public async Task<bool> HasFrames()
        {
            return await buffer.OutputAvailableAsync();
        }


        /// <summary>
        /// "Completes" the search process -
        /// tells the class to complete the tokens currently in buffer and not to take any more.
        /// </summary>
        public async void Complete()
        {
            searchBuffer.Complete();
            await searchTask;
        }
  
        /// <summary>
        /// Analyses the frame currently in buffer (makes an API call, etc)
        /// Adds any found faces to a buffer for face search.
        /// </summary>
        /// <returns>List of face rectangles from frame</returns>
        public async Task<List<Rectangle>> GetRectanglesFromFrame()
        {
            var result = await ProcessFrame(await buffer.ReceiveAsync());
            foreach (Face face in result.faces)
                await searchBuffer.SendAsync(face.face_token);
            return result == null ? 
                null : (from face in result.faces select (Rectangle)face.face_rectangle).ToList();
        }

        /// <summary>
        /// Main task for face search - executes API call, etc.
        /// </summary>
        private async void FaceSearch()
        {
            while (await searchBuffer.OutputAvailableAsync())
            {
                FoundFacesJSON response = await faceApiCalls.SearchFaceInFaceset(Keys.facesetToken, await searchBuffer.ReceiveAsync());
                Debug.WriteLine(response);
                if (!response.Equals(null))
                    foreach (Result result in response.results)
                    {
                        Debug.WriteLine("Confidence: " + result.confidence);
                    }
            }
        }

        /// <summary>
        /// Analyses the given byte array
        /// </summary>
        /// <returns>JSON response, null if invalid</returns>
        public static async Task<FrameAnalysisJSON> ProcessFrame(byte[] frameToProcess)
        {
            Debug.WriteLine("Starting processing of frame");
            var result = await faceApiCalls.AnalyzeFrame(frameToProcess);
            if (result != null)
                Debug.WriteLine(DateTime.Now + " " + result.faces.Count + " face(s) found in given frame");
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
}
