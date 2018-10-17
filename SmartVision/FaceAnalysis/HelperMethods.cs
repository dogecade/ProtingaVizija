using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FaceAnalysis
{
    public class HelperMethods
    {
        /// <summary>
        /// Converts bitmap to byte array
        /// </summary>
        /// <param name="img">Image in bitmap form</param>
        /// <returns>Image in byte[]</returns>
        public static byte[] ImageToByte(Bitmap img)
        {
            byte[] array = null;
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    img.Save(stream, ImageFormat.Jpeg);
                    img.Dispose();
                    array = stream.ToArray();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
            return array;
        }

        /// <summary>
        /// Makes an API call for image analysis.
        /// Returns just the number of faces (for cases where this is all that is needed)
        /// </summary>
        /// <param name="img">Image in bitmap form</param>
        /// <returns>Number of faces in image, -1 if error occured</returns>
        /// 
        public static async Task<int> NumberOfFaces(Bitmap img)
        {
            FaceApiCalls faceApiCalls = new FaceApiCalls(new HttpClientWrapper());
            int faceCount;
            try
            {
                faceCount = JsonConvert.DeserializeObject<FrameAnalysisJSON>(await faceApiCalls.AnalyzeFrame(ImageToByte(img))).faces.Count;
            }
            catch (ArgumentNullException)
            {
                Debug.WriteLine("Invalid API response");
                return -1;
            }
            return faceCount;
        }

        /// <summary>
        /// Processes a bitmap so that it is (more likely to be) accepted by API.
        /// Previous version of image is disposed.
        /// Might need refinement in the future (higher res might be better than better quality in some cases)
        /// Works for now
        /// </summary>
        /// <param name="img">Image in bitmap form</param>
        /// <returns>Processed image in bitmap form</returns>
        public static Bitmap ProcessImage(Bitmap img)
        {
            const int MAX_SIZE = 1000;
            var ratio = Math.Min((double)MAX_SIZE / img.Width, (double)MAX_SIZE / img.Height);
            var newWidth = (int)(img.Width * ratio);
            var newHeight = (int)(img.Height * ratio);

            Bitmap imgClone = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);
            using (Graphics gr = Graphics.FromImage(imgClone))
            {
                gr.DrawImage(img, 0, 0, imgClone.Width, imgClone.Height);
            }
            img.Dispose();
            return imgClone;
        }
    }
}