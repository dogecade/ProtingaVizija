namespace WindowsForms.FormControl
{
    partial class ChooseFaceFormcs
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
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChooseFaceFormcs));
            this.imageListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // imageListView
            // 
            this.imageListView.Alignment = System.Windows.Forms.ListViewAlignment.Default;
            this.imageListView.BackColor = System.Drawing.Color.Blue;
            this.imageListView.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("imageListView.BackgroundImage")));
            this.imageListView.BackgroundImageTiled = true;
            this.imageListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.imageListView.Cursor = System.Windows.Forms.Cursors.Default;
            this.imageListView.GridLines = true;
            this.imageListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.imageListView.Location = new System.Drawing.Point(0, -2);
            this.imageListView.Name = "imageListView";
            this.imageListView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.imageListView.RightToLeftLayout = true;
            this.imageListView.Size = new System.Drawing.Size(299, 260);
            this.imageListView.SmallImageList = this.imageList;
            this.imageListView.TabIndex = 0;
            this.imageListView.TileSize = new System.Drawing.Size(5, 5);
            this.imageListView.UseCompatibleStateImageBehavior = false;
            this.imageListView.View = System.Windows.Forms.View.Details;
            this.imageListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.imageListView_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Choose the face of the missing perosn";
            this.columnHeader1.Width = 201;
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList.ImageSize = new System.Drawing.Size(100, 100);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ChooseFaceFormcs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(295, 257);
            this.Controls.Add(this.imageListView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ChooseFaceFormcs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose a face:";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView imageListView;
        private System.Windows.Forms.ImageList imageList;
        public System.Windows.Forms.ColumnHeader columnHeader1;
    }
}