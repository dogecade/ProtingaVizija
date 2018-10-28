using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Api.Controllers
{
    public class AddPersonController : Controller
    {
        // GET: AddPerson
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MissingPersonView()
        {
            return View();
        }
    }
}