using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsForms.FaceAnalysis;
using WindowsForms.FormControl;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Threading.Tasks.Dataflow;
using System.Drawing;

namespace WindowsForms
{
    class WebcamInput
    {
        private static readonly BufferBlock<Bitmap> buffer = new BufferBlock<Bitmap>();
        private static VideoCapture capture; // Takes video from camera as image frames
        private static int frameCount = 0;
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

                frameCount++;

                // put up for analysis every 15th frame
                if (frameCount == 15)
                {
                    frameCount = 0;
                    await buffer.SendAsync(imageFrame.Bitmap);
                    Debug.WriteLine("Adding frame to queue");
                }
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
                Bitmap frameToProcess = await buffer.ReceiveAsync();
                var result = faceRecognition.AnalyzeImage(frameToProcess);
                Debug.WriteLine(DateTime.Now + " " + result);
            }
        }
    }
}
