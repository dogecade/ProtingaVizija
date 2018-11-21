//using FaceAnalysis;
using FaceAnalysis;
using Objects.Person;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Net.Http;
using Helpers;

namespace Api.Controllers
{
    public class AddPersonController : Controller
    {
        // GET: AddPerson
        public ActionResult Index()
        {
            return View("AddContactPerson");
        }
        public ActionResult MissingPersonView()
        {
            return View("AddMissingPerson");
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
                    FrameAnalysisJSON result = await FaceProcessor.ProcessFrame(new Bitmap(uploadedImage));
                    if (result == null || result.Faces.Count == 0)
                        return Json(new { result = new List<CroppedFace>() }
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
        [HttpPost]
        public async Task<string> AddMissingPerson (MissingPerson missingPersons)
        {
            HttpClientWrapper httpClient = new HttpClientWrapper();
            byte[] byteArray = Convert.FromBase64String(missingPersons.faceImg);
            using (var ms = new MemoryStream(byteArray, 0, byteArray.Length))
            {
                Bitmap image = (Bitmap)Bitmap.FromStream(ms);
                string imgLoc = await httpClient.PostImageToApiString(image);
                missingPersons.faceImg = imgLoc;
                HttpContent content = await httpClient.PostMissingPersonToApiAsync(missingPersons);
                string missing1 = await content.ReadAsStringAsync();
                return missing1;
            }
        }
        [HttpPost]
        public async Task<string> AddContactPerson (ContactPerson contactPersons)
        {
            HttpClientWrapper httpClient = new HttpClientWrapper();
            HttpContent content = await httpClient.PostContactPersonToApiAsync(contactPersons);
            string contact1 = await content.ReadAsStringAsync();
            return contact1;
        }
        [HttpPost]
        public async Task<string> AddRelationship(MissingContact missing)
        {
            HttpClientWrapper httpClient = new HttpClientWrapper();
            HttpContent content = await httpClient.PostRelToApi(missing);
            string contact1 = await content.ReadAsStringAsync();
            return contact1;
        }
        public ActionResult FacesModalView()
        {

            return PartialView();
        }
    }
}