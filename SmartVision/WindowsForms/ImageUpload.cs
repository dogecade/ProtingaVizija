using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsForms
{
    class ImageUpload
    {
        private const string fileFilter = "jpg files(*.jpg)|*.jpg| PNG files(*.png)|*.png| All Files(*.*)|*.*";

        public static void UploadImage()
        {
            var form = Form.ActiveForm as FormFaceDetection;
            form.missingPersonPictureBox.ImageLocation = GetImagePath();
            form.missingPersonPictureBox.BringToFront();
        }

        private static string GetImagePath()
        {
            String imageLocation = "";

            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = fileFilter;

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    imageLocation = dialog.FileName;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return null;
            }

            return imageLocation;
        }
    }
}



