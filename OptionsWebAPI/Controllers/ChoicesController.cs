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
using DiplomaDataModel;

namespace OptionsWebAPI.Controllers
{
    public class ChoicesController : ApiController
    {
        private OptionPickerContext db = new OptionPickerContext();

        // GET: api/Choices
        public IQueryable<Choice> GetChoices()
        {
            IQueryable<Choice> result = db.Choices;
            foreach (Choice choice in result)
            {
                var q1 = from o in db.Options
                         where o.OptionId == choice.FirstChoiceOptionId
                         select o;
                var q2 = from o in db.Options
                         where o.OptionId == choice.SecondChoiceOptionId
                         select o;
                var q3 = from o in db.Options
                         where o.OptionId == choice.ThirdChoiceOptionId
                         select o;
                var q4 = from o in db.Options
                         where o.OptionId == choice.FourthChoiceOptionId
                         select o;
                choice.FirstOption = q1.FirstOrDefault();
                choice.SecondOption = q2.FirstOrDefault();
                choice.ThirdOption = q3.FirstOrDefault();
                choice.FourthOption = q4.FirstOrDefault();
            }
            return result;
        }

        // GET: api/Choices/5
        [ResponseType(typeof(Choice))]
        public IHttpActionResult GetChoice(int id)
        {
            Choice choice = db.Choices.Find(id);
            if (choice == null)
            {
                return NotFound();
            }

            return Ok(choice);
        }

        // PUT: api/Choices/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutChoice(int id, Choice choice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != choice.ChoiceId)
            {
                return BadRequest();
            }

            db.Entry(choice).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChoiceExists(id))
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

        // POST: api/Choices
        [ResponseType(typeof(Choice))]
        public IHttpActionResult PostChoice(Choice choice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Choices.Add(choice);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = choice.ChoiceId }, choice);
        }

        // DELETE: api/Choices/5
        [ResponseType(typeof(Choice))]
        public IHttpActionResult DeleteChoice(int id)
        {
            Choice choice = db.Choices.Find(id);
            if (choice == null)
            {
                return NotFound();
            }

            db.Choices.Remove(choice);
            db.SaveChanges();

            return Ok(choice);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ChoiceExists(int id)
        {
            return db.Choices.Count(e => e.ChoiceId == id) > 0;
        }
    }
}