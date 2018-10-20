using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Emgu.CV;
using Emgu.CV.Structure;

namespace FaceAnalysis
{
    public class FaceProcessor
    {

        private VideoCapture capture;
        private readonly BroadcastBlock<byte[]> buffer = new BroadcastBlock<byte[]>(item => item);
        private static readonly FaceApiCalls faceApiCalls = new FaceApiCalls(new HttpClientWrapper());


        public FaceProcessor(VideoCapture capture)
        {
            this.capture = capture;

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
        /// Analyses the frame currently in buffer (makes an API call, etc)
        /// </summary>
        /// <returns>List of face rectangles from frame</returns>
        public async Task<List<Rectangle>> GetRectanglesFromFrame()
        {
            var result = await ProcessFrame(await buffer.ReceiveAsync());
            foreach (Face face in result.faces)
                await faceApiCalls.SearchFaceInFaceset("aaaaaa", face.face_token);
            return result == null ? 
                null : (from face in result.faces select (Rectangle)face.face_rectangle).ToList();
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
            { 
                Debug.WriteLine(DateTime.Now + " " + result.faces.Count + " face(s) found in given frame");
                foreach (Face face in result.faces)
                    Debug.WriteLine("Face token: " + face.face_token);
            }
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
