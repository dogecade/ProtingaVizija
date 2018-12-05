using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Constants;
using Newtonsoft.Json.Linq;

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
        public ActionResult CaptchaVerificationRegister()
        {
            var response = Request["g-recaptcha-response"];
            string secretKey = Keys.recaptchaSecretKey;
            string result;
            using (var client = new WebClient())
            {
                result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            }
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");

            if (status)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Message = "ReCaptcha validation failed";

            return View("RegisterView");
        }

        public ActionResult CaptchaVerificationLogin()
        {
            var response = Request["g-recaptcha-response"];
            string secretKey = Keys.recaptchaSecretKey;
            string result;
            using (var client = new WebClient())
            {
                result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            }
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");

            if (status)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Message = "ReCaptcha validation failed";

            return View("LoginView");
        }
    }
}
