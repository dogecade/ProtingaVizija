using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsForms.FormControl
{
    class ImageUpload
    {
        private const string fileFilter = "jpg files(*.jpg)|*.jpg| PNG files(*.png)|*.png| All Files(*.*)|*.*";

        public static Bitmap UploadImage()
        {
            string path = GetImagePath();
            return path != null && path != "" ? new Bitmap(Image.FromFile(path)): null;
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



