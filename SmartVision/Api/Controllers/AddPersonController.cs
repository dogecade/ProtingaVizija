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

                    List<CroppedFace> croppedFaces = new List<CroppedFace>();
                    foreach (var face in result.CroppedFaces(uploadedImage, 25))
                    {
                        croppedFaces.Add(face);
                    }
                    return Json(new { result = croppedFaces }
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