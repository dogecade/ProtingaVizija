using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Constants;
using FaceAnalysis;
using Helpers;
using Objects.CameraProperties;

namespace Api.Controllers
{
    public class CameraStreamController : Controller
    {
        // GET: FilmAPerson
        public ActionResult CameraStreamView()
        {
            return View();
        }

        [HttpPost]
        public async Task CaptureSnapshot(string imgBase64)
        {
            // Prepare base64 string
            imgBase64 = imgBase64.Substring(imgBase64.IndexOf("base64,", StringComparison.Ordinal) + 7);
            imgBase64 = imgBase64.Substring(0, imgBase64.LastIndexOf("\"", StringComparison.Ordinal));
            imgBase64 = imgBase64.Replace(" ", "+");

            // Create a bitmap
            byte[] bitmapData = Convert.FromBase64String(FixBase64ForImage(imgBase64));
            System.IO.MemoryStream streamBitmap = new System.IO.MemoryStream(bitmapData);
            Bitmap bitImage = new Bitmap((Bitmap)Image.FromStream(streamBitmap));

            bitImage = HelperMethods.ProcessImage(bitImage);
            // Analyze bitmap
            var processedFrame = await FaceProcessor.ProcessFrame(bitImage);

            foreach (var face in processedFrame.Faces)
            {
                await FaceSearch(face.Face_token);
            }
        }
        public string FixBase64ForImage(string Image)
        {
            System.Text.StringBuilder sbText = new System.Text.StringBuilder(Image, Image.Length);
            sbText.Replace("\r\n", String.Empty); sbText.Replace(" ", String.Empty);
            return sbText.ToString();
        }

        /// <summary>
        /// Task for face search - executes API call, etc.
        /// </summary>
        private async Task FaceSearch(string faceToken)
        {
            FaceApiCalls faceApiCalls = new FaceApiCalls(new HttpClientWrapper());
            SearchResultHandler searchResultHandler = new SearchResultHandler(new CameraProperties("http://quadcam.unrfound.unr.edu/axis-cgi/mjpg/video.cgi", 8023));
            FoundFacesJSON response = await faceApiCalls.SearchFaceInFaceset(Keys.facesetToken, faceToken);
            if (response != null)
                foreach (LikelinessResult result in response.LikelinessConfidences())
                    await searchResultHandler.HandleSearchResult(result);
        }
    }
}