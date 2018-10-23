using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Api.Models;

namespace Api.Controllers
{
    public class MissingPersonsController : ApiController
    {
        private pstop2018Entities1 db = new pstop2018Entities1();

        // GET: api/MissingPersons
        public IQueryable<MissingPerson> GetMissingPersons()
        {
            return db.MissingPersons;
        }

        // GET: api/MissingPersons/5
        [ResponseType(typeof(MissingPerson))]
        public IHttpActionResult GetMissingPerson(int id)
        {
            MissingPerson missingPerson = db.MissingPersons.Find(id);
            if (missingPerson == null)
            {
                return NotFound();
            }

            return Ok(missingPerson);
        }

        // PUT: api/MissingPersons/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMissingPerson(int id, MissingPerson missingPerson)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != missingPerson.Id)
            {
                return BadRequest();
            }

            db.Entry(missingPerson).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MissingPersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/MissingPersons
        [ResponseType(typeof(MissingPerson))]
        public IHttpActionResult PostMissingPerson(MissingPerson missingPerson)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MissingPersons.Add(missingPerson);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = missingPerson.Id }, missingPerson);
        }

        // DELETE: api/MissingPersons/5
        [ResponseType(typeof(MissingPerson))]
        public IHttpActionResult DeleteMissingPerson(int id)
        {
            MissingPerson missingPerson = db.MissingPersons.Find(id);
            if (missingPerson == null)
            {
                return NotFound();
            }

            db.MissingPersons.Remove(missingPerson);
            db.SaveChanges();

            return Ok(missingPerson);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MissingPersonExists(int id)
        {
            return db.MissingPersons.Count(e => e.Id == id) > 0;
        }
    }
}