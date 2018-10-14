using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WindowsForms.FaceAnalysis;
using FaceAnalysis;
using FaceAnalysis.Persons;

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
            // TODO: This line of code loads data into the 'pstop2018DataSet.ContactPersons' table. You can move, or remove it, as needed.
            this.contactPersonsTableAdapter.Fill(this.pstop2018DataSet.ContactPersons);
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
        private async void addMissingPersonButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Verify that properties are valid
                if (!Regex.IsMatch(firstNameBox.Text, @"^[a-zA-Z]+$") || firstNameBox.Text.Equals(""))
                {
                    MessageBox.Show("Missing person First name should contain only letters and cannot be empty!");
                    return;
                }

                if (!Regex.IsMatch(lastNameBox.Text, @"^[a-zA-Z]+$") || lastNameBox.Text.Equals(""))
                {
                    MessageBox.Show("Missing person Last name should contain only letters and cannot be empty!");
                    return;
                }

                if (!Regex.IsMatch(contactFirstNameBox.Text, @"^[a-zA-Z]+$") || contactFirstNameBox.Text.Equals(""))
                {
                    MessageBox.Show("Contact person First name should contain only letters and cannot be empty!");
                    return;
                }

                if (!Regex.IsMatch(contactLastNameBox.Text, @"^[a-zA-Z]+$") || contactLastNameBox.Text.Equals(""))
                {
                    MessageBox.Show("Contact person Last name should contain only letters and cannot be empty!");
                    return;
                }

                if (!Regex.IsMatch(contactPhoneNumberBox.Text, @"^[+][0-9]+$") || contactPhoneNumberBox.Text.Equals(""))
                {
                    MessageBox.Show("Contact person Phone number should be in a valid format(+[Country code]00...00) and cannot be empty!");
                    return;
                }

                if (!Regex.IsMatch(contactEmailAddressBox.Text, @"^([\w\.]+)@([\w]+)((\.(\w){2,3})+)$") || contactEmailAddressBox.Text.Equals(""))
                {
                    MessageBox.Show("Contact person Email address should be in a valid format (foo@bar.baz) and cannot be empty!");
                    return;
                }

                Bitmap missingPersonImage = HelperMethods.ProcessImage(new Bitmap(missingPersonPictureBox.Image));
                MissingPerson missingPerson = new MissingPerson(firstNameBox.Text, lastNameBox.Text, additionalInfoBox.Text, locationBox.Text, dateOfBirthPicker.Value, lastSeenOnPicker.Value, missingPersonImage);
                ContactPerson contactPerson = new ContactPerson(contactFirstNameBox.Text, contactLastNameBox.Text, contactPhoneNumberBox.Text, contactEmailAddressBox.Text);

                switch (await HelperMethods.NumberOfFaces(missingPersonImage))
                {
                    case -1:
                        MessageBox.Show("An error occured while analysing the image, please try again later");
                        break;
                    case 0:
                        MessageBox.Show("Unfortunately, no faces have been detected in the picture! \n" +
                                        "Please try another one.");
                        break;
                    case 1:
                        //add to db here.
                        MessageBox.Show("Face should be added to DB here.");
                        break;
                    default:
                        MessageBox.Show("Unfortunately, more than one face has been detected in the picture! \n" +
                                        "Please try another one.");
                        break;
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
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private void fillByToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.contactPersonsTableAdapter.FillBy(this.pstop2018DataSet.ContactPersons);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }
    }
}