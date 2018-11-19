using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace AdminWeb.Controllers
{
    public class HomeController : Controller
    {
        private static List<string> URLlist = new List<string>{ "https://webcam1.lpl.org/axis-cgi/mjpg/video.cgi" , "http://quadcam.unrfound.unr.edu/axis-cgi/mjpg/video.cgi" };
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddCamera(String streamUrl)
        {
            Debug.WriteLine(streamUrl);
            if (!streamUrl.Equals(""))
            {
                streamUrl = streamUrl.Replace(@"""", string.Empty);
                URLlist.Add(streamUrl);
            }
            return Json(new { result = "succ" }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetStreamList()
        {
            return Json(new { result = URLlist }, JsonRequestBehavior.AllowGet);
        }

    }
}