﻿//using FaceAnalysis;
using FaceAnalysis;
using Objects.Person;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Net.Http;
using Helpers;
using Web.Models;
using Web;
using Web.Controllers;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using Newtonsoft.Json;
using System.Data.Entity;

namespace Api.Controllers

{
    [Authorize]
    public class AddPersonController : Controller
    {
        
        // GET: AddPerson
        public ActionResult AddContactPerson()
        {
            return View();
        }
        public ActionResult AddMissingPerson()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CheckImage()
        {
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic = System.Web.HttpContext.Current.Request.Files["Image"];
                Image image = Image.FromStream(pic.InputStream);

                using (var uploadedImage = HelperMethods.ProcessImage(new Bitmap(image)))
                {
                    FrameAnalysisJSON result = await FaceProcessor.ProcessFrame(new Bitmap(uploadedImage));
                    if (result == null || result.Faces.Count == 0)
                        return Json(new { result = new List<CroppedFace>() }
                    , JsonRequestBehavior.AllowGet);

                    List<CroppedFace> croppedFaces = new List<CroppedFace>();
                    foreach (var face in result.CroppedFaces(uploadedImage, 25))
                    {
                        croppedFaces.Add(face);
                    }
                    return Json(new { result = croppedFaces }
                    , JsonRequestBehavior.AllowGet);
                }

            }
            return null;
        }
        [HttpPost]
        public async Task<ActionResult> AddMissingPerson (MissingPerson missingPersons)
        {
            HttpClientWrapper httpClient = new HttpClientWrapper();
            if (await new FaceApiCalls(httpClient).AddFaceToFaceset(missingPersons.faceToken) == null)
            {
                //TODO: have some proper things to do here.
                throw new SystemException("ding dong");
            }
            byte[] byteArray = Convert.FromBase64String(missingPersons.faceImg);
            using (var ms = new MemoryStream(byteArray, 0, byteArray.Length))
            {
                Bitmap image = (Bitmap)Bitmap.FromStream(ms);
                string imgLoc = await httpClient.PostImageToApiString(image);
                missingPersons.faceImg = imgLoc;
                HttpContent content = await httpClient.PostMissingPersonToApiAsync(missingPersons);
                var name = User.Identity.Name;
                ApplicationDbContext db = new ApplicationDbContext();
                ApplicationUser applicationUser= db.Users.FirstOrDefault(x => x.UserName == name);
                
                string contactPerson = await httpClient.Get("http://viltomas.eu/api/ContactPersons/" + applicationUser.idas);
                string missingParsed = await content.ReadAsStringAsync();
                ContactPerson contactP = JsonConvert.DeserializeObject<ContactPerson>(contactPerson);
                MissingPerson missingP = JsonConvert.DeserializeObject<MissingPerson>(missingParsed);
                MissingContact mc = new MissingContact
                {
                    contactPerson = contactP,
                    missingPerson = missingP
                };
                var contentResponse = await httpClient.PostRelToApi(mc);
                var contentString = await content.ReadAsStringAsync();
                return View("View");
            }
        }
        [HttpPost]
        public async Task<ActionResult> AddContactPerson (ContactPerson contactPersons)
        {
            HttpClientWrapper httpClient = new HttpClientWrapper();
            HttpContent content = await httpClient.PostContactPersonToApiAsync(contactPersons);
            string contactParsed = await content.ReadAsStringAsync();
            return View("View");
        }
        [HttpPost]
        public async Task<string> AddRelationship(MissingContact missing)
        {
            HttpClientWrapper httpClient = new HttpClientWrapper();
            HttpContent content = await httpClient.PostRelToApi(missing);
            string contactParsed = await content.ReadAsStringAsync();
            return contactParsed;
        }
        public ActionResult FacesModalView()
        {

            return PartialView();
        }

    }
}
