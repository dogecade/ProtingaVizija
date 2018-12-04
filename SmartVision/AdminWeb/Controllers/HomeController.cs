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

            if (allBuses != null)
                foreach (var bus in allBuses)
                {
                    items.Add(new SelectListItem() { Text = bus.Name, Value = bus.Id });
                }

            model.Buses = items;
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddCamera(CameraPropertiesModel properties)
        {
            Debug.WriteLine(properties.StreamUrl);
            if (properties.StreamUrl != null)
            {
                properties.StreamUrl = properties.StreamUrl.Replace(@"""", string.Empty);
                var (url, id) = await MJPEGStreamManager.AddStream(properties.StreamUrl, properties);
                return Json(new { url, id }, JsonRequestBehavior.AllowGet);
            }
            else
                return null;
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