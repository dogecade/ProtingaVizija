using System;
using System.Collections.Generic;
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
            //homePanel.BringToFront();
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

            scanPictureBox.Image = m.ToImage<Bgr, Byte>().Bitmap; // Sends the frame to the picture box in the GUI
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

        /// <summary>
        /// Main buttons - switching through panels, exiting application
        /// Author: Tomas Drasutis
        /// </summary>
        private void homeButton_Click(object sender, EventArgs e)
        {
            homePanel.BringToFront();
        }

        private void scanButton_Click(object sender, EventArgs e)
        {
            scanPanel.BringToFront();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            homePanel.BringToFront();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addPersonButton_Click(object sender, EventArgs e)
        {
            addPersonPanel.BringToFront();
        }

        /// <summary>
        /// Gets information about the missing person
        /// Author: Tomas Drasutis
        /// </summary>
        private void confirmPersonButton_Click(object sender, EventArgs e)
        {

            try
            {
                string firstName = firstNameBox.Text;
                string lastName = lastNameBox.Text;
                string dateOfBirth = dateOfBirthPicker.Value.ToShortDateString();
                string additionalInformation = adInfoBox.Text;

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

        }

        /// <summary>
        /// Uploads the picture
        /// Author: Tomas Drasutis
        /// </summary>
        private void uploadButton_Click(object sender, EventArgs e)
        {
            String imageLocation = "";

            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "jpg files(*.jpg)|*.jpg| PNG files(*.png)|*.png| All Files(*.*)|*.*";

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    imageLocation = dialog.FileName;
                    personPictureBox.ImageLocation = imageLocation;
                    personPictureBox.BringToFront();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}