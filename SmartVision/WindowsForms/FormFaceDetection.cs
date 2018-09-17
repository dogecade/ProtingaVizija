using System;
using System.Windows.Forms;

namespace WindowsForms
{
    public partial class FormFaceDetection : Form
    {

        public FormFaceDetection()
        {
            InitializeComponent();

            WebcamInput.EnableWebcam();
        }

        /// <summary>
        /// Dispose the input of the webcam when form is closed
        /// Author: Deividas Brazenas
        /// </summary>
        private void FormFaceDetection_FormClosing(object sender, FormClosedEventArgs e)
        {
            try
            {
                WebcamInput.DisableWebcam();
                FaceDetection.DisableFaceDetection();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}