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
        private const string nameRegex = @"^[a-zA-Z]+$";
        private const string phoneRegex = @"^[+][0-9]+$";
        private const string emailRegex = @"^([\w\.]+)@([\w]+)((\.(\w){2,3})+)$";
        private Color underButtonColor = Color.LightSteelBlue;
        private bool cameraEnabled = false;
        private bool validImage = false;
        private string faceToken;
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
                MessageBox.Show(Messages.failedDbConnection);
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
        private async void addMissingPersonButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Verify that properties are valid
                if (!Regex.IsMatch(firstNameBox.Text, nameRegex) || firstNameBox.Text.Equals(""))
                {
                    MessageBox.Show(Messages.incorrectMissingFirstNamePattern);
                    return;
                }

                if (!Regex.IsMatch(lastNameBox.Text, nameRegex) || lastNameBox.Text.Equals(""))
                {
                    MessageBox.Show(Messages.incorrectLastNamePattern);
                    return;
                }

                if (!Regex.IsMatch(contactFirstNameBox.Text, nameRegex) || contactFirstNameBox.Text.Equals(""))
                {
                    MessageBox.Show(Messages.incorrectContactFirstNamePattern);
                    return;
                }

                if (!Regex.IsMatch(contactLastNameBox.Text, nameRegex) || contactLastNameBox.Text.Equals(""))
                {
                    MessageBox.Show(Messages.incorrectContactLastNamePattern);
                    return;
                }

                if (!Regex.IsMatch(contactPhoneNumberBox.Text, phoneRegex) || contactPhoneNumberBox.Text.Equals(""))
                {
                    MessageBox.Show(Messages.incorrectPhoneNumberPattern);
                    return;
                }

                if (!Regex.IsMatch(contactEmailAddressBox.Text, emailRegex) || contactEmailAddressBox.Text.Equals(""))
                {
                    MessageBox.Show(Messages.incorrectEmailPattern);
                    return;
                }

                //this might be needed for a picture upload in the future.
                Bitmap missingPersonImage = new Bitmap(missingPersonPictureBox.Image);

                if (validImage)
                {
                    if (await new FaceApiCalls(new HttpClientWrapper()).AddFaceToFaceset(FaceAnalysis.Keys.facesetToken, faceToken) == null)
                    {
                        //TODO: have some proper things to do here.
                        throw new SystemException(Messages.invalidApiResponse);
                    }

                    //add to db here.
                    using (Api.Models.pstop2018Entities1 db = new Api.Models.pstop2018Entities1())
                    {
                        db.MissingPersons.Add(InitializeMissingPerson());
                        db.ContactPersons.Add(InitializeContactPerson(db.MissingPersons.Max(p => p.Id)));
                        db.SaveChanges();
                    }
                    MessageBox.Show(Messages.personSubmitSuccessful);
                }
                else
                    MessageBox.Show(Messages.invalidImage);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                MessageBox.Show(Messages.errorWhileSavingPerson);
            }

        }

        /// <summary>
        /// Uploads the picture
        /// Author: Tomas Drasutis
        /// </summary>
        private async void uploadButton_Click(object sender, EventArgs e)
        {
            int chosenImageIndex = 0;
            Bitmap uploadedImage = ImageUpload.UploadImage();
            if (uploadedImage == null)
                return;
            else
                uploadedImage = HelperMethods.ProcessImage(uploadedImage);
            FrameAnalysisJSON result = await FaceProcessor.ProcessFrame((Bitmap)uploadedImage.Clone());
            missingPersonPictureBox.Image?.Dispose();
            missingPersonPictureBox.Image = null;
            if (result == null)
            {
                MessageBox.Show(Messages.errorWhileAnalysingImage);
                validImage = false;
                return;
            }
            switch (result.Faces.Count)
            {
                case 0:
                    MessageBox.Show(Messages.noFacesInImage);
                    validImage = false;
                    uploadedImage.Dispose();
                    return;
                case 1:
                    break;
                default:
                    var chooseFaceForm = new ChooseFaceFormcs(result.Faces, uploadedImage);
                    chooseFaceForm.ShowDialog();
                    if (chooseFaceForm.DialogResult == DialogResult.OK)
                    {
                        chosenImageIndex = chooseFaceForm.SelectedFace;
                        chooseFaceForm.Dispose();
                    }
                    else
                    {
                        return;
                    }
                    break;
            }
            validImage = true;
            missingPersonPictureBox.Image = HelperMethods.CropImage(uploadedImage, result.Faces[chosenImageIndex].Face_rectangle, 25);
            faceToken = result.Faces[chosenImageIndex].Face_token;
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
            missingPerson.faceToken = faceToken;

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

                form.firstNameBox.Text = missingPerson.firstName;
                form.lastNameBox.Text = missingPerson.lastName;
                form.dateOfBirthPicker.Text = missingPerson.dateOfBirth ?? "";
                form.lastSeenOnPicker.Text = missingPerson.lastSeenDate ?? "";
                form.locationBox.Text = missingPerson.lastSeenLocation ?? "";
                form.additionalInfoBox.Text = missingPerson.Additional_Information ?? "";

                form.ShowDialog();
            }
        }
    }
}