using Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Api.Controllers
{
    public class AddPersonController : Controller
    {
        private pstop2018Entities1 db = new pstop2018Entities1();
        // GET: AddPerson
        public ActionResult Index()
        {
            ViewBag.Title = "Smart Vision";

            return View("Index");
        }
        // POST: AddPerson
        [HttpPost]
        public ActionResult AddContactPerson(ContactPerson model)
        {

            if (ModelState.IsValid)
            {
                db.ContactPersons.Add(model);
                db.SaveChanges();
                return View("View");

            }
            return View(model);
        }
    }
}