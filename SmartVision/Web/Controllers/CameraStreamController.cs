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

        public ActionResult SnapshotModalView()
        {
            return PartialView();
        }

        [HttpPost]
        //this should probably return action result, otherwise nothing will happen in the frontend IIRC
        public async Task<ActionResult> CaptureSnapshot(string imgBase64, string latitude, string longitude)
        {
            FaceApiCalls apiCalls = new FaceApiCalls(new HttpClientWrapper());

            // Prepare base64 string
            imgBase64 = imgBase64.Substring(imgBase64.IndexOf("base64,", StringComparison.Ordinal) + 7);
            imgBase64 = imgBase64.Substring(0, imgBase64.LastIndexOf("\"", StringComparison.Ordinal));
            imgBase64 = imgBase64.Replace(" ", "+");

            // Create a bitmap
            byte[] bitmapData = Convert.FromBase64String(FixBase64ForImage(imgBase64));
            System.IO.MemoryStream streamBitmap = new System.IO.MemoryStream(bitmapData);
            Bitmap bitmap = new Bitmap((Bitmap)Image.FromStream(streamBitmap));

            bitmap = HelperMethods.ProcessImage(bitmap);
            // Analyze bitmap
            FrameAnalysisJSON analysisResult = await FaceProcessor.ProcessFrame(bitmap);
            if (analysisResult == null)
            {
                //error must've occured, should alert user.
                return null;
            }

            if (analysisResult.Faces.Count == 0)
            {
                return Json(new { result = "No faces have been found in the provided picture" }, JsonRequestBehavior.AllowGet);
            }

            var biggestConfidence = LikelinessConfidence.LowProbability;

            foreach (var face in analysisResult.Faces)
            {
                var searchResult = await apiCalls.SearchFaceInFaceset(face.Face_token);
                if (searchResult != null)
                {
                    foreach (var likelinessResult in searchResult.LikelinessConfidences()) //might want to set the camera properties to some value.
                    {
                        biggestConfidence = (likelinessResult.Confidence > biggestConfidence)
                            ? likelinessResult.Confidence
                            : biggestConfidence;

                        await SearchResultHandler.HandleOneResult(result: likelinessResult, 
                                                                  minimumConfidence: LikelinessConfidence.VeryHighProbability,
                                                                  cameraProperties: new CameraProperties(Convert.ToDouble(latitude), Convert.ToDouble(longitude)));
                    }
                }

            }

            return
                Json(new
                { result =
                    string.Format("There's a {0} probability that the person/people from your picture are currently missing. Thank you for the submission, appropriate actions will be taken.",
                    biggestConfidence.ToPrettyString())
                }, JsonRequestBehavior.AllowGet);
        }
        public string FixBase64ForImage(string Image)
        {
            System.Text.StringBuilder sbText = new System.Text.StringBuilder(Image, Image.Length);
            sbText.Replace("\r\n", string.Empty); sbText.Replace(" ", string.Empty);
            return sbText.ToString();
        }
    }
}