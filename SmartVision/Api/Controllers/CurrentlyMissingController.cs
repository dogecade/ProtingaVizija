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

            return View(missingPeople);
        }

    }
}