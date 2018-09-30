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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFaceDetection));
            this.buttonsPanel = new System.Windows.Forms.Panel();
            this.logoPanel = new System.Windows.Forms.Panel();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.exitButton = new System.Windows.Forms.Button();
            this.addPersonButton = new System.Windows.Forms.Button();
            this.scanButton = new System.Windows.Forms.Button();
            this.homeButton = new System.Windows.Forms.Button();
            this.homePanel = new System.Windows.Forms.Panel();
            this.homeText = new System.Windows.Forms.TextBox();
            this.scanPanel = new System.Windows.Forms.Panel();
            this.scanPictureBox = new System.Windows.Forms.PictureBox();
            this.addPersonPanel = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.contactEmailAddressBox = new System.Windows.Forms.TextBox();
            this.contactEmailAddressLabel = new System.Windows.Forms.Label();
            this.contactPhoneNumberBox = new System.Windows.Forms.TextBox();
            this.contactPhoneNumberLabel = new System.Windows.Forms.Label();
            this.contactLastNameBox = new System.Windows.Forms.TextBox();
            this.contactLastNameLabel = new System.Windows.Forms.Label();
            this.contactFirstNameBox = new System.Windows.Forms.TextBox();
            this.contactFirstNameLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.locationBox = new System.Windows.Forms.TextBox();
            this.lastSeenOn = new System.Windows.Forms.Label();
            this.lastSeenOnPicker = new System.Windows.Forms.DateTimePicker();
            this.uploadButton = new System.Windows.Forms.Button();
            this.additionalInfoBox = new System.Windows.Forms.TextBox();
            this.missingPersonPictureBox = new System.Windows.Forms.PictureBox();
            this.dateOfBirthPicker = new System.Windows.Forms.DateTimePicker();
            this.addMissingPersonButton = new System.Windows.Forms.Button();
            this.adInfoLabel = new System.Windows.Forms.Label();
            this.lastNameBox = new System.Windows.Forms.TextBox();
            this.firstNameBox = new System.Windows.Forms.TextBox();
            this.dateOfBirthLabel = new System.Windows.Forms.Label();
            this.LastNameLabel = new System.Windows.Forms.Label();
            this.firstNameLabel = new System.Windows.Forms.Label();
            this.addPersonText = new System.Windows.Forms.TextBox();
            this.buttonsPanel.SuspendLayout();
            this.logoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.homePanel.SuspendLayout();
            this.scanPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scanPictureBox)).BeginInit();
            this.addPersonPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.missingPersonPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonsPanel
            // 
            this.buttonsPanel.Controls.Add(this.logoPanel);
            this.buttonsPanel.Controls.Add(this.exitButton);
            this.buttonsPanel.Controls.Add(this.addPersonButton);
            this.buttonsPanel.Controls.Add(this.scanButton);
            this.buttonsPanel.Controls.Add(this.homeButton);
            this.buttonsPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonsPanel.Location = new System.Drawing.Point(0, 0);
            this.buttonsPanel.Name = "buttonsPanel";
            this.buttonsPanel.Size = new System.Drawing.Size(200, 538);
            this.buttonsPanel.TabIndex = 0;
            // 
            // logoPanel
            // 
            this.logoPanel.Controls.Add(this.logoPictureBox);
            this.logoPanel.Location = new System.Drawing.Point(0, 0);
            this.logoPanel.Name = "logoPanel";
            this.logoPanel.Size = new System.Drawing.Size(200, 124);
            this.logoPanel.TabIndex = 4;
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Image = global::WindowsForms.Properties.Resources.dogologo;
            this.logoPictureBox.Location = new System.Drawing.Point(6, 3);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size(191, 118);
            this.logoPictureBox.TabIndex = 0;
            this.logoPictureBox.TabStop = false;
            // 
            // exitButton
            // 
            this.exitButton.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.exitButton.Location = new System.Drawing.Point(3, 421);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(194, 91);
            this.exitButton.TabIndex = 3;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // addPersonButton
            // 
            this.addPersonButton.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.addPersonButton.Location = new System.Drawing.Point(3, 324);
            this.addPersonButton.Name = "addPersonButton";
            this.addPersonButton.Size = new System.Drawing.Size(194, 91);
            this.addPersonButton.TabIndex = 2;
            this.addPersonButton.Text = "Add a missing person";
            this.addPersonButton.UseVisualStyleBackColor = true;
            this.addPersonButton.Click += new System.EventHandler(this.addPersonButton_Click);
            // 
            // scanButton
            // 
            this.scanButton.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.scanButton.Location = new System.Drawing.Point(3, 227);
            this.scanButton.Name = "scanButton";
            this.scanButton.Size = new System.Drawing.Size(194, 91);
            this.scanButton.TabIndex = 1;
            this.scanButton.Text = "Scan";
            this.scanButton.UseVisualStyleBackColor = true;
            this.scanButton.Click += new System.EventHandler(this.scanButton_Click);
            // 
            // homeButton
            // 
            this.homeButton.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.homeButton.Location = new System.Drawing.Point(3, 130);
            this.homeButton.Name = "homeButton";
            this.homeButton.Size = new System.Drawing.Size(194, 91);
            this.homeButton.TabIndex = 0;
            this.homeButton.Text = "Home";
            this.homeButton.UseVisualStyleBackColor = true;
            this.homeButton.Click += new System.EventHandler(this.homeButton_Click);
            // 
            // homePanel
            // 
            this.homePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.homePanel.Controls.Add(this.homeText);
            this.homePanel.Location = new System.Drawing.Point(206, 0);
            this.homePanel.Name = "homePanel";
            this.homePanel.Size = new System.Drawing.Size(602, 538);
            this.homePanel.TabIndex = 1;
            // 
            // homeText
            // 
            this.homeText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.homeText.ForeColor = System.Drawing.Color.White;
            this.homeText.Location = new System.Drawing.Point(120, 11);
            this.homeText.Name = "homeText";
            this.homeText.Size = new System.Drawing.Size(351, 23);
            this.homeText.TabIndex = 0;
            this.homeText.Text = "Main page";
            this.homeText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // scanPanel
            // 
            this.scanPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scanPanel.Controls.Add(this.scanPictureBox);
            this.scanPanel.Location = new System.Drawing.Point(206, 0);
            this.scanPanel.Name = "scanPanel";
            this.scanPanel.Size = new System.Drawing.Size(602, 538);
            this.scanPanel.TabIndex = 2;
            // 
            // scanPictureBox
            // 
            this.scanPictureBox.Location = new System.Drawing.Point(1, 1);
            this.scanPictureBox.Name = "scanPictureBox";
            this.scanPictureBox.Size = new System.Drawing.Size(602, 538);
            this.scanPictureBox.TabIndex = 0;
            this.scanPictureBox.TabStop = false;
            // 
            // addPersonPanel
            // 
            this.addPersonPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.addPersonPanel.Controls.Add(this.textBox1);
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
            this.addPersonPanel.Controls.Add(this.addPersonText);
            this.addPersonPanel.Location = new System.Drawing.Point(206, 0);
            this.addPersonPanel.Name = "addPersonPanel";
            this.addPersonPanel.Size = new System.Drawing.Size(602, 538);
            this.addPersonPanel.TabIndex = 2;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBox1.ForeColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(120, 360);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(351, 23);
            this.textBox1.TabIndex = 26;
            this.textBox1.Text = "Your contact information";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // contactEmailAddressBox
            // 
            this.contactEmailAddressBox.Location = new System.Drawing.Point(120, 496);
            this.contactEmailAddressBox.Name = "contactEmailAddressBox";
            this.contactEmailAddressBox.Size = new System.Drawing.Size(205, 23);
            this.contactEmailAddressBox.TabIndex = 25;
            // 
            // contactEmailAddressLabel
            // 
            this.contactEmailAddressLabel.AutoSize = true;
            this.contactEmailAddressLabel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.contactEmailAddressLabel.Location = new System.Drawing.Point(13, 499);
            this.contactEmailAddressLabel.Name = "contactEmailAddressLabel";
            this.contactEmailAddressLabel.Size = new System.Drawing.Size(103, 19);
            this.contactEmailAddressLabel.TabIndex = 24;
            this.contactEmailAddressLabel.Text = "Email address";
            // 
            // contactPhoneNumberBox
            // 
            this.contactPhoneNumberBox.Location = new System.Drawing.Point(120, 463);
            this.contactPhoneNumberBox.Name = "contactPhoneNumberBox";
            this.contactPhoneNumberBox.Size = new System.Drawing.Size(205, 23);
            this.contactPhoneNumberBox.TabIndex = 23;
            // 
            // contactPhoneNumberLabel
            // 
            this.contactPhoneNumberLabel.AutoSize = true;
            this.contactPhoneNumberLabel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.contactPhoneNumberLabel.Location = new System.Drawing.Point(13, 466);
            this.contactPhoneNumberLabel.Name = "contactPhoneNumberLabel";
            this.contactPhoneNumberLabel.Size = new System.Drawing.Size(111, 19);
            this.contactPhoneNumberLabel.TabIndex = 22;
            this.contactPhoneNumberLabel.Text = "Phone number";
            // 
            // contactLastNameBox
            // 
            this.contactLastNameBox.Location = new System.Drawing.Point(120, 430);
            this.contactLastNameBox.Name = "contactLastNameBox";
            this.contactLastNameBox.Size = new System.Drawing.Size(205, 23);
            this.contactLastNameBox.TabIndex = 21;
            // 
            // contactLastNameLabel
            // 
            this.contactLastNameLabel.AutoSize = true;
            this.contactLastNameLabel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.contactLastNameLabel.Location = new System.Drawing.Point(13, 433);
            this.contactLastNameLabel.Name = "contactLastNameLabel";
            this.contactLastNameLabel.Size = new System.Drawing.Size(80, 19);
            this.contactLastNameLabel.TabIndex = 20;
            this.contactLastNameLabel.Text = "Last name";
            // 
            // contactFirstNameBox
            // 
            this.contactFirstNameBox.Location = new System.Drawing.Point(120, 397);
            this.contactFirstNameBox.Name = "contactFirstNameBox";
            this.contactFirstNameBox.Size = new System.Drawing.Size(205, 23);
            this.contactFirstNameBox.TabIndex = 19;
            // 
            // contactFirstNameLabel
            // 
            this.contactFirstNameLabel.AutoSize = true;
            this.contactFirstNameLabel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.contactFirstNameLabel.Location = new System.Drawing.Point(13, 400);
            this.contactFirstNameLabel.Name = "contactFirstNameLabel";
            this.contactFirstNameLabel.Size = new System.Drawing.Size(78, 19);
            this.contactFirstNameLabel.TabIndex = 18;
            this.contactFirstNameLabel.Text = "First name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(13, 190);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 19);
            this.label1.TabIndex = 16;
            this.label1.Text = "Last seen location";
            // 
            // locationBox
            // 
            this.locationBox.Location = new System.Drawing.Point(120, 187);
            this.locationBox.Name = "locationBox";
            this.locationBox.Size = new System.Drawing.Size(205, 23);
            this.locationBox.TabIndex = 15;
            // 
            // lastSeenOn
            // 
            this.lastSeenOn.AutoSize = true;
            this.lastSeenOn.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lastSeenOn.Location = new System.Drawing.Point(13, 159);
            this.lastSeenOn.Name = "lastSeenOn";
            this.lastSeenOn.Size = new System.Drawing.Size(93, 19);
            this.lastSeenOn.TabIndex = 14;
            this.lastSeenOn.Text = "Last seen on";
            // 
            // lastSeenOnPicker
            // 
            this.lastSeenOnPicker.Location = new System.Drawing.Point(120, 154);
            this.lastSeenOnPicker.Name = "lastSeenOnPicker";
            this.lastSeenOnPicker.Size = new System.Drawing.Size(205, 23);
            this.lastSeenOnPicker.TabIndex = 13;
            // 
            // uploadButton
            // 
            this.uploadButton.Location = new System.Drawing.Point(436, 305);
            this.uploadButton.Name = "uploadButton";
            this.uploadButton.Size = new System.Drawing.Size(75, 26);
            this.uploadButton.TabIndex = 12;
            this.uploadButton.Text = "Upload";
            this.uploadButton.UseVisualStyleBackColor = true;
            this.uploadButton.Click += new System.EventHandler(this.uploadButton_Click);
            // 
            // additionalInfoBox
            // 
            this.additionalInfoBox.Location = new System.Drawing.Point(120, 220);
            this.additionalInfoBox.Multiline = true;
            this.additionalInfoBox.Name = "additionalInfoBox";
            this.additionalInfoBox.Size = new System.Drawing.Size(205, 111);
            this.additionalInfoBox.TabIndex = 8;
            // 
            // missingPersonPictureBox
            // 
            this.missingPersonPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.missingPersonPictureBox.Location = new System.Drawing.Point(380, 68);
            this.missingPersonPictureBox.Name = "missingPersonPictureBox";
            this.missingPersonPictureBox.Size = new System.Drawing.Size(194, 212);
            this.missingPersonPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.missingPersonPictureBox.TabIndex = 11;
            this.missingPersonPictureBox.TabStop = false;
            // 
            // dateOfBirthPicker
            // 
            this.dateOfBirthPicker.Location = new System.Drawing.Point(120, 121);
            this.dateOfBirthPicker.Name = "dateOfBirthPicker";
            this.dateOfBirthPicker.Size = new System.Drawing.Size(205, 23);
            this.dateOfBirthPicker.TabIndex = 10;
            // 
            // addMissingPersonButton
            // 
            this.addMissingPersonButton.Location = new System.Drawing.Point(380, 454);
            this.addMissingPersonButton.Name = "addMissingPersonButton";
            this.addMissingPersonButton.Size = new System.Drawing.Size(207, 66);
            this.addMissingPersonButton.TabIndex = 9;
            this.addMissingPersonButton.Text = "Add missing person";
            this.addMissingPersonButton.UseVisualStyleBackColor = true;
            this.addMissingPersonButton.Click += new System.EventHandler(this.addMissingPersonButton_Click);
            // 
            // adInfoLabel
            // 
            this.adInfoLabel.AutoSize = true;
            this.adInfoLabel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.adInfoLabel.Location = new System.Drawing.Point(13, 250);
            this.adInfoLabel.Name = "adInfoLabel";
            this.adInfoLabel.Size = new System.Drawing.Size(87, 38);
            this.adInfoLabel.TabIndex = 7;
            this.adInfoLabel.Text = "Additional \r\ninformation";
            // 
            // lastNameBox
            // 
            this.lastNameBox.Location = new System.Drawing.Point(120, 88);
            this.lastNameBox.Name = "lastNameBox";
            this.lastNameBox.Size = new System.Drawing.Size(205, 23);
            this.lastNameBox.TabIndex = 5;
            // 
            // firstNameBox
            // 
            this.firstNameBox.Location = new System.Drawing.Point(120, 55);
            this.firstNameBox.Name = "firstNameBox";
            this.firstNameBox.Size = new System.Drawing.Size(205, 23);
            this.firstNameBox.TabIndex = 4;
            // 
            // dateOfBirthLabel
            // 
            this.dateOfBirthLabel.AutoSize = true;
            this.dateOfBirthLabel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.dateOfBirthLabel.Location = new System.Drawing.Point(13, 126);
            this.dateOfBirthLabel.Name = "dateOfBirthLabel";
            this.dateOfBirthLabel.Size = new System.Drawing.Size(95, 19);
            this.dateOfBirthLabel.TabIndex = 3;
            this.dateOfBirthLabel.Text = "Date of birth";
            // 
            // LastNameLabel
            // 
            this.LastNameLabel.AutoSize = true;
            this.LastNameLabel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.LastNameLabel.Location = new System.Drawing.Point(13, 91);
            this.LastNameLabel.Name = "LastNameLabel";
            this.LastNameLabel.Size = new System.Drawing.Size(80, 19);
            this.LastNameLabel.TabIndex = 2;
            this.LastNameLabel.Text = "Last name";
            // 
            // firstNameLabel
            // 
            this.firstNameLabel.AutoSize = true;
            this.firstNameLabel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.firstNameLabel.Location = new System.Drawing.Point(13, 58);
            this.firstNameLabel.Name = "firstNameLabel";
            this.firstNameLabel.Size = new System.Drawing.Size(78, 19);
            this.firstNameLabel.TabIndex = 1;
            this.firstNameLabel.Text = "First name";
            // 
            // addPersonText
            // 
            this.addPersonText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.addPersonText.ForeColor = System.Drawing.Color.White;
            this.addPersonText.Location = new System.Drawing.Point(120, 11);
            this.addPersonText.Name = "addPersonText";
            this.addPersonText.Size = new System.Drawing.Size(351, 23);
            this.addPersonText.TabIndex = 0;
            this.addPersonText.Text = "This is where you can add a missing person";
            this.addPersonText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FormFaceDetection
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(75)))));
            this.ClientSize = new System.Drawing.Size(820, 538);
            this.Controls.Add(this.addPersonPanel);
            this.Controls.Add(this.homePanel);
            this.Controls.Add(this.buttonsPanel);
            this.Controls.Add(this.scanPanel);
            this.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormFaceDetection";
            this.Load += new System.EventHandler(this.FormFaceDetection_Load);
            this.buttonsPanel.ResumeLayout(false);
            this.logoPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.homePanel.ResumeLayout(false);
            this.homePanel.PerformLayout();
            this.scanPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scanPictureBox)).EndInit();
            this.addPersonPanel.ResumeLayout(false);
            this.addPersonPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.missingPersonPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel buttonsPanel;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button addPersonButton;
        private System.Windows.Forms.Button scanButton;
        public System.Windows.Forms.Panel homePanel;
        private System.Windows.Forms.TextBox homeText;
        private System.Windows.Forms.Panel scanPanel;
        public System.Windows.Forms.PictureBox scanPictureBox;
        private System.Windows.Forms.Panel logoPanel;
        private System.Windows.Forms.Panel addPersonPanel;
        private System.Windows.Forms.TextBox addPersonText;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.Label firstNameLabel;
        private System.Windows.Forms.Label LastNameLabel;
        private System.Windows.Forms.Label adInfoLabel;
        private System.Windows.Forms.TextBox lastNameBox;
        private System.Windows.Forms.TextBox firstNameBox;
        private System.Windows.Forms.Label dateOfBirthLabel;
        private System.Windows.Forms.Button addMissingPersonButton;
        private System.Windows.Forms.TextBox additionalInfoBox;
        private System.Windows.Forms.DateTimePicker dateOfBirthPicker;
        public System.Windows.Forms.PictureBox missingPersonPictureBox;
        private System.Windows.Forms.Button uploadButton;
        private System.Windows.Forms.Label lastSeenOn;
        private System.Windows.Forms.DateTimePicker lastSeenOnPicker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox locationBox;
        private System.Windows.Forms.TextBox contactEmailAddressBox;
        private System.Windows.Forms.Label contactEmailAddressLabel;
        private System.Windows.Forms.TextBox contactPhoneNumberBox;
        private System.Windows.Forms.Label contactPhoneNumberLabel;
        private System.Windows.Forms.TextBox contactLastNameBox;
        private System.Windows.Forms.Label contactLastNameLabel;
        private System.Windows.Forms.TextBox contactFirstNameBox;
        private System.Windows.Forms.Label contactFirstNameLabel;
        private System.Windows.Forms.TextBox textBox1;
        internal System.Windows.Forms.Button homeButton;
    }
}

