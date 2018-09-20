using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace WindowsForms
{
    class WebcamInput
    {
        private static VideoCapture capture; // Takes video from camera as image frames

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
            using (var imageFrame = capture.QueryFrame().ToImage<Bgr, Byte>())
            {
                FaceDetection.FaceDetectionFromFrame(imageFrame); // Face detection

                var form = FormFaceDetection.Current;

                form.scanPictureBox.Image = imageFrame.Bitmap;
            }
        }
    }
}
