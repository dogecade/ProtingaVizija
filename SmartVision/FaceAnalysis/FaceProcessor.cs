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
        private readonly FaceApiCalls faceApiCalls = new FaceApiCalls(new HttpClientWrapper());


        public FaceProcessor(VideoCapture capture)
        {
            this.capture = capture;

        }


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

        public async Task<bool> HasFrames()
        {
            return await buffer.OutputAvailableAsync();
        }

        public async Task<List<Rectangle>> ProcessFrame()
        {
            byte[] frameToProcess = await buffer.ReceiveAsync();
            List<Rectangle> faceRectangles = new List<Rectangle>();
            Debug.WriteLine("Starting processing of frame");
            try
            {
                var result = JsonConvert.DeserializeObject<FrameAnalysisJSON>(faceApiCalls.AnalyzeFrame(frameToProcess).Result);
                Debug.WriteLine(DateTime.Now + " " + result.faces.Count + " face(s) found in given frame");
                foreach (Face face in result.faces)
                    faceRectangles.Add(face.face_rectangle);
            }
            catch (ArgumentNullException)
            {
                Debug.WriteLine("Invalid response received from API");
            }
            return faceRectangles;
        }
    }
}
