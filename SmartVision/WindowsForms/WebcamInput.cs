using System;
using System.Windows.Forms;
using WindowsForms.FormControl;
using FaceAnalysis;
using System.Drawing;
using AForge.Video;
using AForge.Video.DirectShow;
using Objects.CameraProperties;

namespace WindowsForms
{
    public class WebcamInput
    {
        private static Bitmap lastImage;
        private static ProcessableVideoSource source;
        private static FaceProcessor processor;
        private static int BusId = 744;
        /// <summary>
        /// Enables the input of the webcam
        /// </summary>
        public static bool EnableWebcam(string cameraUrl = null)
        {
            IVideoSource capture;
            try
            {
                if (cameraUrl == null)
                    capture = new VideoCaptureDevice(
                        new FilterInfoCollection(FilterCategory.VideoInputDevice)[0].MonikerString);
                else //assumes that it's a MJPEG stream, it's forms so whatever
                    capture = new MJPEGStream(cameraUrl);
                source = new ProcessableVideoSource(capture);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                MessageBox.Show(Messages.cameraNotFound);
                return false;
            }
            processor = new FaceProcessor(source, new CameraProperties(cameraUrl, BusId));
            source.NewFrame += GetFrame;
            source.Start();
            return true;
        }

        /// <summary>
        /// Disables the input of the webcam
        /// </summary>
        public static void DisableWebcam()
        {
            try
            {
                source.Stop();
                processor.Complete();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Gets frame from source
        /// Author: Arnas Danaitis
        /// </summary>
        private static void GetFrame(object sender, NewFrameEventArgs e)
        {
            Bitmap image = new Bitmap(e.Frame);
            FormFaceDetection.Current.scanPictureBox.Image = image;

            lastImage?.Dispose();
            lastImage = image;
        }
    }
}
