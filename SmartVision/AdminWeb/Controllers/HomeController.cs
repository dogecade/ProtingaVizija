using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using AdminWeb.Models;
using AForge.Video;
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
            //TODO: fetch camera properties.
            CameraPropertiesModel propertiesModel = new CameraPropertiesModel
            {
                BusModel = new BusModel()
            };
            var allBuses = BusHelpers.GetAllAvailableBusses();
            if (allBuses != null)
                propertiesModel.BusModel.Buses = allBuses
                    .Select(bus => new SelectListItem { Text = bus.Name, Value = bus.Id });
            propertiesModel.ApiKey = ConfigurationManager.AppSettings["ApiKey"];
            propertiesModel.ApiSecret = ConfigurationManager.AppSettings["ApiSecret"];
            propertiesModel.FacesetToken = ConfigurationManager.AppSettings["FacesetToken"];
            return View(propertiesModel);   
        }

        [HttpPost]
        public ActionResult AddCamera(string streamUrl, string streamType)
        {

            if (string.IsNullOrWhiteSpace(streamUrl))
                return null;
            Debug.WriteLine($"Adding stream {streamUrl}");
            IVideoSource stream;
            switch (streamType)
            {
                case "jpeg":
                    stream = new JPEGStream(streamUrl);
                    break;
                case "mjpeg":
                    stream = new MJPEGStream(streamUrl);
                    break;
                default:
                    return null;
            }
            var (url, id) = MJPEGStreamManager.AddStream(stream);
            return Json(new { url, id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ChangeProperties(CameraPropertiesModel properties)
        {
            //TODO: save camera properties to config as well.
            MJPEGStreamManager.Processor.UpdateProperties(properties);
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            config.AppSettings.Settings["ApiKey"].Value = properties.ApiKey;
            config.AppSettings.Settings["ApiSecret"].Value = properties.ApiSecret;
            config.AppSettings.Settings["FacesetToken"].Value = properties.FacesetToken;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            return RedirectToAction("Configuration");
        }

        [HttpGet]
        public ActionResult GetStreamList()
        {
            return Json(new { result = MJPEGStreamManager.GetStreams() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RemoveStream(string streamId)
        {
            Debug.WriteLine($"Removing stream {streamId}");
            if (streamId != "")
            {
                MJPEGStreamManager.RemoveStream(streamId);
                return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
            }
            else
                return null;
        }

        [HttpGet]
        public async Task<ActionResult> StartProcessor()
        {
            //TODO: fetch latest properties from config and set that before starting.
            //TODO: stopping
            await MJPEGStreamManager.Processor.Start();
            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }

    }
}