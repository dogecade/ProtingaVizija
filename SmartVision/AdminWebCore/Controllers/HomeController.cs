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
        private readonly IFaceProcessingService processingService;

        public HomeController(IFaceProcessingService processingService)
        {
            this.processingService = processingService;
        }

        public IActionResult Index()
        {
            return View();
        }

        private (string apiKey, string apiSecret, string facesetToken) GetApiDetailsFromConfig()
        {
            return (Properties.Settings.Default.ApiKey,
                    Properties.Settings.Default.ApiSecret,
                    Properties.Settings.Default.FacesetToken);
        }

        private (string postCode, string houseNo, string street, string city, string country, int busId, bool isBus) GetCameraDetailsFromConfig()
        {
            return (Properties.Settings.Default.PostCode,
                    Properties.Settings.Default.HouseNo,
                    Properties.Settings.Default.Street,
                    Properties.Settings.Default.City,
                    Properties.Settings.Default.Country,
                    Properties.Settings.Default.BusId,
                    Properties.Settings.Default.IsBus);
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
            Properties.Settings.Default.ApiKey = properties.ApiKey;
            Properties.Settings.Default.ApiSecret = properties.ApiSecret;
            Properties.Settings.Default.FacesetToken = properties.FacesetToken;
            Properties.Settings.Default.PostCode = properties.PostalCode;
            Properties.Settings.Default.HouseNo = properties.HouseNumber;
            Properties.Settings.Default.Street = properties.StreetName;
            Properties.Settings.Default.City = properties.CityName;
            Properties.Settings.Default.Country = properties.CountryName;

            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();

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
            var (apiKey, apiSecret, facesetToken) = GetApiDetailsFromConfig();
            var (postCode, houseNo, street, city, country, busId, isBus) = GetCameraDetailsFromConfig();
            var properties = new CameraProperties(street, houseNo, city, country, postCode, busId, isBus);
            var keySet = new FaceAnalysis.ApiKeySet(apiKey, apiSecret, facesetToken);
            processingService.Processor.UpdateProperties(properties);
            processingService.Processor.UpdateKeys(keySet);
            
            if (!processingService.Processor.IsProcessing)
                await processingService.Processor.Start();
            else
                processingService.Processor.Stop();

            return Json(new { result = "success" });
        }
    }
}
