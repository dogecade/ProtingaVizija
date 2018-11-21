using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using AdminWeb.Models;
using BusService;

namespace AdminWeb.Controllers
{
    public class HomeController : Controller
    {
        private static List<string> URLlist = new List<string> { "https://webcam1.lpl.org/axis-cgi/mjpg/video.cgi", "http://quadcam.unrfound.unr.edu/axis-cgi/mjpg/video.cgi" };
        public ActionResult Index()
        {
            var model = new BusModel();
            var allBuses = BusHelpers.GetAllAvailableBusses();
            var items = new List<SelectListItem>();

            foreach (var bus in allBuses)
            {
                items.Add(new SelectListItem() { Text = bus.Name, Value = bus.Id });
            }

            model.Buses = items;
            return View(model);
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
        [HttpPost]
        public ActionResult RemoveStream(String streamUrl)
        {
            Debug.WriteLine(streamUrl);
            if (!streamUrl.Equals(""))
            {
                streamUrl = streamUrl.Replace(@"""", string.Empty);
                URLlist.Remove(streamUrl);
            }
            return Json(new { result = "succ" }, JsonRequestBehavior.AllowGet);
        }

    }
}