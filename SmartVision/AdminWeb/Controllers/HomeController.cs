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
            return View();
        }

        public ActionResult Configuration()
        {
            var model = new BusModel();
            var allBuses = BusHelpers.GetAllAvailableBusses();
            var items = new List<SelectListItem>();

            if (allBuses != null)
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
                var (url, id) = MJPEGStreamManager.AddStream(streamUrl);
                return Json(new { url, id }, JsonRequestBehavior.AllowGet);
            }
            else
                return null;
        }

        [HttpPost]
        public ActionResult ChangeProperties(CameraPropertiesModel properties)
        {
            MJPEGStreamManager.Processor.UpdateProperties(properties);
            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetStreamList()
        {
            return Json(new { result = MJPEGStreamManager.GetStreams() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RemoveStream(string streamId)
        {
            Debug.WriteLine(streamId);
            if (streamId != "")
            {
                streamId = streamId.Replace(@"""", string.Empty);
                MJPEGStreamManager.RemoveStream(streamId);
                return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
            }
            else
                return null;
        }

    }
}