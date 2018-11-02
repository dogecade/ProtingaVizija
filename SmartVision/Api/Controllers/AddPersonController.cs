//using FaceAnalysis;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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
        public ActionResult CheckImage()
        {
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic = System.Web.HttpContext.Current.Request.Files["Image"];
                Image image = Image.FromStream(pic.InputStream);
                using (var ms = new MemoryStream())
                {
                    using (var bitmap = new Bitmap(image))
                    {
                        
                        bitmap.Save(ms, ImageFormat.Jpeg);
                        var SigBase64 = Convert.ToBase64String(ms.GetBuffer()); //Get Base64
                        return Json(new { result = SigBase64 }
                        , JsonRequestBehavior.AllowGet);
                    }
                }

            }
            return null;
        }
        /*private async FrameAnalysisJSON Process(Bitmap uploadedImage)
        {
            uploadedImage = FaceAnalysis.HelperMethods.ProcessImage(uploadedImage);
            FrameAnalysisJSON result = await FaceProcessor.ProcessFrame((Bitmap)uploadedImage.Clone());
            //HelperMethods.CropImage(uploadedImage, result.Faces[chosenImageIndex].Face_rectangle, 25);

            return result;
        }*/

    }
}