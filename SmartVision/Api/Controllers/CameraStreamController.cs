using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

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
        public void CaptureSnapshot(string imgBase64)
        {
            imgBase64 = imgBase64.Replace(" ", "+");
            Byte[] bitmapData = Convert.FromBase64String(FixBase64ForImage(imgBase64));
            System.IO.MemoryStream streamBitmap = new System.IO.MemoryStream(bitmapData);
            Bitmap bitImage = new Bitmap((Bitmap)Image.FromStream(streamBitmap));

            bitImage.Save(@"C:\dev\foto.png");
        }
        public string FixBase64ForImage(string Image)
        {
            System.Text.StringBuilder sbText = new System.Text.StringBuilder(Image, Image.Length);
            sbText.Replace("\r\n", String.Empty); sbText.Replace(" ", String.Empty);
            return sbText.ToString();
        }
    }
}