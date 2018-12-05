using System.Diagnostics;
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

            foreach (var dx in missingPeople)
            {
                dx.faceImg = dx.faceImg.Replace("\\", "");
                Debug.WriteLine(dx.firstName);
            }
            return View(missingPeople);
        }
        public ActionResult MoreInformationModal()
        {
            return PartialView();
        }
    }
}