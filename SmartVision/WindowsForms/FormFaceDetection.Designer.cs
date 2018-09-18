namespace WindowsForms
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
            this.buttonsPanel = new System.Windows.Forms.Panel();
            this.logoPanel = new System.Windows.Forms.Panel();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.exitButton = new System.Windows.Forms.Button();
            this.addPersonButton = new System.Windows.Forms.Button();
            this.scanButton = new System.Windows.Forms.Button();
            this.homeButton = new System.Windows.Forms.Button();
            this.homePanel = new System.Windows.Forms.Panel();
            this.homeText = new System.Windows.Forms.TextBox();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.scanPanel = new System.Windows.Forms.Panel();
            this.scanPictureBox = new System.Windows.Forms.PictureBox();
            this.addPersonPanel = new System.Windows.Forms.Panel();
            this.dateOfBirthPicker = new System.Windows.Forms.DateTimePicker();
            this.confirmPersonButton = new System.Windows.Forms.Button();
            this.adInfoBox = new System.Windows.Forms.TextBox();
            this.adInfoLabel = new System.Windows.Forms.Label();
            this.lastNameBox = new System.Windows.Forms.TextBox();
            this.firstNameBox = new System.Windows.Forms.TextBox();
            this.dateOfBirthLabel = new System.Windows.Forms.Label();
            this.LastNameLabel = new System.Windows.Forms.Label();
            this.firstNameLabel = new System.Windows.Forms.Label();
            this.addPersonText = new System.Windows.Forms.TextBox();
            this.personPictureBox = new System.Windows.Forms.PictureBox();
            this.uploadButton = new System.Windows.Forms.Button();
            this.buttonsPanel.SuspendLayout();
            this.logoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.homePanel.SuspendLayout();
            this.scanPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scanPictureBox)).BeginInit();
            this.addPersonPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.personPictureBox)).BeginInit();
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
            this.homePanel.Size = new System.Drawing.Size(602, 318);
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
            // bottomPanel
            // 
            this.bottomPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bottomPanel.Location = new System.Drawing.Point(206, 324);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(602, 202);
            this.bottomPanel.TabIndex = 2;
            // 
            // scanPanel
            // 
            this.scanPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scanPanel.Controls.Add(this.scanPictureBox);
            this.scanPanel.Location = new System.Drawing.Point(206, 0);
            this.scanPanel.Name = "scanPanel";
            this.scanPanel.Size = new System.Drawing.Size(602, 318);
            this.scanPanel.TabIndex = 2;
            // 
            // scanPictureBox
            // 
            this.scanPictureBox.Location = new System.Drawing.Point(1, 1);
            this.scanPictureBox.Name = "scanPictureBox";
            this.scanPictureBox.Size = new System.Drawing.Size(596, 312);
            this.scanPictureBox.TabIndex = 0;
            this.scanPictureBox.TabStop = false;
            // 
            // addPersonPanel
            // 
            this.addPersonPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.addPersonPanel.Controls.Add(this.uploadButton);
            this.addPersonPanel.Controls.Add(this.personPictureBox);
            this.addPersonPanel.Controls.Add(this.dateOfBirthPicker);
            this.addPersonPanel.Controls.Add(this.confirmPersonButton);
            this.addPersonPanel.Controls.Add(this.adInfoBox);
            this.addPersonPanel.Controls.Add(this.adInfoLabel);
            this.addPersonPanel.Controls.Add(this.lastNameBox);
            this.addPersonPanel.Controls.Add(this.firstNameBox);
            this.addPersonPanel.Controls.Add(this.dateOfBirthLabel);
            this.addPersonPanel.Controls.Add(this.LastNameLabel);
            this.addPersonPanel.Controls.Add(this.firstNameLabel);
            this.addPersonPanel.Controls.Add(this.addPersonText);
            this.addPersonPanel.Location = new System.Drawing.Point(206, 0);
            this.addPersonPanel.Name = "addPersonPanel";
            this.addPersonPanel.Size = new System.Drawing.Size(602, 318);
            this.addPersonPanel.TabIndex = 2;
            // 
            // dateOfBirthPicker
            // 
            this.dateOfBirthPicker.Location = new System.Drawing.Point(120, 134);
            this.dateOfBirthPicker.Name = "dateOfBirthPicker";
            this.dateOfBirthPicker.Size = new System.Drawing.Size(205, 23);
            this.dateOfBirthPicker.TabIndex = 10;
            // 
            // confirmPersonButton
            // 
            this.confirmPersonButton.Location = new System.Drawing.Point(442, 286);
            this.confirmPersonButton.Name = "confirmPersonButton";
            this.confirmPersonButton.Size = new System.Drawing.Size(75, 27);
            this.confirmPersonButton.TabIndex = 9;
            this.confirmPersonButton.Text = "Confirm";
            this.confirmPersonButton.UseVisualStyleBackColor = true;
            this.confirmPersonButton.Click += new System.EventHandler(this.confirmPersonButton_Click);
            // 
            // adInfoBox
            // 
            this.adInfoBox.Location = new System.Drawing.Point(120, 168);
            this.adInfoBox.Multiline = true;
            this.adInfoBox.Name = "adInfoBox";
            this.adInfoBox.Size = new System.Drawing.Size(205, 145);
            this.adInfoBox.TabIndex = 8;
            // 
            // adInfoLabel
            // 
            this.adInfoLabel.AutoSize = true;
            this.adInfoLabel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.adInfoLabel.Location = new System.Drawing.Point(17, 162);
            this.adInfoLabel.Name = "adInfoLabel";
            this.adInfoLabel.Size = new System.Drawing.Size(87, 38);
            this.adInfoLabel.TabIndex = 7;
            this.adInfoLabel.Text = "Additional \r\ninformation";
            // 
            // lastNameBox
            // 
            this.lastNameBox.Location = new System.Drawing.Point(120, 101);
            this.lastNameBox.Name = "lastNameBox";
            this.lastNameBox.Size = new System.Drawing.Size(205, 23);
            this.lastNameBox.TabIndex = 5;
            // 
            // firstNameBox
            // 
            this.firstNameBox.Location = new System.Drawing.Point(120, 68);
            this.firstNameBox.Name = "firstNameBox";
            this.firstNameBox.Size = new System.Drawing.Size(205, 23);
            this.firstNameBox.TabIndex = 4;
            // 
            // dateOfBirthLabel
            // 
            this.dateOfBirthLabel.AutoSize = true;
            this.dateOfBirthLabel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.dateOfBirthLabel.Location = new System.Drawing.Point(17, 134);
            this.dateOfBirthLabel.Name = "dateOfBirthLabel";
            this.dateOfBirthLabel.Size = new System.Drawing.Size(95, 19);
            this.dateOfBirthLabel.TabIndex = 3;
            this.dateOfBirthLabel.Text = "Date of birth";
            // 
            // LastNameLabel
            // 
            this.LastNameLabel.AutoSize = true;
            this.LastNameLabel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.LastNameLabel.Location = new System.Drawing.Point(17, 101);
            this.LastNameLabel.Name = "LastNameLabel";
            this.LastNameLabel.Size = new System.Drawing.Size(80, 19);
            this.LastNameLabel.TabIndex = 2;
            this.LastNameLabel.Text = "Last name";
            // 
            // firstNameLabel
            // 
            this.firstNameLabel.AutoSize = true;
            this.firstNameLabel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.firstNameLabel.Location = new System.Drawing.Point(17, 68);
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
            // personPictureBox
            // 
            this.personPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.personPictureBox.Location = new System.Drawing.Point(380, 68);
            this.personPictureBox.Name = "personPictureBox";
            this.personPictureBox.Size = new System.Drawing.Size(194, 212);
            this.personPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.personPictureBox.TabIndex = 11;
            this.personPictureBox.TabStop = false;
            // 
            // uploadButton
            // 
            this.uploadButton.Location = new System.Drawing.Point(442, 159);
            this.uploadButton.Name = "uploadButton";
            this.uploadButton.Size = new System.Drawing.Size(75, 26);
            this.uploadButton.TabIndex = 12;
            this.uploadButton.Text = "Upload";
            this.uploadButton.UseVisualStyleBackColor = true;
            this.uploadButton.Click += new System.EventHandler(this.uploadButton_Click);
            // 
            // FaceDetection
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(75)))));
            this.ClientSize = new System.Drawing.Size(820, 538);
            this.Controls.Add(this.addPersonPanel);
            this.Controls.Add(this.homePanel);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.buttonsPanel);
            this.Controls.Add(this.scanPanel);
            this.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.buttonsPanel.ResumeLayout(false);
            this.logoPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.homePanel.ResumeLayout(false);
            this.homePanel.PerformLayout();
            this.scanPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scanPictureBox)).EndInit();
            this.addPersonPanel.ResumeLayout(false);
            this.addPersonPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.personPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel buttonsPanel;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button addPersonButton;
        private System.Windows.Forms.Button scanButton;
        private System.Windows.Forms.Button homeButton;
        private System.Windows.Forms.Panel homePanel;
        private System.Windows.Forms.TextBox homeText;
        private System.Windows.Forms.Panel bottomPanel;
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
        private System.Windows.Forms.Button confirmPersonButton;
        private System.Windows.Forms.TextBox adInfoBox;
        private System.Windows.Forms.DateTimePicker dateOfBirthPicker;
        private System.Windows.Forms.PictureBox personPictureBox;
        private System.Windows.Forms.Button uploadButton;
    }
}

