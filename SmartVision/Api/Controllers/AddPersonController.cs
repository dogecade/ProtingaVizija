//using FaceAnalysis;
using FaceAnalysis;
using System;
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
                using (var ms = new MemoryStream())
                {
                    using (var bitmap = new Bitmap(image))
                    {
                        var uploadedImage = HelperMethods.ProcessImage(bitmap);
                        FrameAnalysisJSON result = await FaceProcessor.ProcessFrame((Bitmap)uploadedImage.Clone());
                        if (result == null || result.Faces.Count == 0)
                            return  Json(new { result = new {facesCount = 0 } }
                        , JsonRequestBehavior.AllowGet);
                        var faceImage = HelperMethods.CropImage(uploadedImage, result.Faces[0].Face_rectangle, 25);
                        faceImage.Save(ms, ImageFormat.Jpeg);
                        var SigBase64 = Convert.ToBase64String(ms.GetBuffer()); //Get Base64
                        return Json(new { result = new { image = SigBase64, faceToken = result.Faces[0].Face_token, facesCount = result.Faces.Count } }
                        , JsonRequestBehavior.AllowGet);
                    }
                }

            }
            return null;
        }
    }
}