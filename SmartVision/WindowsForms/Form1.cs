using System;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace WindowsForms
{
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