using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsForms.FormControl;
using Emgu.CV;
using Emgu.CV.Structure;
using FaceAnalysis;
using System.Threading;
using System.Drawing;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace WindowsForms
{
    public class WebcamInput
    {
        private static List<Rectangle> faceRectangles = new List<Rectangle>();
        private static CancellationTokenSource tokenSource = new CancellationTokenSource();
        private static Task taskConsumer;
        private static Image<Bgr, Byte> lastImage;
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
                    throw new SystemException("Input camera was not found");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                MessageBox.Show("Input camera was not found!");
                return false;
            }
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.Dispose();
                tokenSource = new CancellationTokenSource();
            }
            processor = new FaceProcessor(capture);
            taskConsumer = Task.Run(() => ProcessFrameAsync());
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Gets frame from input and adds to buffer.
        /// Author: Arnas Danaitis
        /// </summary>
        private static async void GetFrameAsync(object sender, EventArgs e)
        {
            var imageFrame = await processor.GetCaptureFrame();
            if (imageFrame == null)
                return;
            FormFaceDetection.Current.scanPictureBox.Image = imageFrame.Bitmap;
            lock (faceRectangles)
                foreach (Rectangle face in faceRectangles)
                    imageFrame.Draw(face, new Bgr(Color.Red), 1);
            if (lastImage != null)
                lastImage.Dispose();
            lastImage = imageFrame;
        }

        /// <summary>
        /// Processes frames from the buffer.
        /// Author: Arnas Danaitis
        /// </summary>
        private static async void ProcessFrameAsync()
        {
            while (await processor.HasFrames() && !tokenSource.IsCancellationRequested)
                faceRectangles = await processor.ProcessFrame();
            lock (faceRectangles)
                faceRectangles.Clear();
        }
    }
}
