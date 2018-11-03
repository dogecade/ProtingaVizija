//using FaceAnalysis;
using FaceAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Api.Controllers
{
    public class AddPersonController : Controller
    {
        // GET: AddPerson
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MissingPersonView()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CheckImage()
        {
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic = System.Web.HttpContext.Current.Request.Files["Image"];
                Image image = Image.FromStream(pic.InputStream);

                using (var uploadedImage = HelperMethods.ProcessImage(new Bitmap(image)))
                {
                    FrameAnalysisJSON result = await FaceProcessor.ProcessFrame(new Bitmap(uploadedImage)); Debug.WriteLine(result.Faces.Count);
                    if (result == null || result.Faces.Count == 0)
                        return Json(new { result = new { facesCount = 0 } }
                    , JsonRequestBehavior.AllowGet);

                    List<string> listOfFaces = new List<string>();
                    List<string> faceTokens = new List<string>();
                    foreach (var face in result.Faces)
                    {
                        using (var ms = new MemoryStream())
                        {
                            HelperMethods.CropImage(new Bitmap(uploadedImage), face.Face_rectangle, 25).Save(ms, ImageFormat.Jpeg);
                            listOfFaces.Add(Convert.ToBase64String(ms.GetBuffer()));
                        }
                        faceTokens.Add(face.Face_token);

                    }
                    return Json(new { result = new { images = listOfFaces, faceToken = faceTokens, facesCount = result.Faces.Count } }
                    , JsonRequestBehavior.AllowGet);
                }

            }
            return null;
        }

        public ActionResult FacesModalView()
        {

            return PartialView();
        }
    }
}