using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdminWeb.Models;
using System.Configuration;
using BusService;
using Microsoft.AspNetCore.Mvc.Rendering;
using AForge.Video;
using Objects.CameraProperties;

namespace AdminWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly FaceProcesssingService processingService;

        public HomeController(FaceProcesssingService processingService)
        {
            this.processingService = processingService;
        }

        public IActionResult Index()
        {
            return View();
        }
        private (string apiKey, string apiSecret, string facesetToken) GetApiDetailsFromConfig()
        {
            return (ConfigurationManager.AppSettings["ApiKey"],
                    ConfigurationManager.AppSettings["ApiSecret"],
                    ConfigurationManager.AppSettings["FacesetToken"]);
        }

        private (string postCode, string houseNo, string street, string city, string country, int busId, bool isBus) GetCameraDetailsFromConfig()
        {
            if (!int.TryParse(ConfigurationManager.AppSettings["BusId"], out int busId))
                busId = 0;
            if (!bool.TryParse(ConfigurationManager.AppSettings["IsBus"], out bool isBus))
                isBus = false;
            return (ConfigurationManager.AppSettings["PostCode"],
                    ConfigurationManager.AppSettings["HouseNo"],
                    ConfigurationManager.AppSettings["Street"],
                    ConfigurationManager.AppSettings["City"],
                    ConfigurationManager.AppSettings["Country"],
                    busId,
                    isBus);
        }

        public ActionResult Configuration()
        {
            CameraPropertiesModel propertiesModel = new CameraPropertiesModel
            {
                BusModel = new BusModel()
            };
            var allBuses = BusHelpers.GetAllAvailableBuses();
            if (allBuses != null)
                propertiesModel.BusModel.Buses = allBuses
                    .Select(bus => new SelectListItem { Text = bus.Name, Value = bus.Id });
            else
                propertiesModel.BusModel.Buses = Enumerable.Empty<SelectListItem>();
            (propertiesModel.ApiKey, propertiesModel.ApiSecret, propertiesModel.FacesetToken) = GetApiDetailsFromConfig();
            (propertiesModel.PostalCode,
            propertiesModel.HouseNumber,
            propertiesModel.StreetName,
            propertiesModel.CityName,
            propertiesModel.CountryName,
            propertiesModel.BusId,
            propertiesModel.IsBus) = GetCameraDetailsFromConfig();
            propertiesModel.IsProcessing = processingService.Processor.IsProcessing;
            return View(propertiesModel);
        }

        [HttpPost]
        public async Task<ActionResult> AddCamera(string streamUrl, string streamType)
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
            var (url, id) = await processingService.AddStreamAsync(stream);
            return Json(new { url, id });
        }

        [HttpPost]
        public ActionResult ChangeProperties(CameraPropertiesModel properties)
        {
            processingService.Processor.UpdateProperties(properties);
            processingService.Processor.UpdateKeys(new FaceAnalysis.ApiKeySet(properties.ApiKey, properties.ApiSecret, properties.FacesetToken));
            /* TODO: need a better way for this
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            config.AppSettings.Settings["ApiKey"].Value = properties.ApiKey;
            config.AppSettings.Settings["ApiSecret"].Value = properties.ApiSecret;
            config.AppSettings.Settings["FacesetToken"].Value = properties.FacesetToken;
            config.AppSettings.Settings["PostCode"].Value = properties.PostalCode;
            config.AppSettings.Settings["HouseNo"].Value = properties.HouseNumber;
            config.AppSettings.Settings["Street"].Value = properties.StreetName;
            config.AppSettings.Settings["City"].Value = properties.CityName;
            config.AppSettings.Settings["Country"].Value = properties.CountryName;

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            */
            return RedirectToAction("Configuration");
        }

        [HttpGet]
        public ActionResult GetStreamList()
        {
            return Json(new { result = processingService.GetStreams() });
        }

        [HttpPost]
        public ActionResult RemoveStream(string streamId)
        {
            Debug.WriteLine($"Removing stream {streamId}");
            if (streamId != "")
            {
                processingService.RemoveStream(streamId);
                return Json(new { result = "success" });
            }
            else
                return null;
        }

        [HttpGet]
        public async Task<ActionResult> StartStopProcessor()
        {
            /*
            var (apiKey, apiSecret, facesetToken) = GetApiDetailsFromConfig();
            var (postCode, houseNo, street, city, country, busId, isBus) = GetCameraDetailsFromConfig();
            var properties = new CameraProperties(street, houseNo, city, country, postCode, busId, isBus);
            var keySet = new FaceAnalysis.ApiKeySet(apiKey, apiSecret, facesetToken);
            processingService.Processor.UpdateProperties(properties);
            processingService.Processor.UpdateKeys(keySet);
            */
            if (!processingService.Processor.IsProcessing)
                await processingService.Processor.Start();
            else
                processingService.Processor.Stop();

            return Json(new { result = "success" });
        }
    }
}
