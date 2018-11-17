using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

using Emiasto.DAL;
using Emiasto.Models;

namespace Emiasto.Controllers
{
    public class ReviewLikesController : Controller
    {
        private CityContext db = new CityContext();

        [Authorize]
        public RedirectToRouteResult Like([Bind(Include = "Id,ReviewId,UserId")] ReviewLike reviewLike, [Bind(Include = "FacilityId")] int facilityId)
        {
            reviewLike.UserId = db.Users.Single(user => user.UserName == reviewLike.UserId).Id;
            db.ReviewLikes.Add(reviewLike);
            db.SaveChanges();

            return RedirectToAction("Details", "Facilities", new { id = facilityId });
        }

        // GET: ReviewLikes
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var reviewLikes = db.ReviewLikes.Include(r => r.Review);
            return View(reviewLikes.ToList());
        }

        // GET: ReviewLikes/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReviewLike reviewLike = db.ReviewLikes.Find(id);
            if (reviewLike == null)
            {
                return HttpNotFound();
            }
            return View(reviewLike);
        }

        // GET: ReviewLikes/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.ReviewId = new SelectList(db.Reviews, "Id", "Text");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Fullname");
            return View();
        }

        // POST: ReviewLikes/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,ReviewId,UserId")] ReviewLike reviewLike)
        {
            if (ModelState.IsValid)
            {
                db.ReviewLikes.Add(reviewLike);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ReviewId = new SelectList(db.Reviews, "Id", "UserId", reviewLike.ReviewId);
            return View(reviewLike);
        }

        // GET: ReviewLikes/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReviewLike reviewLike = db.ReviewLikes.Find(id);
            if (reviewLike == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReviewId = new SelectList(db.Reviews, "Id", "Text", reviewLike.Review.Text);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Fullname", reviewLike.UserId);
            return View(reviewLike);
        }

        // POST: ReviewLikes/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,ReviewId,UserId")] ReviewLike reviewLike)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reviewLike).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ReviewId = new SelectList(db.Reviews, "Id", "UserId", reviewLike.ReviewId);
            return View(reviewLike);
        }

        // GET: ReviewLikes/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReviewLike reviewLike = db.ReviewLikes.Find(id);
            if (reviewLike == null)
            {
                return HttpNotFound();
            }
            return View(reviewLike);
        }

        // POST: ReviewLikes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            ReviewLike reviewLike = db.ReviewLikes.Find(id);
            db.ReviewLikes.Remove(reviewLike);
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
