using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DiplomaDataModel;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
namespace OptionsWebSite.Controllers
{
    public class ChoicesController : Controller
    {
        private OptionPickerContext db = new OptionPickerContext();
        // http://stackoverflow.com/questions/20925822/asp-mvc5-identity-how-to-get-current-applicationuser/22746384
        private Models.ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

        // GET: Choices
        public ActionResult Index()
        {
            var choices = db.Choices.Include(c => c.FirstOption).Include(c => c.FourthOption).Include(c => c.SecondOption).Include(c => c.ThirdOption).Include(c => c.YearTerm);
            //ViewBag.Message = TempData["choiceExists"].ToString();
            return View(choices.ToList());
        }

        // GET: Choices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Choice choice = db.Choices.Find(id);
            if (choice == null)
            {
                return HttpNotFound();
            }
            return View(choice);
        }

        public String getYearTerm()
        {
            var q = from y in db.YearTerms
                    where y.IsDefault == true
                    select y;
            var yt = q.FirstOrDefault();
            switch (yt.Term)
            {
                case 10:
                    return "Winter " + yt.Year;
                case 20:
                    return "Spring/Summer " + yt.Year;
                case 30:
                    return "Fall " + yt.Year;
            }
            return "" + yt.Year;
        }

        public int getTermId()
        {
            var q = from y in db.YearTerms
                    where y.IsDefault == true
                    select y;
            var yt = q.FirstOrDefault();
            return yt.YearTermId;
        }

        public Boolean exists(String user, int term)
        {
            var q = from c in db.Choices
                    where c.StudentId == user
                    && c.YearTermId == term
                    select c;
            var yt = q.FirstOrDefault();
            if (yt == null)
                return true;
            return false;
        }

        public List<Option> getActiveOptions()
        {
            Option[] options = db.Options.ToArray();
            List<Option> activeOptions = new List<Option>();
            for (var i = 0; i < options.Length; i++)
            {
                if (options[i].IsActive == true)
                {
                    activeOptions.Add(options[i]);
                }
            }
            return activeOptions;
        }
   
        // GET: Choices/Create
        [Authorize]
        public ActionResult Create()
        {
            if (exists(this.user.UserName, getTermId()))
            {
                List<Option> activeOptions = getActiveOptions();
                ViewBag.FirstChoiceOptionId = new SelectList(activeOptions, "OptionId", "Title");
                ViewBag.FourthChoiceOptionId = new SelectList(activeOptions, "OptionId", "Title");
                ViewBag.SecondChoiceOptionId = new SelectList(activeOptions, "OptionId", "Title");
                ViewBag.ThirdChoiceOptionId = new SelectList(activeOptions, "OptionId", "Title");
                ViewBag.YearTermId = "" + getTermId();
                ViewBag.YearTerm = getYearTerm();
                ViewBag.StudentId = this.user.UserName;
                return View();
            }
            else
            {
                TempData["choiceExists"] = "Options have already been selected for " + this.user.UserName + " for the current term.";
                return RedirectToAction("Index");
            }
        }
   
        // POST: Choices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ChoiceId,YearTermId,StudentId,StudentFirstName,StudentLastName,FirstChoiceOptionId,SecondChoiceOptionId,ThirdChoiceOptionId,FourthChoiceOptionId,SelectionDate")] Choice choice)
        {
            int[] choices = new int[4];
            choices[0] = (int)choice.FirstChoiceOptionId;
            choices[1] = (int)choice.SecondChoiceOptionId;
            choices[2] = (int)choice.ThirdChoiceOptionId;
            choices[3] = (int)choice.FourthChoiceOptionId;
            bool isUnique = choices.Distinct().Count() == choices.Count();
            if (!isUnique)
            {
                ModelState.AddModelError(string.Empty, "All option choices need to be unique");
            }
            if (ModelState.IsValid)
            {
                db.Choices.Add(choice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<Option> activeOptions = getActiveOptions();
            ViewBag.FirstChoiceOptionId = new SelectList(activeOptions, "OptionId", "Title", choice.FirstChoiceOptionId);
            ViewBag.FourthChoiceOptionId = new SelectList(activeOptions, "OptionId", "Title", choice.FourthChoiceOptionId);
            ViewBag.SecondChoiceOptionId = new SelectList(activeOptions, "OptionId", "Title", choice.SecondChoiceOptionId);
            ViewBag.ThirdChoiceOptionId = new SelectList(activeOptions, "OptionId", "Title", choice.ThirdChoiceOptionId);
            ViewBag.YearTermId = choice.YearTermId;
            ViewBag.YearTerm = getYearTerm();
            ViewBag.StudentId = choice.StudentId;
            return View(choice);
        }
   
        // GET: Choices/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Choice choice = db.Choices.Find(id);
            if (choice == null)
            {
                return HttpNotFound();
            }
            List<Option> activeOptions = getActiveOptions();
            ViewBag.FirstChoiceOptionId = new SelectList(activeOptions, "OptionId", "Title", choice.FirstChoiceOptionId);
            ViewBag.FourthChoiceOptionId = new SelectList(activeOptions, "OptionId", "Title", choice.FourthChoiceOptionId);
            ViewBag.SecondChoiceOptionId = new SelectList(activeOptions, "OptionId", "Title", choice.SecondChoiceOptionId);
            ViewBag.ThirdChoiceOptionId = new SelectList(activeOptions, "OptionId", "Title", choice.ThirdChoiceOptionId);
            ViewBag.YearTermId = "" + getTermId();
            ViewBag.YearTerm = getYearTerm();
            ViewBag.StudentId = this.user.UserName;
            return View(choice);
        }
  
        // POST: Choices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ChoiceId,YearTermId,StudentId,StudentFirstName,StudentLastName,FirstChoiceOptionId,SecondChoiceOptionId,ThirdChoiceOptionId,FourthChoiceOptionId,SelectionDate")] Choice choice)
        {
            int[] choices = new int[4];
            choices[0] = (int)choice.FirstChoiceOptionId;
            choices[1] = (int)choice.SecondChoiceOptionId;
            choices[2] = (int)choice.ThirdChoiceOptionId;
            choices[3] = (int)choice.FourthChoiceOptionId;
            bool isUnique = choices.Distinct().Count() == choices.Count();
            if (!isUnique)
            {
                ModelState.AddModelError(string.Empty, "All option choices need to be unique");
            }
            if (ModelState.IsValid)
            {
                db.Entry(choice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<Option> activeOptions = getActiveOptions();
            ViewBag.FirstChoiceOptionId = new SelectList(activeOptions, "OptionId", "Title", choice.FirstChoiceOptionId);
            ViewBag.FourthChoiceOptionId = new SelectList(activeOptions, "OptionId", "Title", choice.FourthChoiceOptionId);
            ViewBag.SecondChoiceOptionId = new SelectList(activeOptions, "OptionId", "Title", choice.SecondChoiceOptionId);
            ViewBag.ThirdChoiceOptionId = new SelectList(activeOptions, "OptionId", "Title", choice.ThirdChoiceOptionId);
            ViewBag.YearTermId = choice.YearTermId;
            ViewBag.YearTerm = choice.YearTerm;
            ViewBag.StudentId = choice.StudentId;
            return View(choice);
        }

        // GET: Choices/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {

            var q = from y in db.Choices
                    select y;
            var c = q.ToList();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Choice choice = db.Choices.Find(id);
            if (choice == null)
            {
                return HttpNotFound();
            }
            return View(choice);
        }
   
        // POST: Choices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Choice choice = db.Choices.Find(id);
            db.Choices.Remove(choice);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}