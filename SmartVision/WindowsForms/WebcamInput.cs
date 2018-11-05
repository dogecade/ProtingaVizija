using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsForms.FormControl;
using FaceAnalysis;
using System.Threading;
using System.Drawing;
using System.Collections.Generic;
using AForge.Video;
using AForge.Video.DirectShow;

namespace WindowsForms
{
    public class WebcamInput
    {
        private static IList<Rectangle> faceRectangles = new List<Rectangle>();
        private static CancellationTokenSource tokenSource = new CancellationTokenSource();
        private static Bitmap lastImage;
        private static IVideoSource capture;
        private static FaceProcessor processor;

        /// <summary>
        /// Enables the input of the webcam
        /// </summary>
        public static bool EnableWebcam(string cameraUrl = null)
        {
            try
            {
                if (cameraUrl == null)
                    capture = new VideoCaptureDevice(
                        new FilterInfoCollection(FilterCategory.VideoInputDevice)[0].MonikerString);
                
                //else
                //    capture = new VideoCapture(cameraUrl);
                capture.Start();
                //try catch needed for proper things
                //if (!capture.)
                //    throw new SystemException(Messages.cameraNotFound);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                MessageBox.Show(Messages.cameraNotFound);
                return false;
            }
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Dispose();
                tokenSource = new CancellationTokenSource();
            }
            processor = new FaceProcessor(capture);
            capture.NewFrame += GetFrame;
            processor.FrameProcessed += GetRectangles;
            return true;
        }

        /// <summary>
        /// Disables the input of the webcam
        /// </summary>
        public static void DisableWebcam()
        {
            try
            {
                capture.Stop();
                capture.NewFrame -= GetFrame;
                processor.FrameProcessed -= GetRectangles;
                tokenSource.Cancel();
                processor.Complete();
                lock (faceRectangles)
                    faceRectangles.Clear();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Gets frame from source, draws face rectangles on it.
        /// Author: Arnas Danaitis
        /// </summary>
        private static void GetFrame(object sender, NewFrameEventArgs e)
        {
            Bitmap image;
            lock (e)
                image = new Bitmap(e.Frame);
            FormFaceDetection.Current.scanPictureBox.Image = image;
            
            lock (faceRectangles)
                using (Graphics g = Graphics.FromImage(image))
                using (Pen pen = new Pen(new SolidBrush(Color.Red), 1))
                    foreach (Rectangle face in faceRectangles)
                        g.DrawRectangle(pen, face);
            lastImage?.Dispose();
            lastImage = image;
        }

        /// <summary>
        /// Gets list of faces from processor.
        /// Author: Arnas Danaitis
        /// </summary>
        private static void GetRectangles(object sender, FrameProcessedEventArgs e)
        {
            lock (faceRectangles)
                faceRectangles = e.FaceRectangles ?? faceRectangles;
        }
    }
}
