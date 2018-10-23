namespace WindowsForms.FormControl
{
    partial class FormFaceDetection
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFaceDetection));
            this.homePanel = new System.Windows.Forms.Panel();
            this.youCanHelpLabel = new System.Windows.Forms.Label();
            this.missingPeopleDataGrid = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.faceTokenDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.firstNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastSeenDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastSeenLocationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.additionalInformationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.missingPersonsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.pstop2018DataSet2 = new WindowsForms.pstop2018DataSet2();
            this.currentlyMissingLabel = new System.Windows.Forms.Label();
            this.homeLabel = new System.Windows.Forms.Label();
            this.scanPanel = new System.Windows.Forms.Panel();
            this.cameraUrlLabel = new System.Windows.Forms.Label();
            this.activateScanButton = new System.Windows.Forms.Button();
            this.cameraUrlBox = new System.Windows.Forms.TextBox();
            this.useWebcamPragueBox = new System.Windows.Forms.CheckBox();
            this.scanPictureBox = new System.Windows.Forms.PictureBox();
            this.addPersonPanel = new System.Windows.Forms.Panel();
            this.contactLabel = new System.Windows.Forms.Label();
            this.contactEmailAddressBox = new MetroFramework.Controls.MetroTextBox();
            this.contactEmailAddressLabel = new System.Windows.Forms.Label();
            this.contactPhoneNumberBox = new MetroFramework.Controls.MetroTextBox();
            this.contactPhoneNumberLabel = new System.Windows.Forms.Label();
            this.contactLastNameBox = new MetroFramework.Controls.MetroTextBox();
            this.contactLastNameLabel = new System.Windows.Forms.Label();
            this.contactFirstNameBox = new MetroFramework.Controls.MetroTextBox();
            this.contactFirstNameLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.locationBox = new MetroFramework.Controls.MetroTextBox();
            this.lastSeenOn = new System.Windows.Forms.Label();
            this.lastSeenOnPicker = new System.Windows.Forms.DateTimePicker();
            this.uploadButton = new System.Windows.Forms.Button();
            this.additionalInfoBox = new MetroFramework.Controls.MetroTextBox();
            this.missingPersonPictureBox = new System.Windows.Forms.PictureBox();
            this.dateOfBirthPicker = new System.Windows.Forms.DateTimePicker();
            this.addMissingPersonButton = new System.Windows.Forms.Button();
            this.adInfoLabel = new System.Windows.Forms.Label();
            this.lastNameBox = new MetroFramework.Controls.MetroTextBox();
            this.firstNameBox = new MetroFramework.Controls.MetroTextBox();
            this.dateOfBirthLabel = new System.Windows.Forms.Label();
            this.LastNameLabel = new System.Windows.Forms.Label();
            this.firstNameLabel = new System.Windows.Forms.Label();
            this.addPersonLabel = new System.Windows.Forms.Label();
            this.homeButton = new System.Windows.Forms.Button();
            this.scanButton = new System.Windows.Forms.Button();
            this.addPersonButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.underHomePanel = new System.Windows.Forms.Panel();
            this.underScanPanel = new System.Windows.Forms.Panel();
            this.underPersonPanel = new System.Windows.Forms.Panel();
            this.underExitPanel = new System.Windows.Forms.Panel();
            this.buttonsPanel = new System.Windows.Forms.Panel();
            this.missingPersonsTableAdapter = new WindowsForms.pstop2018DataSet2TableAdapters.MissingPersonsTableAdapter();
            this.homePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.missingPeopleDataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.missingPersonsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pstop2018DataSet2)).BeginInit();
            this.scanPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scanPictureBox)).BeginInit();
            this.addPersonPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.missingPersonPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.buttonsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // homePanel
            // 
            this.homePanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.homePanel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.homePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.homePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.homePanel.Controls.Add(this.youCanHelpLabel);
            this.homePanel.Controls.Add(this.missingPeopleDataGrid);
            this.homePanel.Controls.Add(this.currentlyMissingLabel);
            this.homePanel.Controls.Add(this.homeLabel);
            this.homePanel.Location = new System.Drawing.Point(148, 131);
            this.homePanel.Margin = new System.Windows.Forms.Padding(2);
            this.homePanel.Name = "homePanel";
            this.homePanel.Size = new System.Drawing.Size(539, 460);
            this.homePanel.TabIndex = 1;
            // 
            // youCanHelpLabel
            // 
            this.youCanHelpLabel.AutoSize = true;
            this.youCanHelpLabel.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.youCanHelpLabel.Location = new System.Drawing.Point(121, 414);
            this.youCanHelpLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.youCanHelpLabel.Name = "youCanHelpLabel";
            this.youCanHelpLabel.Size = new System.Drawing.Size(267, 30);
            this.youCanHelpLabel.TabIndex = 9;
            this.youCanHelpLabel.Text = "You can help us find them!";
            // 
            // missingPeopleDataGrid
            // 
            this.missingPeopleDataGrid.AllowUserToAddRows = false;
            this.missingPeopleDataGrid.AllowUserToDeleteRows = false;
            this.missingPeopleDataGrid.AllowUserToResizeColumns = false;
            this.missingPeopleDataGrid.AllowUserToResizeRows = false;
            this.missingPeopleDataGrid.AutoGenerateColumns = false;
            this.missingPeopleDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.missingPeopleDataGrid.BackgroundColor = System.Drawing.Color.NavajoWhite;
            this.missingPeopleDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.missingPeopleDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.faceTokenDataGridViewTextBoxColumn,
            this.firstNameDataGridViewTextBoxColumn,
            this.lastNameDataGridViewTextBoxColumn,
            this.lastSeenDateDataGridViewTextBoxColumn,
            this.lastSeenLocationDataGridViewTextBoxColumn,
            this.additionalInformationDataGridViewTextBoxColumn});
            this.missingPeopleDataGrid.DataSource = this.missingPersonsBindingSource;
            this.missingPeopleDataGrid.Location = new System.Drawing.Point(59, 101);
            this.missingPeopleDataGrid.Margin = new System.Windows.Forms.Padding(2);
            this.missingPeopleDataGrid.Name = "missingPeopleDataGrid";
            this.missingPeopleDataGrid.ReadOnly = true;
            this.missingPeopleDataGrid.RowTemplate.Height = 24;
            this.missingPeopleDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.missingPeopleDataGrid.Size = new System.Drawing.Size(435, 296);
            this.missingPeopleDataGrid.TabIndex = 8;
            this.missingPeopleDataGrid.TabStop = false;
            this.missingPeopleDataGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.missingPeopleDataGrid_CellDoubleClick);
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.Width = 42;
            // 
            // faceTokenDataGridViewTextBoxColumn
            // 
            this.faceTokenDataGridViewTextBoxColumn.DataPropertyName = "faceToken";
            this.faceTokenDataGridViewTextBoxColumn.HeaderText = "faceToken";
            this.faceTokenDataGridViewTextBoxColumn.Name = "faceTokenDataGridViewTextBoxColumn";
            this.faceTokenDataGridViewTextBoxColumn.ReadOnly = true;
            this.faceTokenDataGridViewTextBoxColumn.Width = 83;
            // 
            // firstNameDataGridViewTextBoxColumn
            // 
            this.firstNameDataGridViewTextBoxColumn.DataPropertyName = "firstName";
            this.firstNameDataGridViewTextBoxColumn.HeaderText = "firstName";
            this.firstNameDataGridViewTextBoxColumn.Name = "firstNameDataGridViewTextBoxColumn";
            this.firstNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.firstNameDataGridViewTextBoxColumn.Width = 81;
            // 
            // lastNameDataGridViewTextBoxColumn
            // 
            this.lastNameDataGridViewTextBoxColumn.DataPropertyName = "lastName";
            this.lastNameDataGridViewTextBoxColumn.HeaderText = "lastName";
            this.lastNameDataGridViewTextBoxColumn.Name = "lastNameDataGridViewTextBoxColumn";
            this.lastNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastNameDataGridViewTextBoxColumn.Width = 79;
            // 
            // lastSeenDateDataGridViewTextBoxColumn
            // 
            this.lastSeenDateDataGridViewTextBoxColumn.DataPropertyName = "lastSeenDate";
            this.lastSeenDateDataGridViewTextBoxColumn.HeaderText = "lastSeenDate";
            this.lastSeenDateDataGridViewTextBoxColumn.Name = "lastSeenDateDataGridViewTextBoxColumn";
            this.lastSeenDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastSeenDateDataGridViewTextBoxColumn.Width = 99;
            // 
            // lastSeenLocationDataGridViewTextBoxColumn
            // 
            this.lastSeenLocationDataGridViewTextBoxColumn.DataPropertyName = "lastSeenLocation";
            this.lastSeenLocationDataGridViewTextBoxColumn.HeaderText = "lastSeenLocation";
            this.lastSeenLocationDataGridViewTextBoxColumn.Name = "lastSeenLocationDataGridViewTextBoxColumn";
            this.lastSeenLocationDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastSeenLocationDataGridViewTextBoxColumn.Width = 119;
            // 
            // additionalInformationDataGridViewTextBoxColumn
            // 
            this.additionalInformationDataGridViewTextBoxColumn.DataPropertyName = "Additional_Information";
            this.additionalInformationDataGridViewTextBoxColumn.HeaderText = "Additional_Information";
            this.additionalInformationDataGridViewTextBoxColumn.Name = "additionalInformationDataGridViewTextBoxColumn";
            this.additionalInformationDataGridViewTextBoxColumn.ReadOnly = true;
            this.additionalInformationDataGridViewTextBoxColumn.Width = 152;
            // 
            // missingPersonsBindingSource
            // 
            this.missingPersonsBindingSource.DataMember = "MissingPersons";
            this.missingPersonsBindingSource.DataSource = this.pstop2018DataSet2;
            // 
            // pstop2018DataSet2
            // 
            this.pstop2018DataSet2.DataSetName = "pstop2018DataSet2";
            this.pstop2018DataSet2.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // currentlyMissingLabel
            // 
            this.currentlyMissingLabel.AutoSize = true;
            this.currentlyMissingLabel.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.currentlyMissingLabel.Location = new System.Drawing.Point(56, 50);
            this.currentlyMissingLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.currentlyMissingLabel.Name = "currentlyMissingLabel";
            this.currentlyMissingLabel.Size = new System.Drawing.Size(226, 25);
            this.currentlyMissingLabel.TabIndex = 7;
            this.currentlyMissingLabel.Text = "Currently missing people:";
            // 
            // homeLabel
            // 
            this.homeLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.homeLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.homeLabel.ForeColor = System.Drawing.Color.White;
            this.homeLabel.Location = new System.Drawing.Point(133, 9);
            this.homeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.homeLabel.Name = "homeLabel";
            this.homeLabel.Size = new System.Drawing.Size(281, 19);
            this.homeLabel.TabIndex = 0;
            this.homeLabel.Text = "Main page";
            this.homeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // scanPanel
            // 
            this.scanPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.scanPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scanPanel.Controls.Add(this.cameraUrlLabel);
            this.scanPanel.Controls.Add(this.activateScanButton);
            this.scanPanel.Controls.Add(this.cameraUrlBox);
            this.scanPanel.Controls.Add(this.useWebcamPragueBox);
            this.scanPanel.Controls.Add(this.scanPictureBox);
            this.scanPanel.Location = new System.Drawing.Point(148, 131);
            this.scanPanel.Margin = new System.Windows.Forms.Padding(2);
            this.scanPanel.Name = "scanPanel";
            this.scanPanel.Size = new System.Drawing.Size(539, 460);
            this.scanPanel.TabIndex = 2;
            // 
            // cameraUrlLabel
            // 
            this.cameraUrlLabel.AutoSize = true;
            this.cameraUrlLabel.BackColor = System.Drawing.Color.Transparent;
            this.cameraUrlLabel.Location = new System.Drawing.Point(121, 25);
            this.cameraUrlLabel.Name = "cameraUrlLabel";
            this.cameraUrlLabel.Size = new System.Drawing.Size(78, 13);
            this.cameraUrlLabel.TabIndex = 4;
            this.cameraUrlLabel.Text = "IP camera URL";
            // 
            // activateScanButton
            // 
            this.activateScanButton.Location = new System.Drawing.Point(429, 4);
            this.activateScanButton.Name = "activateScanButton";
            this.activateScanButton.Size = new System.Drawing.Size(93, 23);
            this.activateScanButton.TabIndex = 3;
            this.activateScanButton.Text = "Activate scan";
            this.activateScanButton.UseVisualStyleBackColor = true;
            this.activateScanButton.Click += new System.EventHandler(this.activateScanButton_Click);
            // 
            // cameraUrlBox
            // 
            this.cameraUrlBox.Enabled = false;
            this.cameraUrlBox.Location = new System.Drawing.Point(122, 5);
            this.cameraUrlBox.Name = "cameraUrlBox";
            this.cameraUrlBox.Size = new System.Drawing.Size(292, 21);
            this.cameraUrlBox.TabIndex = 2;
            // 
            // useWebcamPragueBox
            // 
            this.useWebcamPragueBox.AutoSize = true;
            this.useWebcamPragueBox.Checked = true;
            this.useWebcamPragueBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useWebcamPragueBox.Location = new System.Drawing.Point(7, 6);
            this.useWebcamPragueBox.Name = "useWebcamPragueBox";
            this.useWebcamPragueBox.Size = new System.Drawing.Size(90, 17);
            this.useWebcamPragueBox.TabIndex = 1;
            this.useWebcamPragueBox.Text = "Use webcam";
            this.useWebcamPragueBox.UseVisualStyleBackColor = true;
            this.useWebcamPragueBox.CheckedChanged += new System.EventHandler(this.useWebcamPragueBox_CheckedChanged);
            // 
            // scanPictureBox
            // 
            this.scanPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scanPictureBox.Location = new System.Drawing.Point(-1, 46);
            this.scanPictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.scanPictureBox.Name = "scanPictureBox";
            this.scanPictureBox.Size = new System.Drawing.Size(540, 414);
            this.scanPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.scanPictureBox.TabIndex = 0;
            this.scanPictureBox.TabStop = false;
            // 
            // addPersonPanel
            // 
            this.addPersonPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.addPersonPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.addPersonPanel.Controls.Add(this.contactLabel);
            this.addPersonPanel.Controls.Add(this.contactEmailAddressBox);
            this.addPersonPanel.Controls.Add(this.contactEmailAddressLabel);
            this.addPersonPanel.Controls.Add(this.contactPhoneNumberBox);
            this.addPersonPanel.Controls.Add(this.contactPhoneNumberLabel);
            this.addPersonPanel.Controls.Add(this.contactLastNameBox);
            this.addPersonPanel.Controls.Add(this.contactLastNameLabel);
            this.addPersonPanel.Controls.Add(this.contactFirstNameBox);
            this.addPersonPanel.Controls.Add(this.contactFirstNameLabel);
            this.addPersonPanel.Controls.Add(this.label1);
            this.addPersonPanel.Controls.Add(this.locationBox);
            this.addPersonPanel.Controls.Add(this.lastSeenOn);
            this.addPersonPanel.Controls.Add(this.lastSeenOnPicker);
            this.addPersonPanel.Controls.Add(this.uploadButton);
            this.addPersonPanel.Controls.Add(this.additionalInfoBox);
            this.addPersonPanel.Controls.Add(this.missingPersonPictureBox);
            this.addPersonPanel.Controls.Add(this.dateOfBirthPicker);
            this.addPersonPanel.Controls.Add(this.addMissingPersonButton);
            this.addPersonPanel.Controls.Add(this.adInfoLabel);
            this.addPersonPanel.Controls.Add(this.lastNameBox);
            this.addPersonPanel.Controls.Add(this.firstNameBox);
            this.addPersonPanel.Controls.Add(this.dateOfBirthLabel);
            this.addPersonPanel.Controls.Add(this.LastNameLabel);
            this.addPersonPanel.Controls.Add(this.firstNameLabel);
            this.addPersonPanel.Controls.Add(this.addPersonLabel);
            this.addPersonPanel.Location = new System.Drawing.Point(148, 131);
            this.addPersonPanel.Margin = new System.Windows.Forms.Padding(2);
            this.addPersonPanel.Name = "addPersonPanel";
            this.addPersonPanel.Size = new System.Drawing.Size(539, 460);
            this.addPersonPanel.TabIndex = 2;
            // 
            // contactLabel
            // 
            this.contactLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.contactLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.contactLabel.ForeColor = System.Drawing.Color.White;
            this.contactLabel.Location = new System.Drawing.Point(133, 290);
            this.contactLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.contactLabel.Name = "contactLabel";
            this.contactLabel.Size = new System.Drawing.Size(281, 19);
            this.contactLabel.TabIndex = 26;
            this.contactLabel.Text = "Your contact information";
            this.contactLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // contactEmailAddressBox
            // 
            // 
            // 
            // 
            this.contactEmailAddressBox.CustomButton.Image = null;
            this.contactEmailAddressBox.CustomButton.Location = new System.Drawing.Point(148, 2);
            this.contactEmailAddressBox.CustomButton.Margin = new System.Windows.Forms.Padding(2);
            this.contactEmailAddressBox.CustomButton.Name = "";
            this.contactEmailAddressBox.CustomButton.Size = new System.Drawing.Size(13, 13);
            this.contactEmailAddressBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.contactEmailAddressBox.CustomButton.TabIndex = 1;
            this.contactEmailAddressBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.contactEmailAddressBox.CustomButton.UseSelectable = true;
            this.contactEmailAddressBox.CustomButton.Visible = false;
            this.contactEmailAddressBox.Lines = new string[0];
            this.contactEmailAddressBox.Location = new System.Drawing.Point(148, 399);
            this.contactEmailAddressBox.Margin = new System.Windows.Forms.Padding(2);
            this.contactEmailAddressBox.MaxLength = 32767;
            this.contactEmailAddressBox.Name = "contactEmailAddressBox";
            this.contactEmailAddressBox.PasswordChar = '\0';
            this.contactEmailAddressBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.contactEmailAddressBox.SelectedText = "";
            this.contactEmailAddressBox.SelectionLength = 0;
            this.contactEmailAddressBox.SelectionStart = 0;
            this.contactEmailAddressBox.ShortcutsEnabled = true;
            this.contactEmailAddressBox.Size = new System.Drawing.Size(164, 18);
            this.contactEmailAddressBox.TabIndex = 25;
            this.contactEmailAddressBox.UseSelectable = true;
            this.contactEmailAddressBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.contactEmailAddressBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // contactEmailAddressLabel
            // 
            this.contactEmailAddressLabel.AutoSize = true;
            this.contactEmailAddressLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.contactEmailAddressLabel.Location = new System.Drawing.Point(42, 399);
            this.contactEmailAddressLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.contactEmailAddressLabel.Name = "contactEmailAddressLabel";
            this.contactEmailAddressLabel.Size = new System.Drawing.Size(77, 13);
            this.contactEmailAddressLabel.TabIndex = 24;
            this.contactEmailAddressLabel.Text = "Email address";
            // 
            // contactPhoneNumberBox
            // 
            // 
            // 
            // 
            this.contactPhoneNumberBox.CustomButton.Image = null;
            this.contactPhoneNumberBox.CustomButton.Location = new System.Drawing.Point(148, 2);
            this.contactPhoneNumberBox.CustomButton.Margin = new System.Windows.Forms.Padding(2);
            this.contactPhoneNumberBox.CustomButton.Name = "";
            this.contactPhoneNumberBox.CustomButton.Size = new System.Drawing.Size(13, 13);
            this.contactPhoneNumberBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.contactPhoneNumberBox.CustomButton.TabIndex = 1;
            this.contactPhoneNumberBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.contactPhoneNumberBox.CustomButton.UseSelectable = true;
            this.contactPhoneNumberBox.CustomButton.Visible = false;
            this.contactPhoneNumberBox.Lines = new string[0];
            this.contactPhoneNumberBox.Location = new System.Drawing.Point(148, 373);
            this.contactPhoneNumberBox.Margin = new System.Windows.Forms.Padding(2);
            this.contactPhoneNumberBox.MaxLength = 32767;
            this.contactPhoneNumberBox.Name = "contactPhoneNumberBox";
            this.contactPhoneNumberBox.PasswordChar = '\0';
            this.contactPhoneNumberBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.contactPhoneNumberBox.SelectedText = "";
            this.contactPhoneNumberBox.SelectionLength = 0;
            this.contactPhoneNumberBox.SelectionStart = 0;
            this.contactPhoneNumberBox.ShortcutsEnabled = true;
            this.contactPhoneNumberBox.Size = new System.Drawing.Size(164, 18);
            this.contactPhoneNumberBox.TabIndex = 23;
            this.contactPhoneNumberBox.UseSelectable = true;
            this.contactPhoneNumberBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.contactPhoneNumberBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // contactPhoneNumberLabel
            // 
            this.contactPhoneNumberLabel.AutoSize = true;
            this.contactPhoneNumberLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.contactPhoneNumberLabel.Location = new System.Drawing.Point(42, 373);
            this.contactPhoneNumberLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.contactPhoneNumberLabel.Name = "contactPhoneNumberLabel";
            this.contactPhoneNumberLabel.Size = new System.Drawing.Size(83, 13);
            this.contactPhoneNumberLabel.TabIndex = 22;
            this.contactPhoneNumberLabel.Text = "Phone number";
            // 
            // contactLastNameBox
            // 
            // 
            // 
            // 
            this.contactLastNameBox.CustomButton.Image = null;
            this.contactLastNameBox.CustomButton.Location = new System.Drawing.Point(148, 2);
            this.contactLastNameBox.CustomButton.Margin = new System.Windows.Forms.Padding(2);
            this.contactLastNameBox.CustomButton.Name = "";
            this.contactLastNameBox.CustomButton.Size = new System.Drawing.Size(13, 13);
            this.contactLastNameBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.contactLastNameBox.CustomButton.TabIndex = 1;
            this.contactLastNameBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.contactLastNameBox.CustomButton.UseSelectable = true;
            this.contactLastNameBox.CustomButton.Visible = false;
            this.contactLastNameBox.Lines = new string[0];
            this.contactLastNameBox.Location = new System.Drawing.Point(148, 346);
            this.contactLastNameBox.Margin = new System.Windows.Forms.Padding(2);
            this.contactLastNameBox.MaxLength = 32767;
            this.contactLastNameBox.Name = "contactLastNameBox";
            this.contactLastNameBox.PasswordChar = '\0';
            this.contactLastNameBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.contactLastNameBox.SelectedText = "";
            this.contactLastNameBox.SelectionLength = 0;
            this.contactLastNameBox.SelectionStart = 0;
            this.contactLastNameBox.ShortcutsEnabled = true;
            this.contactLastNameBox.Size = new System.Drawing.Size(164, 18);
            this.contactLastNameBox.TabIndex = 21;
            this.contactLastNameBox.UseSelectable = true;
            this.contactLastNameBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.contactLastNameBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // contactLastNameLabel
            // 
            this.contactLastNameLabel.AutoSize = true;
            this.contactLastNameLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.contactLastNameLabel.Location = new System.Drawing.Point(42, 346);
            this.contactLastNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.contactLastNameLabel.Name = "contactLastNameLabel";
            this.contactLastNameLabel.Size = new System.Drawing.Size(58, 13);
            this.contactLastNameLabel.TabIndex = 20;
            this.contactLastNameLabel.Text = "Last name";
            // 
            // contactFirstNameBox
            // 
            // 
            // 
            // 
            this.contactFirstNameBox.CustomButton.Image = null;
            this.contactFirstNameBox.CustomButton.Location = new System.Drawing.Point(148, 2);
            this.contactFirstNameBox.CustomButton.Margin = new System.Windows.Forms.Padding(2);
            this.contactFirstNameBox.CustomButton.Name = "";
            this.contactFirstNameBox.CustomButton.Size = new System.Drawing.Size(13, 13);
            this.contactFirstNameBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.contactFirstNameBox.CustomButton.TabIndex = 1;
            this.contactFirstNameBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.contactFirstNameBox.CustomButton.UseSelectable = true;
            this.contactFirstNameBox.CustomButton.Visible = false;
            this.contactFirstNameBox.Lines = new string[0];
            this.contactFirstNameBox.Location = new System.Drawing.Point(148, 320);
            this.contactFirstNameBox.Margin = new System.Windows.Forms.Padding(2);
            this.contactFirstNameBox.MaxLength = 32767;
            this.contactFirstNameBox.Name = "contactFirstNameBox";
            this.contactFirstNameBox.PasswordChar = '\0';
            this.contactFirstNameBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.contactFirstNameBox.SelectedText = "";
            this.contactFirstNameBox.SelectionLength = 0;
            this.contactFirstNameBox.SelectionStart = 0;
            this.contactFirstNameBox.ShortcutsEnabled = true;
            this.contactFirstNameBox.Size = new System.Drawing.Size(164, 18);
            this.contactFirstNameBox.TabIndex = 19;
            this.contactFirstNameBox.UseSelectable = true;
            this.contactFirstNameBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.contactFirstNameBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // contactFirstNameLabel
            // 
            this.contactFirstNameLabel.AutoSize = true;
            this.contactFirstNameLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.contactFirstNameLabel.Location = new System.Drawing.Point(42, 320);
            this.contactFirstNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.contactFirstNameLabel.Name = "contactFirstNameLabel";
            this.contactFirstNameLabel.Size = new System.Drawing.Size(60, 13);
            this.contactFirstNameLabel.TabIndex = 18;
            this.contactFirstNameLabel.Text = "First name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(42, 152);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Last seen location";
            // 
            // locationBox
            // 
            // 
            // 
            // 
            this.locationBox.CustomButton.Image = null;
            this.locationBox.CustomButton.Location = new System.Drawing.Point(148, 2);
            this.locationBox.CustomButton.Margin = new System.Windows.Forms.Padding(2);
            this.locationBox.CustomButton.Name = "";
            this.locationBox.CustomButton.Size = new System.Drawing.Size(13, 13);
            this.locationBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.locationBox.CustomButton.TabIndex = 1;
            this.locationBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.locationBox.CustomButton.UseSelectable = true;
            this.locationBox.CustomButton.Visible = false;
            this.locationBox.Lines = new string[0];
            this.locationBox.Location = new System.Drawing.Point(148, 152);
            this.locationBox.Margin = new System.Windows.Forms.Padding(2);
            this.locationBox.MaxLength = 32767;
            this.locationBox.Name = "locationBox";
            this.locationBox.PasswordChar = '\0';
            this.locationBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.locationBox.SelectedText = "";
            this.locationBox.SelectionLength = 0;
            this.locationBox.SelectionStart = 0;
            this.locationBox.ShortcutsEnabled = true;
            this.locationBox.Size = new System.Drawing.Size(164, 18);
            this.locationBox.TabIndex = 15;
            this.locationBox.UseSelectable = true;
            this.locationBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.locationBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // lastSeenOn
            // 
            this.lastSeenOn.AutoSize = true;
            this.lastSeenOn.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lastSeenOn.Location = new System.Drawing.Point(42, 126);
            this.lastSeenOn.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lastSeenOn.Name = "lastSeenOn";
            this.lastSeenOn.Size = new System.Drawing.Size(71, 13);
            this.lastSeenOn.TabIndex = 14;
            this.lastSeenOn.Text = "Last seen on";
            // 
            // lastSeenOnPicker
            // 
            this.lastSeenOnPicker.Location = new System.Drawing.Point(148, 126);
            this.lastSeenOnPicker.Margin = new System.Windows.Forms.Padding(2);
            this.lastSeenOnPicker.Name = "lastSeenOnPicker";
            this.lastSeenOnPicker.Size = new System.Drawing.Size(165, 21);
            this.lastSeenOnPicker.TabIndex = 13;
            // 
            // uploadButton
            // 
            this.uploadButton.Location = new System.Drawing.Point(401, 246);
            this.uploadButton.Margin = new System.Windows.Forms.Padding(2);
            this.uploadButton.Name = "uploadButton";
            this.uploadButton.Size = new System.Drawing.Size(67, 21);
            this.uploadButton.TabIndex = 12;
            this.uploadButton.Text = "Upload";
            this.uploadButton.Click += new System.EventHandler(this.uploadButton_Click);
            // 
            // additionalInfoBox
            // 
            // 
            // 
            // 
            this.additionalInfoBox.CustomButton.Image = null;
            this.additionalInfoBox.CustomButton.Location = new System.Drawing.Point(76, 1);
            this.additionalInfoBox.CustomButton.Margin = new System.Windows.Forms.Padding(2);
            this.additionalInfoBox.CustomButton.Name = "";
            this.additionalInfoBox.CustomButton.Size = new System.Drawing.Size(87, 87);
            this.additionalInfoBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.additionalInfoBox.CustomButton.TabIndex = 1;
            this.additionalInfoBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.additionalInfoBox.CustomButton.UseSelectable = true;
            this.additionalInfoBox.CustomButton.Visible = false;
            this.additionalInfoBox.Lines = new string[0];
            this.additionalInfoBox.Location = new System.Drawing.Point(148, 178);
            this.additionalInfoBox.Margin = new System.Windows.Forms.Padding(2);
            this.additionalInfoBox.MaxLength = 32767;
            this.additionalInfoBox.Multiline = true;
            this.additionalInfoBox.Name = "additionalInfoBox";
            this.additionalInfoBox.PasswordChar = '\0';
            this.additionalInfoBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.additionalInfoBox.SelectedText = "";
            this.additionalInfoBox.SelectionLength = 0;
            this.additionalInfoBox.SelectionStart = 0;
            this.additionalInfoBox.ShortcutsEnabled = true;
            this.additionalInfoBox.Size = new System.Drawing.Size(164, 89);
            this.additionalInfoBox.TabIndex = 8;
            this.additionalInfoBox.UseSelectable = true;
            this.additionalInfoBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.additionalInfoBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // missingPersonPictureBox
            // 
            this.missingPersonPictureBox.BackColor = System.Drawing.Color.NavajoWhite;
            this.missingPersonPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.missingPersonPictureBox.Location = new System.Drawing.Point(356, 57);
            this.missingPersonPictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.missingPersonPictureBox.Name = "missingPersonPictureBox";
            this.missingPersonPictureBox.Size = new System.Drawing.Size(156, 170);
            this.missingPersonPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.missingPersonPictureBox.TabIndex = 11;
            this.missingPersonPictureBox.TabStop = false;
            // 
            // dateOfBirthPicker
            // 
            this.dateOfBirthPicker.Location = new System.Drawing.Point(148, 99);
            this.dateOfBirthPicker.Margin = new System.Windows.Forms.Padding(2);
            this.dateOfBirthPicker.Name = "dateOfBirthPicker";
            this.dateOfBirthPicker.Size = new System.Drawing.Size(165, 21);
            this.dateOfBirthPicker.TabIndex = 10;
            // 
            // addMissingPersonButton
            // 
            this.addMissingPersonButton.Location = new System.Drawing.Point(356, 366);
            this.addMissingPersonButton.Margin = new System.Windows.Forms.Padding(2);
            this.addMissingPersonButton.Name = "addMissingPersonButton";
            this.addMissingPersonButton.Size = new System.Drawing.Size(166, 53);
            this.addMissingPersonButton.TabIndex = 9;
            this.addMissingPersonButton.Text = "Add missing person";
            this.addMissingPersonButton.Click += new System.EventHandler(this.addMissingPersonButton_Click);
            // 
            // adInfoLabel
            // 
            this.adInfoLabel.AutoSize = true;
            this.adInfoLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.adInfoLabel.Location = new System.Drawing.Point(42, 178);
            this.adInfoLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.adInfoLabel.Name = "adInfoLabel";
            this.adInfoLabel.Size = new System.Drawing.Size(68, 26);
            this.adInfoLabel.TabIndex = 7;
            this.adInfoLabel.Text = "Additional \r\ninformation";
            // 
            // lastNameBox
            // 
            // 
            // 
            // 
            this.lastNameBox.CustomButton.Image = null;
            this.lastNameBox.CustomButton.Location = new System.Drawing.Point(148, 2);
            this.lastNameBox.CustomButton.Margin = new System.Windows.Forms.Padding(2);
            this.lastNameBox.CustomButton.Name = "";
            this.lastNameBox.CustomButton.Size = new System.Drawing.Size(13, 13);
            this.lastNameBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.lastNameBox.CustomButton.TabIndex = 1;
            this.lastNameBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.lastNameBox.CustomButton.UseSelectable = true;
            this.lastNameBox.CustomButton.Visible = false;
            this.lastNameBox.Lines = new string[0];
            this.lastNameBox.Location = new System.Drawing.Point(148, 73);
            this.lastNameBox.Margin = new System.Windows.Forms.Padding(2);
            this.lastNameBox.MaxLength = 32767;
            this.lastNameBox.Name = "lastNameBox";
            this.lastNameBox.PasswordChar = '\0';
            this.lastNameBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.lastNameBox.SelectedText = "";
            this.lastNameBox.SelectionLength = 0;
            this.lastNameBox.SelectionStart = 0;
            this.lastNameBox.ShortcutsEnabled = true;
            this.lastNameBox.Size = new System.Drawing.Size(164, 18);
            this.lastNameBox.TabIndex = 5;
            this.lastNameBox.UseSelectable = true;
            this.lastNameBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.lastNameBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // firstNameBox
            // 
            // 
            // 
            // 
            this.firstNameBox.CustomButton.Image = null;
            this.firstNameBox.CustomButton.Location = new System.Drawing.Point(148, 2);
            this.firstNameBox.CustomButton.Margin = new System.Windows.Forms.Padding(2);
            this.firstNameBox.CustomButton.Name = "";
            this.firstNameBox.CustomButton.Size = new System.Drawing.Size(13, 13);
            this.firstNameBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.firstNameBox.CustomButton.TabIndex = 1;
            this.firstNameBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.firstNameBox.CustomButton.UseSelectable = true;
            this.firstNameBox.CustomButton.Visible = false;
            this.firstNameBox.Lines = new string[0];
            this.firstNameBox.Location = new System.Drawing.Point(148, 46);
            this.firstNameBox.Margin = new System.Windows.Forms.Padding(2);
            this.firstNameBox.MaxLength = 32767;
            this.firstNameBox.Name = "firstNameBox";
            this.firstNameBox.PasswordChar = '\0';
            this.firstNameBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.firstNameBox.SelectedText = "";
            this.firstNameBox.SelectionLength = 0;
            this.firstNameBox.SelectionStart = 0;
            this.firstNameBox.ShortcutsEnabled = true;
            this.firstNameBox.Size = new System.Drawing.Size(164, 18);
            this.firstNameBox.TabIndex = 4;
            this.firstNameBox.UseSelectable = true;
            this.firstNameBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.firstNameBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // dateOfBirthLabel
            // 
            this.dateOfBirthLabel.AutoSize = true;
            this.dateOfBirthLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.dateOfBirthLabel.Location = new System.Drawing.Point(42, 99);
            this.dateOfBirthLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.dateOfBirthLabel.Name = "dateOfBirthLabel";
            this.dateOfBirthLabel.Size = new System.Drawing.Size(73, 13);
            this.dateOfBirthLabel.TabIndex = 3;
            this.dateOfBirthLabel.Text = "Date of birth";
            // 
            // LastNameLabel
            // 
            this.LastNameLabel.AutoSize = true;
            this.LastNameLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.LastNameLabel.Location = new System.Drawing.Point(42, 73);
            this.LastNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LastNameLabel.Name = "LastNameLabel";
            this.LastNameLabel.Size = new System.Drawing.Size(58, 13);
            this.LastNameLabel.TabIndex = 2;
            this.LastNameLabel.Text = "Last name";
            // 
            // firstNameLabel
            // 
            this.firstNameLabel.AutoSize = true;
            this.firstNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.firstNameLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.firstNameLabel.Location = new System.Drawing.Point(42, 46);
            this.firstNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.firstNameLabel.Name = "firstNameLabel";
            this.firstNameLabel.Size = new System.Drawing.Size(60, 13);
            this.firstNameLabel.TabIndex = 1;
            this.firstNameLabel.Text = "First name";
            // 
            // addPersonLabel
            // 
            this.addPersonLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.addPersonLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.addPersonLabel.Cursor = System.Windows.Forms.Cursors.Default;
            this.addPersonLabel.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.addPersonLabel.ForeColor = System.Drawing.Color.White;
            this.addPersonLabel.Location = new System.Drawing.Point(133, 9);
            this.addPersonLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.addPersonLabel.Name = "addPersonLabel";
            this.addPersonLabel.Size = new System.Drawing.Size(281, 19);
            this.addPersonLabel.TabIndex = 0;
            this.addPersonLabel.Text = "This is where you can add a missing person";
            this.addPersonLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // homeButton
            // 
            this.homeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.homeButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.homeButton.BackColor = System.Drawing.Color.NavajoWhite;
            this.homeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.homeButton.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.homeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.homeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.homeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.homeButton.Image = ((System.Drawing.Image)(resources.GetObject("homeButton.Image")));
            this.homeButton.Location = new System.Drawing.Point(146, 11);
            this.homeButton.Margin = new System.Windows.Forms.Padding(2);
            this.homeButton.Name = "homeButton";
            this.homeButton.Size = new System.Drawing.Size(155, 73);
            this.homeButton.TabIndex = 0;
            this.homeButton.Text = "Home";
            this.homeButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.homeButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.homeButton.UseVisualStyleBackColor = false;
            this.homeButton.Click += new System.EventHandler(this.homeButton_Click);
            // 
            // scanButton
            // 
            this.scanButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.scanButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.scanButton.BackColor = System.Drawing.Color.NavajoWhite;
            this.scanButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.scanButton.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.scanButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.scanButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.scanButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.scanButton.Image = ((System.Drawing.Image)(resources.GetObject("scanButton.Image")));
            this.scanButton.Location = new System.Drawing.Point(306, 11);
            this.scanButton.Margin = new System.Windows.Forms.Padding(2);
            this.scanButton.Name = "scanButton";
            this.scanButton.Size = new System.Drawing.Size(155, 73);
            this.scanButton.TabIndex = 1;
            this.scanButton.Text = "Scan";
            this.scanButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.scanButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.scanButton.UseVisualStyleBackColor = false;
            this.scanButton.Click += new System.EventHandler(this.scanButton_Click);
            // 
            // addPersonButton
            // 
            this.addPersonButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.addPersonButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.addPersonButton.BackColor = System.Drawing.Color.NavajoWhite;
            this.addPersonButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.addPersonButton.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.addPersonButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.addPersonButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.addPersonButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addPersonButton.Image = ((System.Drawing.Image)(resources.GetObject("addPersonButton.Image")));
            this.addPersonButton.Location = new System.Drawing.Point(466, 11);
            this.addPersonButton.Margin = new System.Windows.Forms.Padding(2);
            this.addPersonButton.Name = "addPersonButton";
            this.addPersonButton.Size = new System.Drawing.Size(155, 73);
            this.addPersonButton.TabIndex = 2;
            this.addPersonButton.Text = "Add a missing person";
            this.addPersonButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.addPersonButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.addPersonButton.UseVisualStyleBackColor = false;
            this.addPersonButton.Click += new System.EventHandler(this.addPersonButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.exitButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.exitButton.BackColor = System.Drawing.Color.NavajoWhite;
            this.exitButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.exitButton.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.exitButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.exitButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.exitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exitButton.Image = ((System.Drawing.Image)(resources.GetObject("exitButton.Image")));
            this.exitButton.Location = new System.Drawing.Point(626, 11);
            this.exitButton.Margin = new System.Windows.Forms.Padding(2);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(155, 73);
            this.exitButton.TabIndex = 3;
            this.exitButton.Text = "Exit";
            this.exitButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.exitButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.exitButton.UseVisualStyleBackColor = false;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.logoPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.logoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("logoPictureBox.Image")));
            this.logoPictureBox.Location = new System.Drawing.Point(19, 0);
            this.logoPictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size(122, 94);
            this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.logoPictureBox.TabIndex = 0;
            this.logoPictureBox.TabStop = false;
            // 
            // underHomePanel
            // 
            this.underHomePanel.BackColor = System.Drawing.Color.Transparent;
            this.underHomePanel.Location = new System.Drawing.Point(146, 86);
            this.underHomePanel.Margin = new System.Windows.Forms.Padding(2);
            this.underHomePanel.Name = "underHomePanel";
            this.underHomePanel.Size = new System.Drawing.Size(155, 8);
            this.underHomePanel.TabIndex = 2;
            // 
            // underScanPanel
            // 
            this.underScanPanel.BackColor = System.Drawing.Color.Transparent;
            this.underScanPanel.Location = new System.Drawing.Point(306, 86);
            this.underScanPanel.Margin = new System.Windows.Forms.Padding(2);
            this.underScanPanel.Name = "underScanPanel";
            this.underScanPanel.Size = new System.Drawing.Size(155, 8);
            this.underScanPanel.TabIndex = 3;
            // 
            // underPersonPanel
            // 
            this.underPersonPanel.BackColor = System.Drawing.Color.Transparent;
            this.underPersonPanel.Location = new System.Drawing.Point(466, 86);
            this.underPersonPanel.Margin = new System.Windows.Forms.Padding(2);
            this.underPersonPanel.Name = "underPersonPanel";
            this.underPersonPanel.Size = new System.Drawing.Size(155, 8);
            this.underPersonPanel.TabIndex = 3;
            // 
            // underExitPanel
            // 
            this.underExitPanel.BackColor = System.Drawing.Color.Transparent;
            this.underExitPanel.Location = new System.Drawing.Point(626, 86);
            this.underExitPanel.Margin = new System.Windows.Forms.Padding(2);
            this.underExitPanel.Name = "underExitPanel";
            this.underExitPanel.Size = new System.Drawing.Size(155, 8);
            this.underExitPanel.TabIndex = 4;
            // 
            // buttonsPanel
            // 
            this.buttonsPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonsPanel.BackColor = System.Drawing.Color.Transparent;
            this.buttonsPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonsPanel.Controls.Add(this.underExitPanel);
            this.buttonsPanel.Controls.Add(this.underPersonPanel);
            this.buttonsPanel.Controls.Add(this.underScanPanel);
            this.buttonsPanel.Controls.Add(this.underHomePanel);
            this.buttonsPanel.Controls.Add(this.logoPictureBox);
            this.buttonsPanel.Controls.Add(this.exitButton);
            this.buttonsPanel.Controls.Add(this.addPersonButton);
            this.buttonsPanel.Controls.Add(this.scanButton);
            this.buttonsPanel.Controls.Add(this.homeButton);
            this.buttonsPanel.Location = new System.Drawing.Point(-16, 2);
            this.buttonsPanel.Margin = new System.Windows.Forms.Padding(2);
            this.buttonsPanel.Name = "buttonsPanel";
            this.buttonsPanel.Size = new System.Drawing.Size(862, 103);
            this.buttonsPanel.TabIndex = 0;
            // 
            // missingPersonsTableAdapter
            // 
            this.missingPersonsTableAdapter.ClearBeforeFill = true;
            // 
            // FormFaceDetection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(800, 600);
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(868, 599);
            this.Controls.Add(this.homePanel);
            this.Controls.Add(this.addPersonPanel);
            this.Controls.Add(this.scanPanel);
            this.Controls.Add(this.buttonsPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormFaceDetection";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TransparencyKey = System.Drawing.Color.DeepSkyBlue;
            this.Load += new System.EventHandler(this.FormFaceDetection_Load);
            this.homePanel.ResumeLayout(false);
            this.homePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.missingPeopleDataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.missingPersonsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pstop2018DataSet2)).EndInit();
            this.scanPanel.ResumeLayout(false);
            this.scanPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scanPictureBox)).EndInit();
            this.addPersonPanel.ResumeLayout(false);
            this.addPersonPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.missingPersonPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.buttonsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.Panel homePanel;
        private System.Windows.Forms.Label homeLabel;
        private System.Windows.Forms.Panel scanPanel;
        public System.Windows.Forms.PictureBox scanPictureBox;
        private System.Windows.Forms.Panel addPersonPanel;
        private System.Windows.Forms.Label addPersonLabel;
        private System.Windows.Forms.Label firstNameLabel;
        private System.Windows.Forms.Label LastNameLabel;
        private System.Windows.Forms.Label adInfoLabel;
        private MetroFramework.Controls.MetroTextBox lastNameBox;
        private MetroFramework.Controls.MetroTextBox firstNameBox;
        private System.Windows.Forms.Label dateOfBirthLabel;
        private System.Windows.Forms.Button addMissingPersonButton;
        private MetroFramework.Controls.MetroTextBox additionalInfoBox;
        private System.Windows.Forms.DateTimePicker dateOfBirthPicker;
        public System.Windows.Forms.PictureBox missingPersonPictureBox;
        private System.Windows.Forms.Button uploadButton;
        private System.Windows.Forms.Label lastSeenOn;
        private System.Windows.Forms.DateTimePicker lastSeenOnPicker;
        private System.Windows.Forms.Label label1;
        private MetroFramework.Controls.MetroTextBox locationBox;
        private MetroFramework.Controls.MetroTextBox contactEmailAddressBox;
        private System.Windows.Forms.Label contactEmailAddressLabel;
        private MetroFramework.Controls.MetroTextBox contactPhoneNumberBox;
        private System.Windows.Forms.Label contactPhoneNumberLabel;
        private MetroFramework.Controls.MetroTextBox contactLastNameBox;
        private System.Windows.Forms.Label contactLastNameLabel;
        private MetroFramework.Controls.MetroTextBox contactFirstNameBox;
        private System.Windows.Forms.Label contactFirstNameLabel;
        private System.Windows.Forms.Label contactLabel;
        private System.Windows.Forms.Label currentlyMissingLabel;
        internal System.Windows.Forms.Button homeButton;
        private System.Windows.Forms.Button scanButton;
        private System.Windows.Forms.Button addPersonButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.Panel underHomePanel;
        private System.Windows.Forms.Panel underScanPanel;
        private System.Windows.Forms.Panel underPersonPanel;
        private System.Windows.Forms.Panel underExitPanel;
        private System.Windows.Forms.Panel buttonsPanel;
        private System.Windows.Forms.DataGridView missingPeopleDataGrid;
        private System.Windows.Forms.Label youCanHelpLabel;
        private System.Windows.Forms.CheckBox useWebcamPragueBox;
        private System.Windows.Forms.TextBox cameraUrlBox;
        private System.Windows.Forms.Button activateScanButton;
        private System.Windows.Forms.Label cameraUrlLabel;
        private pstop2018DataSet2 pstop2018DataSet2;
        private System.Windows.Forms.BindingSource missingPersonsBindingSource;
        private pstop2018DataSet2TableAdapters.MissingPersonsTableAdapter missingPersonsTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn faceTokenDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn firstNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastSeenDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastSeenLocationDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn additionalInformationDataGridViewTextBoxColumn;
    }
}