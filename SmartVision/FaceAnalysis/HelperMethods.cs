using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace FaceAnalysis
{
    public static class HelperMethods
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
        /// Crops an image to a given rectangle + given percentage
        /// </summary>
        /// <param name="img">Image in bitmap form</param>
        /// <returns>Cropped image in bitmap form</returns>
        public static Bitmap CropImage(Bitmap img, Rectangle cropRectangle, int percentage)
        {
            cropRectangle.Inflate(cropRectangle.Width * percentage / 100, cropRectangle.Height * percentage / 100);
            Bitmap imgClone = new Bitmap(cropRectangle.Width, cropRectangle.Height, PixelFormat.Format24bppRgb);
            Debug.WriteLine(img.Width + " " + img.Height);
            Debug.WriteLine(cropRectangle);
            using (Graphics gr = Graphics.FromImage(imgClone))
            {
                gr.DrawImage(img, new Rectangle(0, 0, imgClone.Width, imgClone.Height),
                             cropRectangle, GraphicsUnit.Pixel);
            }
            img.Dispose();
            return imgClone;
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
                gr.DrawImage(img, 0, 0, imgClone.Width, imgClone.Height);
            img.Dispose();
            return imgClone;
        }

        /// <summary>
        /// Combines multiple frames into a single one,
        /// preserving identifying information
        /// </summary>
        /// <typeparam name="IdentifierType">Type that identifies each bitmap / rectangle</typeparam>
        /// <param name="dictionary">Dictionary of identifier / bitmap pairs</param>
        /// <returns></returns>
        //TODO: should probably attempt to add padding to help prevent "phantom" face rectangles.
        public static (IDictionary<IdentifierType, Rectangle> rectangles, Bitmap image) ProcessImages<IdentifierType>(IDictionary<IdentifierType, Bitmap> dictionary)
        { 
            const int MAX_EDGE_IMAGES = 4;
            foreach (var key in dictionary.Keys)
                dictionary[key] = ProcessImage(dictionary[key]);
            Point currentPosition = new Point(0, 0);
            var idsRectangles = new Dictionary<IdentifierType, Rectangle>();
            var rows = dictionary
               .Where(pair => pair.Value != null)
               .Select((pair, i) => new { Index = i, Value = pair})
               .GroupBy(pair => pair.Index / MAX_EDGE_IMAGES)
               .Select(pairList => pairList.Select(element => element.Value));
            Bitmap conjoinedBitmap = new Bitmap(width: rows.Max(row => row.Sum(pair => pair.Value.Width)),
                                                height: rows.Sum(row => row.Max(pair => pair.Value.Height)));
            using (Graphics g = Graphics.FromImage(conjoinedBitmap))
                foreach (var pairRow in rows)
                {
                    int maxHeight = 0;
                    foreach (var pair in pairRow) 
                    {
                        g.DrawImage(pair.Value, currentPosition);
                        idsRectangles[pair.Key] = new Rectangle(currentPosition, new Size(pair.Value.Width, pair.Value.Height));
                        currentPosition.X += pair.Value.Width;
                        maxHeight = maxHeight > pair.Value.Height ? maxHeight : pair.Value.Height;
                        pair.Value.Dispose();
                    }
                    currentPosition.X = 0;
                    currentPosition.Y += maxHeight;
                }      

            return (idsRectangles, conjoinedBitmap);          
        }
    }
}