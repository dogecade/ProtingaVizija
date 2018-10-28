using System.Linq;
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