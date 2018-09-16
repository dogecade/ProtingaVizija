using System;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace WindowsForms
{
    public partial class FaceDetection : Form
    {
        private VideoCapture capture; // Takes video from camera as image frames
        private CascadeClassifier cascade = new CascadeClassifier(
                @"C:\dev\ProtingaVizija\SmartVision\WindowsForms\XML\haarcascade_frontalface_default.xml"); // Used for face detection

        public FaceDetection()
        {
            InitializeComponent();

            EnableWebcam();
        }

        /// <summary>
        /// Enables the input of the webcam
        /// Author: Deividas Brazenas
        /// </summary>
        private void EnableWebcam()
        {
            try
            {
                capture = new VideoCapture();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            Application.Idle += ProcessFrame;
        }

        /// <summary>
        /// Processes the frame from webcam input
        /// Author: Deividas Brazenas
        /// </summary>
        private void ProcessFrame(object sender, EventArgs e)
        {

            using (var imageFrame = capture.QueryFrame().ToImage<Bgr, Byte>())
            {
                FaceDetectionFromFrame(imageFrame); // Face detection

                pictureBox.Image = imageFrame.Bitmap; 
            }
        }

        /// <summary>
        /// Detects a face in a frame and draws a rectangle around it
        /// </summary>
        /// <param name="imageFrame">Frame to analyze</param>
        private void FaceDetectionFromFrame(Image<Bgr, byte> imageFrame)
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

        /// <summary>
        /// Dispose the input of the webcam when form is closed
        /// Author: Deividas Brazenas
        /// </summary>
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                capture.Dispose();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}