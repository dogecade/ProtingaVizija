using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsForms.FaceAnalysis;
using WindowsForms.FormControl;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Threading.Tasks.Dataflow;
using System.Drawing;
using System.Drawing.Imaging;

namespace WindowsForms
{
    public class WebcamInput
    {
        private const int BUFFER_LIMIT = 10000;
        private static readonly BufferBlock<byte[]> buffer = new BufferBlock<byte[]>(new DataflowBlockOptions {BoundedCapacity = BUFFER_LIMIT});
        private static VideoCapture capture; // Takes video from camera as image frames
        private static int frameCount = 0;
        private static Task taskConsumer;
        /// <summary>
        /// Enables the input of the webcam
        /// Author: Deividas Brazenas
        /// </summary>
        public static bool EnableWebcam()
        {
            try
            {
                capture = new VideoCapture();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            try
            {
                if (!capture.IsOpened)
                {
                    throw new SystemException("Input camera was not found");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                MessageBox.Show("Input camera was not found!");
                return false;
            }
            taskConsumer = Task.Run(() => ProcessFrameAsync());
            Application.Idle += GetFrameAsync;

            return true;
        }

        /// <summary>
        /// Disables the input of the webcam
        /// Author: Deividas Brazenas
        /// </summary>
        public static async void DisableWebcam()
        {
            try
            {
                capture.Dispose();
                Application.Idle -= GetFrameAsync;
                buffer.Complete();
                await taskConsumer;
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
            using (var imageFrame = capture.QueryFrame().ToImage<Bgr, Byte>())
            {
                var form = FormFaceDetection.Current;
                form.scanPictureBox.Image = imageFrame.Bitmap;

                frameCount++;

                // put up for analysis every 15th frame
                if (frameCount == 15)
                {
                    frameCount = 0;
                    await buffer.SendAsync(ImageToByte(imageFrame.Bitmap));
                    Debug.WriteLine("Adding frame to queue");
                }
            }
        }

        /// <summary>
        /// Processes frames from the buffer.
        /// Author: Arnas Danaitis
        /// </summary>
        private static async void ProcessFrameAsync()
        {
            FaceRecognition faceRecognition = new FaceRecognition();
            while (await buffer.OutputAvailableAsync())
            {
                byte[] frameToProcess = await buffer.ReceiveAsync();
                if (buffer.Count >= BUFFER_LIMIT * 0.9)
                {
                    Debug.WriteLine("Buffer size within 10% of limit, skipping processing.");
                    Debug.WriteLine(buffer.Count.ToString() + ">=" + (BUFFER_LIMIT * 0.9).ToString());
                }
                else
                {
                    Debug.WriteLine("Starting processing of frame. Remaining frames: " + buffer.Count.ToString());
                    var result = FaceRecognition.AnalyzeImage(frameToProcess);
                    Debug.WriteLine(DateTime.Now + " " + result);
                }
            }
        }

        /// <summary>
        /// Converts bitmap to byte array
        /// Author: Arnas Danaitis
        /// </summary>
        /// <param name="img">Image in bitmap form</param>
        /// <returns>Image in byte[]</returns>
        public static byte[] ImageToByte(Bitmap img)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    img.Save(stream, ImageFormat.Bmp);
                    img.Dispose();
                    return stream.ToArray();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }
    }
}
