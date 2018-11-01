using Api.Models;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;

namespace Api.Controllers
{
    public class CurrentlyMissingController : Controller
    {
        // GET: CurrentlyMissing
        public ActionResult Index()
        {
            var missingPeople = new MissingPersonsController().GetMissingPersons().ToList();

            var sourceString = missingPeople[0].faceImg.Replace("[", "");
            sourceString = sourceString.Replace("]", "");
            sourceString = sourceString.Replace("\"", "");
            sourceString = sourceString.Replace(@"\\\\", @"\");
            sourceString = sourceString.Remove(0, 1);
            sourceString = sourceString.Remove(sourceString.Length - 1, 1);

            /*pstop2018Entities1 db = new pstop2018Entities1();

            using (var uow = db.CreateUnitOfWork())
            {
                if (imageUpload != null && imageUpload.ContentLength > 0)
                {
                    var fileName = Path.GetRandomFileName() + Path.GetExtension(imageUpload.FileName);
                    var path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads"), fileName);
                    imageUpload.SaveAs(path);
                    achievement.ImageLoc = path;
                }

                uow.Add(achievement);
                Save(uow);
                return true;
            }*/


            //ViewBag.Image = ConvertHTMLToImage(sourceString);
            return View(missingPeople);
        }

        /*[System.Web.Http.HttpGet]
        [ActionName("ConvertHTMLToImage")]
        public HttpResponseMessage ConvertHTMLToImage(string filePath)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);

            FileStream fileStream = new FileStream(filePath + ".png", FileMode.Open);
            Image image = Image.FromStream(fileStream);
            MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Jpeg);

            result.Content = new ByteArrayContent(memoryStream.ToArray());
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            return result;
        }*/

    }
}