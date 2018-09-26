using System;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace WindowsForms
{
    class FaceDetection
    {
        private static CascadeClassifier cascade = new CascadeClassifier("..\\..\\XML\\haarcascade_frontalface_default.xml"); // Used for face detection

        /// <summary>
        /// Detects a face in a frame and draws a rectangle around it
        /// </summary>
        /// <param name="imageFrame">Frame to analyze</param>
        public static void FaceDetectionFromFrame(Image<Bgr, byte> imageFrame)
        {
            if (imageFrame != null)
            {
                var grayFrame = imageFrame.Convert<Gray, Byte>();

                var faces = cascade.DetectMultiScale(grayFrame, 1.1, 10, Size.Empty); // The actual face detection happens here

                foreach (var face in faces)
                {
                    imageFrame.Draw(face, new Bgr(Color.Blue), 3); // The detected face(s) is(are) highlighted here using a box that is drawn around it/them
                }
            }
        }

        public static void DisableFaceDetection()
        {
            try
            {
                cascade.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Detects a face in a picture and returns an answer
        /// </summary>
        public static bool FaceDetectionFromPicture(Bitmap faceImageBitmap)
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(faceImageBitmap);
            var grayFrame = image.Convert<Gray, Byte>();
            var faces = cascade.DetectMultiScale(grayFrame, 1.1, 10, Size.Empty);

            switch (faces.Length)
            {
                case 0:
                    MessageBox.Show("Unfortunately, no faces have been detected in the picture! \n" +
                   "Please try another one.");
                    return false;
                case 1:
                    return true;
                default:
                    MessageBox.Show("Unfortunately, more than one face has been detected in the picture! \n" +
                   "Please try another one.");
                    return false;
            }
                
        }

    }
}
