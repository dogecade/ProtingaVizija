using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Mvc;
using AdminWeb.Models;
using BusService;
using StreamingBackend;

namespace AdminWeb.Controllers
{
    public class HomeController : Controller
    {
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
        public ActionResult AddCamera(string streamUrl)
        {
            Debug.WriteLine(streamUrl);
            if (streamUrl != null)
            {
                streamUrl = streamUrl.Replace(@"""", string.Empty);
                MJPEGStreamManager.AddStream(streamUrl);
            }
            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetStreamList()
        {
            return Json(new { result = MJPEGStreamManager.GetStreamUrls() }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult RemoveStream(string streamUrl)
        {
            Debug.WriteLine(streamUrl);
            if (streamUrl != "")
            {
                streamUrl = streamUrl.Replace(@"""", string.Empty);
                MJPEGStreamManager.RemoveStream(streamUrl);
            }
            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }

    }
}