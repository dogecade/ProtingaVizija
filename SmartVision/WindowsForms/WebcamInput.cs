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

namespace WindowsForms
{
    public class WebcamInput
    {
        private static readonly BroadcastBlock<byte[]> buffer = new BroadcastBlock<byte[]>(item => item);
        private static VideoCapture capture; // Takes video from camera as image frames
        private static Task taskConsumer;
        /// <summary>
        /// Enables the input of the webcam
        /// Author: Deividas Brazenas
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
        /// Author: Deividas Brazenas
        /// </summary>
        public static async void DisableWebcam()
        {
            try
            {
                capture.Dispose();
                Application.Idle -= GetFrameAsync;
                buffer.Complete();
                await taskConsumer;
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

                await buffer.SendAsync(FaceRecognition.ImageToByte(imageFrame.Bitmap));
            }
        }

        /// <summary>
        /// Processes frames from the buffer.
        /// Author: Arnas Danaitis
        /// </summary>
        private static async void ProcessFrameAsync()
        {
            FaceRecognition faceRecognition = new FaceRecognition();
            while (await buffer.OutputAvailableAsync())
            {
                byte[] frameToProcess = await buffer.ReceiveAsync();
                Debug.WriteLine("Starting processing of frame");
                var result = FaceRecognition.AnalyzeImage(frameToProcess);
                Debug.WriteLine(DateTime.Now + " " + result);
            }
        }
    }
}
