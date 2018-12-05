using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Api.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Smart Vision";

            return View();
        }
        public ActionResult RegisterView()
        {
            ViewBag.Title = "Smart Vision";

            return View();
        }
        public ActionResult LoginView()
        {
            ViewBag.Title = "Smart Vision";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Title = "Smart Vision";

            return View();
        }
    }
}
