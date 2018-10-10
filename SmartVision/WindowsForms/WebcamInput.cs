using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsForms.FaceAnalysis;
using WindowsForms.FormControl;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Threading.Tasks.Dataflow;
using FaceAnalysis;
using System.Threading;
using System.Drawing;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace WindowsForms
{
    public class WebcamInput
    {
        private static List<Rectangle> faceRectangles = new List<Rectangle>();
        private static CancellationTokenSource tokenSource = new CancellationTokenSource();
        private static CancellationToken token = tokenSource.Token;
        private static readonly BroadcastBlock<byte[]> buffer = new BroadcastBlock<byte[]>(item => item);
        private static VideoCapture capture; // Takes video from camera as image frames
        private static Task taskConsumer;
        private static readonly FaceApiCalls faceApiCalls = new FaceApiCalls(new HttpClientWrapper());

        /// <summary>
        /// Enables the input of the webcam
        /// </summary>
        public static bool EnableWebcam()
        {
            try
            {
                capture = new VideoCapture();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            try
            {
                if (!capture.IsOpened)
                {
                    throw new SystemException("Input camera was not found");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                MessageBox.Show("Input camera was not found!");
                return false;
            }
            taskConsumer = Task.Run(() => ProcessFrameAsync());
            Application.Idle += GetFrameAsync;
            return true;
        }

        /// <summary>
        /// Disables the input of the webcam
        /// </summary>
        public static void DisableWebcam()
        {
            try
            {
                capture.Dispose();
                Application.Idle -= GetFrameAsync;
                tokenSource.Cancel();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Gets frame from input and adds to buffer.
        /// Author: Arnas Danaitis
        /// </summary>
        private static async void GetFrameAsync(object sender, EventArgs e)
        {
            using (var imageFrame = capture.QueryFrame().ToImage<Bgr, Byte>())
            {
                var form = FormFaceDetection.Current;
                form.scanPictureBox.Image = imageFrame.Bitmap;
                foreach (Rectangle face in faceRectangles)
                    imageFrame.Draw(face, new Bgr(Color.Red), 1);
                await buffer.SendAsync(HelperMethods.ImageToByte(imageFrame.Bitmap));
            }

        }

        /// <summary>
        /// Processes frames from the buffer.
        /// Author: Arnas Danaitis
        /// </summary>
        private static async void ProcessFrameAsync()
        {
            while (await buffer.OutputAvailableAsync())
            {
                if (token.IsCancellationRequested)
                    break;
                byte[] frameToProcess = await buffer.ReceiveAsync();
                Debug.WriteLine("Starting processing of frame");
                try
                {
                    var result = JsonConvert.DeserializeObject<FrameAnalysisJSON>(faceApiCalls.AnalyzeFrame(frameToProcess).Result);
                    Debug.WriteLine(DateTime.Now + " " + result.faces.Count + " face(s) found in given frame");
                    faceRectangles.Clear();
                    foreach (Face face in result.faces)
                        faceRectangles.Add(face.face_rectangle);
                }
                catch (ArgumentNullException)
                {
                    Debug.WriteLine("Invalid response received from API");
                }
            }
        }
    }
}
