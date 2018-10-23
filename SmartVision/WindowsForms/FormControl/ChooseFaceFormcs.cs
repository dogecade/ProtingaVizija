using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsForms.FormControl
{
    public partial class ChooseFaceFormcs : Form
    {
        public int SelectedFace { get; set; }

        public ChooseFaceFormcs(IList<FaceAnalysis.Face> faces, Bitmap uploadedImage)
        {
            InitializeComponent();
            InitializeImageView(faces, uploadedImage);

        }
        public void InitializeImageView(IList<FaceAnalysis.Face> faces, Bitmap uploadedImage)
        {
            string textNearFace;

            for (int i = 0; i < faces.Count(); i++)
            {
                Bitmap tempImage = new Bitmap(uploadedImage);
                imageList.Images.Add(FaceAnalysis.HelperMethods.CropImage(tempImage, faces[i].Face_rectangle, 25));

                imageListView.SmallImageList = imageList;
                textNearFace = "      Face Nr. " + (i + 1).ToString() + "            ";
                imageListView.Items.Add(textNearFace, i);
                imageListView.Items[i].ForeColor = Color.White;
                imageListView.Items[i].Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
            }

            if (faces.Count == 2)
                imageListView.Columns[0].Width += SystemInformation.VerticalScrollBarWidth;
        }

        private void imageListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SelectedFace = imageListView.SelectedItems[0].ImageIndex;
            this.DialogResult = DialogResult.OK;
        }
    }
}
