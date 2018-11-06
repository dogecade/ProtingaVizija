using AForge.Video;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace AdminPage.Controllers
{
    public class VideoController : ApiController
    {
        private static MJPEGStream mjpegStream;// = new MJPEGStream("http://192.168.0.102:8080/video");
        private static bool frameAvailable = false;
        private static Bitmap frame = null;
        private static string BOUNDARY = "frame";

        /// <summary>
        /// Initializer for the MJPEGstream
        /// </summary>
        VideoController()
        {
            //mjpegStream.Source = @"{{http://192.168.0.102:8080/video}}";
            //mjpegStream = new MJPEGStream("http://192.168.0.102:8080/video");
            //mjpegStream.NewFrame += new NewFrameEventHandler(ShowFrameEvent);
        }

        [System.Web.Http.HttpPost]
        public ActionResult SetStream()
        {
            mjpegStream = new MJPEGStream("http://192.168.0.102:8080/video");
            mjpegStream.NewFrame += new NewFrameEventHandler(ShowFrameEvent);
            
            return new JsonResult()
            {
                Data = 5,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public HttpResponseMessage GetVideoContent()
        {
            Debug.WriteLine(mjpegStream);
            mjpegStream?.Start();
            var response = Request.CreateResponse();
            response.Content = new PushStreamContent((Action<Stream, HttpContent, TransportContext>)StartStream);
            response.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/x-mixed-replace; boundary=" + BOUNDARY);
            return response;
        }

        /// <summary>
        /// Craete an appropriate header.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private byte[] CreateHeader(int length)
        {
            string header =
                "--" + BOUNDARY + "\r\n" +
                "Content-Type:image/jpeg\r\n" +
                "Content-Length:" + length + "\r\n\r\n";

            return Encoding.ASCII.GetBytes(header);
        }

        public byte[] CreateFooter()
        {
            return Encoding.ASCII.GetBytes("\r\n");
        }

        /// <summary>
        /// Write the given frame to the stream
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="frame">Bitmap format frame</param>
        private void WriteFrame(Stream stream, Bitmap frame)
        {
            // prepare image data
            byte[] imageData = null;

            // this is to make sure memory stream is disposed after using
            using (MemoryStream ms = new MemoryStream())
            {
                frame.Save(ms, ImageFormat.Jpeg);
                imageData = ms.ToArray();
            }

            // prepare header
            byte[] header = CreateHeader(imageData.Length);
            // prepare footer
            byte[] footer = CreateFooter();

            // Start writing data
            stream.Write(header, 0, header.Length);
            stream.Write(imageData, 0, imageData.Length);
            stream.Write(footer, 0, footer.Length);
        }

        /// <summary>
        /// While the MJPEGStream is running and clients are connected,
        /// continue sending frames.
        /// </summary>
        /// <param name="stream">Stream to write to.</param>
        /// <param name="httpContent">The content information</param>
        /// <param name="transportContext"></param>
        private void StartStream(Stream stream, HttpContent httpContent, TransportContext transportContext)
        {
            if(mjpegStream != null)
            {
                while (mjpegStream.IsRunning && HttpContext.Current.Response.IsClientConnected)
                {
                    if (frameAvailable)
                    {
                        try
                        {
                            Debug.WriteLine("NX");
                            WriteFrame(stream, frame);
                            frame.Dispose();
                            frameAvailable = false;
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e);
                        }
                    }
                }
                StopStream();
            }
            
        }

        /// <summary>
        /// This event is thrown when a new frame is detected by the MJPEGStream
        /// </summary>
        /// <param name="sender">Object that is sending the event</param>
        /// <param name="eventArgs">Data from the event, including the frame</param>
        public static void ShowFrameEvent(object sender, NewFrameEventArgs eventArgs)
        {
            Debug.WriteLine("BBD");
            frame = new Bitmap(eventArgs.Frame);
            frameAvailable = true;
        }

        /// <summary>
        /// Stop the stream.
        /// </summary>
        private void StopStream()
        {
            Debug.WriteLine("Stop stream");
            mjpegStream?.Stop();
        }
    }
}
