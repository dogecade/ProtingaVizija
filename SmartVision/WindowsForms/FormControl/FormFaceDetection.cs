using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using FaceAnalysis;
using FaceAnalysis.Persons;
using System.Linq;
using System.Collections.Generic;

namespace WindowsForms.FormControl
{
    public partial class FormFaceDetection : System.Windows.Forms.Form
    {
        private const string enabledButtonText = "Enable scan";
        private const string disabledButtonText = "Disable scan";
        private Color underButtonColor = Color.LightSteelBlue;
        private bool cameraEnabled = false;
        private bool validImage = false;
        public static FormFaceDetection Current { get; private set; }

        public FormFaceDetection()
        {
            InitializeComponent();
            Current = this;
        }

        private void FormFaceDetection_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'pstop2018DataSet2.MissingPersons' table. You can move, or remove it, as needed.
            try
            {
                this.missingPersonsTableAdapter.Fill(this.pstop2018DataSet2.MissingPersons);
            }
            catch (System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("Failed to establish database connection");
            }
            homePanel.BringToFront();
            underHomePanel.BackColor = underButtonColor;
        }

        /// <summary>
        /// Main buttons - switching through panels, exiting application
        /// Author: Tomas Drasutis
        /// </summary>
        public void homeButton_Click(object sender, EventArgs e)
        {
            disableScanPanel();

            try
            {
                this.missingPersonsTableAdapter.Fill(this.pstop2018DataSet2.MissingPersons);
            }
            catch (System.Data.SqlClient.SqlException)
            {

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

            scanPanel.BringToFront();

        }

        private void addPersonButton_Click(object sender, EventArgs e)
        {
            disableScanPanel();

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

                //this might be needed for a picture upload in the future.
                Bitmap missingPersonImage = HelperMethods.ProcessImage(new Bitmap(missingPersonPictureBox.Image));

                if (validImage)
                {
                    //add to db here.
                    using (Api.Models.pstop2018Entities1 db = new Api.Models.pstop2018Entities1())
                    {
                        db.MissingPersons.Add(InitializeMissingPerson());
                        db.ContactPersons.Add(InitializeContactPerson(db.MissingPersons.Max(p => p.Id)));
                        db.SaveChanges();
                        MessageBox.Show("Missing person submitted successfully.");
                    }
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
        private async void uploadButton_Click(object sender, EventArgs e)
        {
            Bitmap uploadedImage = ImageUpload.UploadImage();
            if (uploadedImage == null)
                return;
            List<Rectangle> faceRectangles = await HelperMethods.FaceRectangleList((Bitmap)uploadedImage.Clone());
            switch (faceRectangles.Count)
            {
                case -1:
                    MessageBox.Show("An error occured while analysing the image, please try again later");
                    validImage = false;
                    missingPersonPictureBox.Image = null;
                    break;
                case 0:
                    MessageBox.Show("Unfortunately, no faces have been detected in the picture! \n" +
                                    "Please try another one.");
                    validImage = false;
                    missingPersonPictureBox.Image = null;
                    break;
                case 1:
                    validImage = true;
                    //missingPersonPictureBox.Image = uploadedImage;
                    missingPersonPictureBox.Image = HelperMethods.CropImage(uploadedImage, faceRectangles[0], 25);
                    break;
                default:
                    MessageBox.Show("Unfortunately, more than one face has been detected in the picture! \n" +
                                    "Please try another one.");
                    validImage = false;
                    missingPersonPictureBox.Image = null;
                    break;
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
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private void useWebcamPragueBox_CheckedChanged(object sender, EventArgs e)
        {
            cameraUrlBox.Enabled = !useWebcamPragueBox.Checked;
        }

        private void activateScanButton_Click(object sender, EventArgs e)
        {
            if (!cameraEnabled)
            {
                cameraEnabled =
                    useWebcamPragueBox.Checked ? WebcamInput.EnableWebcam() : WebcamInput.EnableWebcam(cameraUrlBox.Text);
                useWebcamPragueBox.Enabled = !cameraEnabled;
                cameraUrlBox.Enabled = !cameraEnabled;
            }
            else
                disableScanPanel();
            activateScanButton.Text = cameraEnabled ? disabledButtonText : enabledButtonText;
        }

        private void disableScanPanel()
        {
            if (cameraEnabled)
            {
                WebcamInput.DisableWebcam();
                Current.scanPictureBox.Image = null;
                cameraEnabled = false;
                activateScanButton.Text = enabledButtonText;
                useWebcamPragueBox.Enabled = true;
                cameraUrlBox.Enabled = !useWebcamPragueBox.Checked;
            }
        }

        private Api.Models.MissingPerson InitializeMissingPerson()
        {
            Api.Models.MissingPerson missingPerson = new Api.Models.MissingPerson();
            missingPerson.firstName = firstNameBox.Text;
            missingPerson.lastName = lastNameBox.Text;
            missingPerson.lastSeenDate = lastSeenOnPicker.Value.ToString();
            missingPerson.lastSeenLocation = locationBox.Text;
            missingPerson.Additional_Information = additionalInfoBox.Text;

            return missingPerson;
        }
        private Api.Models.ContactPerson InitializeContactPerson(int missingPersonId)
        {
            Api.Models.ContactPerson contactPerson = new Api.Models.ContactPerson();
            contactPerson.firstName = contactFirstNameBox.Text;
            contactPerson.lastName = contactLastNameBox.Text;
            contactPerson.missingPersonId = (missingPersonId + 1).ToString();
            contactPerson.phoneNumber = contactPhoneNumberBox.Text;
            contactPerson.emailAddress = contactEmailAddressBox.Text;

            return contactPerson;
        }

        private void missingPeopleDataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ExtraInfoForm form = new ExtraInfoForm();
            var Id = Convert.ToInt32(missingPeopleDataGrid.CurrentRow.Cells[0].Value);
            var stringId = Id.ToString();

            using (Api.Models.pstop2018Entities1 db = new Api.Models.pstop2018Entities1())
            {
                Api.Models.MissingPerson missingPerson = db.MissingPersons.Find(Id);
                Api.Models.ContactPerson contactPerson = db.ContactPersons.FirstOrDefault(f => f.missingPersonId == stringId);

                form.firstNameBox.Text = missingPerson.firstName;
                form.lastNameBox.Text = missingPerson.lastName;
                form.lastSeenOnPicker.Text = missingPerson.lastSeenDate;
                form.locationBox.Text = missingPerson.lastSeenLocation;
                form.additionalInfoBox.Text = missingPerson.Additional_Information;
                form.contactEmailAddressBox.Text = contactPerson.emailAddress;
                form.contactPhoneNumberBox.Text = contactPerson.phoneNumber;
                form.contactLastNameBox.Text = contactPerson.lastName;
                form.contactFirstNameBox.Text = contactPerson.firstName;
                //form.dateOfBirthPicker.Text = ;

                form.ShowDialog();
            }
        }
    }
}