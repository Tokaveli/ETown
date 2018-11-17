using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

using Emiasto.DAL;
using Emiasto.Models;

namespace Emiasto.Controllers
{
    public class ReviewsController : Controller
    {
        private CityContext db = new CityContext();

        // GET: Reviews
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var reviews = db.Reviews.Include(r => r.Facility);
            return View(reviews.ToList());
        }

        // GET: Reviews/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // GET: Reviews/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.FacilityId = new SelectList(db.Facilities, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Fullname");
            return View();
        }

        // POST: Reviews/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,FacilityId,UserId,Rating,Text,Release")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FacilityId = new SelectList(db.Facilities, "Id", "Name", review.FacilityId);
            return View(review);
        }

        // GET: Reviews/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            ViewBag.FacilityId = new SelectList(db.Facilities, "Id", "Name", review.FacilityId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Fullname", review.UserId);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,FacilityId,UserId,Rating,Text,Release")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FacilityId = new SelectList(db.Facilities, "Id", "Name", review.FacilityId);
            return View(review);
        }

        // GET: Reviews/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
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
