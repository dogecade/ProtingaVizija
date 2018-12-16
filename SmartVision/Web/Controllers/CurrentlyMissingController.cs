using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using FaceAnalysis;

namespace Api.Controllers
{
    public class CurrentlyMissingController : Controller
    {
        // GET: CurrentlyMissing
        public async Task<ActionResult> Index()
        {
            List<Objects.Person.MissingPerson> missingPeople = await new CallsToDb().GetPeopleData();

            foreach (var person in missingPeople)
            {
                person.faceImg = person.faceImg.Replace("\\", "");
            }
            return View(missingPeople);
        }
        public ActionResult MoreInformationModal()
        {
            return PartialView();
        }
    }
}