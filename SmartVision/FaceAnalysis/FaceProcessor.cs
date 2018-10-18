using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Emgu.CV;
using Emgu.CV.Structure;
using Newtonsoft.Json;

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
        public async Task<List<Rectangle>> ProcessFrame()
        {
            byte[] frameToProcess = await buffer.ReceiveAsync();
            return await ProcessFrame(frameToProcess);
        }

        public static async Task<List<Rectangle>> ProcessFrame(byte[] frameToProcess)
        {
            Debug.WriteLine("Starting processing of frame");
            try
            {
                var result = JsonConvert.DeserializeObject<FrameAnalysisJSON>(await faceApiCalls.AnalyzeFrame(frameToProcess));
                Debug.WriteLine(DateTime.Now + " " + result.faces.Count + " face(s) found in given frame");
                return (from face in result.faces select (Rectangle)face.face_rectangle).ToList();
            }
            catch (ArgumentNullException)
            {
                Debug.WriteLine("Invalid response received from API");
                return null;
            }
            
        }

        public static async Task<List<Rectangle>> ProcessFrame(Bitmap bitmap)
        {
            return await ProcessFrame(HelperMethods.ImageToByte(bitmap));
        }
    }
}
