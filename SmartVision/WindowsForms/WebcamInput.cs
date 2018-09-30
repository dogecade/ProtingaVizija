using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsForms.FaceAnalysis;
using WindowsForms.FormControl;
using Emgu.CV;
using Emgu.CV.Structure;

namespace WindowsForms
{
    class WebcamInput
    {
        private static VideoCapture capture; // Takes video from camera as image frames
        private static int frameCount = 0;
        private static Task<string> analyzeTask;
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
                var popup = new FormPopup();
                popup.ShowDialog();
                return false;
            }

            Application.Idle += ProcessFrame;
            return true;
        }

        /// <summary>
        /// Disables the input of the webcam
        /// Author: Deividas Brazenas
        /// </summary>
        public static void DisableWebcam()
        {
            try
            {
                capture.Dispose();
                Application.Idle -= ProcessFrame;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Processes the frame from webcam input
        /// Author: Deividas Brazenas
        /// </summary>
        private static void ProcessFrame(object sender, EventArgs e)
        {
            FaceRecognition faceRecognition = new FaceRecognition();
            using (var imageFrame = capture.QueryFrame().ToImage<Bgr, Byte>())
            {
                var form = FormFaceDetection.Current;
                form.scanPictureBox.Image = imageFrame.Bitmap;

                frameCount++;

                // Analyze every 15th frame
                if (frameCount == 15)
                {
                    frameCount = 0;

                    analyzeTask = Task.Run(() =>
                    {
                        return faceRecognition.AnalyzeImage(imageFrame.Bitmap);
                    });
                    analyzeTask.Wait(100);

                    Debug.WriteLine(DateTime.Now + " " + analyzeTask.Result);
                }
            }
        }
    }
}
