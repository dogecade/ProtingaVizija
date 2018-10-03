using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsForms.FaceAnalysis;
using WindowsForms.Persons;

namespace WindowsForms.FormControl
{
    public partial class FormFaceDetection : System.Windows.Forms.Form
    {
        private Color underButtonColor = Color.LightSteelBlue;
        private bool cameraEnabled = false;
        public static FormFaceDetection Current { get; private set; }

        public FormFaceDetection()
        {
            InitializeComponent();
            Current = this;
        }

        private void FormFaceDetection_Load(object sender, EventArgs e)
        {
            homePanel.BringToFront();
            underHomePanel.BackColor = underButtonColor;
        }

        /// <summary>
        /// Main buttons - switching through panels, exiting application
        /// Author: Tomas Drasutis
        /// </summary>
        public void homeButton_Click(object sender, EventArgs e)
        {
            if (cameraEnabled)
            {
                WebcamInput.DisableWebcam();
                cameraEnabled = false;
            }

            homePanel.BringToFront();

            underHomePanel.BackColor = underButtonColor;
            underScanPanel.BackColor = Color.Transparent;
            underPersonPanel.BackColor = Color.Transparent;
            underExitPanel.BackColor = Color.Transparent;
        }

        private void scanButton_Click(object sender, EventArgs e)
        {

            underHomePanel.BackColor = Color.Transparent;
            underScanPanel.BackColor = underButtonColor;
            underPersonPanel.BackColor = Color.Transparent;
            underExitPanel.BackColor = Color.Transparent;

            if (!cameraEnabled && WebcamInput.EnableWebcam())
            {
                scanPanel.BringToFront();
                cameraEnabled = true;
            }

            else
            {
                homePanel.BringToFront();
            }
        }

        private void addPersonButton_Click(object sender, EventArgs e)
        {
            if (cameraEnabled)
            {
                WebcamInput.DisableWebcam();
                cameraEnabled = false;
            }

            addPersonPanel.BringToFront();

            underHomePanel.BackColor = Color.Transparent;
            underScanPanel.BackColor = Color.Transparent;
            underPersonPanel.BackColor = underButtonColor;
            underExitPanel.BackColor = Color.Transparent;
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Gets information about the missing person
        /// Author: Tomas Drasutis
        /// </summary>
        private void addMissingPersonButton_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap missingPersonImage = new Bitmap(missingPersonPictureBox.Image);
                MissingPerson missingPerson = new MissingPerson(firstNameBox.Text, lastNameBox.Text, additionalInfoBox.Text, locationBox.Text, dateOfBirthPicker.Value, lastSeenOnPicker.Value, missingPersonImage);
                ContactPerson contactPerson = new ContactPerson(contactFirstNameBox.Text, contactLastNameBox.Text, contactPhoneNumberBox.Text, contactEmailAddressBox.Text);

                if (FaceDetection.FaceDetectionFromPicture(missingPersonImage))
                {
                    //to put to database
                }
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
            ImageUpload.UploadImage();
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