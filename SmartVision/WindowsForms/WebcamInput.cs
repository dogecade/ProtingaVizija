using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsForms.FormControl;
using Emgu.CV;
using Emgu.CV.Structure;
using FaceAnalysis;
using System.Threading;
using System.Drawing;
using System.Collections.Generic;

namespace WindowsForms
{
    public class WebcamInput
    {
        private static List<Rectangle> faceRectangles = new List<Rectangle>();
        private static CancellationTokenSource tokenSource = new CancellationTokenSource();
        private static Task taskAnalysis;
        private static Bitmap lastImage;
        private static VideoCapture capture;
        private static FaceProcessor processor;

        /// <summary>
        /// Enables the input of the webcam
        /// </summary>
        public static bool EnableWebcam(string cameraUrl = null)
        {
            try
            {
                if (cameraUrl == null)
                    capture = new VideoCapture();
                else
                    capture = new VideoCapture(cameraUrl);
                if (!capture.IsOpened)
                    throw new SystemException(Messages.cameraNotFound);
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
            taskAnalysis = Task.Run(() => ProcessFrameAsync());
            Application.Idle += GetFrameAsync;
            return true;
        }

        /// <summary>
        /// Disables the input of the webcam
        /// </summary>
        public static void DisableWebcam()
        {
            try
            {
                capture.Dispose();
                Application.Idle -= GetFrameAsync;
                tokenSource.Cancel();
                processor.Complete();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Gets frame from processor, draws face rectangles on it.
        /// Author: Arnas Danaitis
        /// </summary>
        private static async void GetFrameAsync(object sender, EventArgs e)
        {
            //the forms app doesn't do multiple inputs, so just use the first bitmap.
            Bitmap image = (await processor.GetCaptureFrames())[0];
            if (image == null)
                return;
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
        private static async void ProcessFrameAsync()
        {
            while (await processor.HasFrames() && !tokenSource.IsCancellationRequested)
            {
                faceRectangles = await processor.GetRectanglesFromFrame() ?? faceRectangles;
            }
                
            lock (faceRectangles)
                faceRectangles.Clear();
        }
    }
}
