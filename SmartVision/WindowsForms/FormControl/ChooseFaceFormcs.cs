using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            int i = 0;
            string textNearFace;
            imageListView.Columns.Add("Choose the face of the missing person", 150);
            imageListView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);


            foreach (var face in faces)
            {
                Bitmap tempImage = new Bitmap(uploadedImage);
                imageList.Images.Add(FaceAnalysis.HelperMethods.CropImage(tempImage, face.Face_rectangle, 25));

                imageListView.SmallImageList = imageList;
                textNearFace = "Face Nr. " + (i + 1).ToString();
                imageListView.Items.Add(textNearFace, i);
                i++;
            }
        }

        private void imageListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SelectedFace = imageListView.SelectedItems[0].ImageIndex;
            this.DialogResult = DialogResult.OK;
        }
    }
}
