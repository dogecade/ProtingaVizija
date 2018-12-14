using Api.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Api.Controllers
{
    public class MissingContactController : ApiController
    {
        private pstop2018Entities1 db = new pstop2018Entities1();
        [HttpPost]
        public string CreateRel(MissingContact missingContact)
        {
            if (!ModelState.IsValid)
            {
                return "bolgas modelis";
            }
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.ContactPersons.Attach(missingContact.contactPerson);
                db.MissingPersons.Attach(missingContact.missingPerson);
                missingContact.contactPerson.MissingPersons.Add(missingContact.missingPerson);
                missingContact.missingPerson.ContactPersons.Add(missingContact.contactPerson);
                db.SaveChanges();

                return "ok";
            }
            catch (System.Exception e)
            {
                Trace.WriteLine(e);
                return e.ToString();
            }
        }
    }
}
