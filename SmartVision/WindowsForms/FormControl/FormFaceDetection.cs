using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using FaceAnalysis;
using System.Linq;
using WindowsForms.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using HttpHelpers;
using Wrappers;

namespace WindowsForms.FormControl
{
    public partial class FormFaceDetection : System.Windows.Forms.Form
    {
        private HttpClientWrapper httpClient = new HttpClientWrapper();
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
                    HttpContent content = await httpClient.PostImageToApi(missingPersonImage);
                    string response = await content.ReadAsStringAsync();
                    //post image to api
                    Console.WriteLine("posting image to api" + response);

                    //initialize contact person
                    ContactPerson contact = InitializeContactPerson();

                    //initialize missing person
                    MissingPerson missing = InitializeMissingPerson(response);

                    content = await httpClient.PostContactPersonToApiAsync(contact);
                    string contact1 = await content.ReadAsStringAsync();
                    Console.WriteLine("contact" + contact1);
                    content = await httpClient.PostMissingPersonToApiAsync(missing);
                    string missing1 = await content.ReadAsStringAsync();
                    Console.WriteLine("missing" + missing1);
                    MissingPerson parsedMiss = JsonConvert.DeserializeObject<MissingPerson>(missing1);
                    ContactPerson parsedCont = JsonConvert.DeserializeObject<ContactPerson>(contact1);
                    MissingContact misscont = new MissingContact();
                    misscont.contactPerson = parsedCont;
                    misscont.missingPerson = parsedMiss;
                    //create a relationship between persons
                    Console.WriteLine(await httpClient.PostRelToApi(misscont));
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

        private MissingPerson InitializeMissingPerson(string imgLocation)
        {
            MissingPerson missingPerson = new MissingPerson();
            missingPerson.firstName = firstNameBox.Text;
            missingPerson.lastName = lastNameBox.Text;
            missingPerson.lastSeenDate = lastSeenOnPicker.Value.ToString();
            missingPerson.lastSeenLocation = locationBox.Text;
            missingPerson.Additional_Information = additionalInfoBox.Text;
            missingPerson.dateOfBirth = dateOfBirthPicker.Text;
            missingPerson.faceImg = imgLocation;
            missingPerson.faceToken = faceToken;

            return missingPerson;
        }
        private ContactPerson InitializeContactPerson()
        {
            ContactPerson contactPerson = new ContactPerson();
            contactPerson.firstName = contactFirstNameBox.Text;
            contactPerson.lastName = contactLastNameBox.Text;
            contactPerson.phoneNumber = contactPhoneNumberBox.Text;
            contactPerson.emailAddress = contactEmailAddressBox.Text;

            return contactPerson;
        }

        private void missingPeopleDataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ExtraInfoForm form = new ExtraInfoForm();
            var missingPersonsList = new CallsToDb().GetPeopleData();
            var missingPerson = missingPersonsList.First(x => x.Id == Convert.ToInt32(missingPeopleDataGrid.CurrentRow.Cells[0].Value));
            var contactPerson = missingPerson.ContactPersons.FirstOrDefault();

            form.firstNameBox.Text = missingPerson.firstName;
            form.lastNameBox.Text = missingPerson.lastName;
            form.dateOfBirthPicker.Text = missingPerson.dateOfBirth;
            form.lastSeenOnPicker.Text = missingPerson.lastSeenDate;
            form.locationBox.Text = missingPerson.lastSeenLocation;
            form.additionalInfoBox.Text = missingPerson.Additional_Information;
            form.contactEmailAddressBox.Text = contactPerson.emailAddress;
            form.contactPhoneNumberBox.Text = contactPerson.phoneNumber;
            form.contactLastNameBox.Text = contactPerson.lastName;
            form.contactFirstNameBox.Text = contactPerson.firstName;
            form.missingPersonPictureBox.ImageLocation = missingPerson.faceImg;

            form.ShowDialog();
        }
    }
}