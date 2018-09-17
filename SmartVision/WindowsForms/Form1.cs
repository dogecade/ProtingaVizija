using System;
<<<<<<< HEAD:SmartVision/WindowsForms/FaceDetection.cs
using System.Drawing;
=======
using System.Windows.Forms;
>>>>>>> master:SmartVision/WindowsForms/Form1.cs
using Emgu.CV;
using Emgu.CV.Structure;

namespace WindowsForms
{
<<<<<<< HEAD:SmartVision/WindowsForms/FaceDetection.cs
    class FaceDetection
    {
        private static CascadeClassifier cascade = new CascadeClassifier(
            @"C:\dev\ProtingaVizija\SmartVision\WindowsForms\XML\haarcascade_frontalface_default.xml"); // Used for face detection

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
=======
    public partial class Form1 : Form
    {
        private VideoCapture capture; // Takes video from camera as image frames

        public Form1()
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
            Mat m = new Mat();
            capture.Retrieve(m);

            pictureBox.Image = m.ToImage<Bgr, Byte>().Bitmap; // Sends the frame to the picture box in the GUI
>>>>>>> master:SmartVision/WindowsForms/Form1.cs
        }

        public static void DisableFaceDetection()
        {
            try
            {
                cascade.Dispose();
            } 
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }

        }
    }
}
