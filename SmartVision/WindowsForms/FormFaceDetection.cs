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
            //homePanel.BringToFront();
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