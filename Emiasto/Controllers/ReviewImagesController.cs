using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Emiasto.DAL;
using Emiasto.Models;

namespace Emiasto.Controllers
{
    public class ReviewImagesController : Controller
    {
        private CityContext db = new CityContext();

        // GET: ReviewImages
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var reviewImages = db.ReviewImages.Include(r => r.Review);
            return View(reviewImages.ToList());
        }

        // GET: ReviewImages/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReviewImage reviewImage = db.ReviewImages.Find(id);
            if (reviewImage == null)
            {
                return HttpNotFound();
            }
            return View(reviewImage);
        }

        // GET: ReviewImages/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.ReviewId = new SelectList(db.Reviews, "Id", "Text");
            return View();
        }

        // POST: ReviewImages/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,ReviewId,Image")] ReviewImage reviewImage)
        {
            HttpPostedFileBase file = Request.Files["Image"];
            if (file != null && file.ContentLength > 0)
            {
                reviewImage.Image = file.FileName;
            }
            ModelState.Clear();
            TryValidateModel(reviewImage);
            if (ModelState.IsValid)
            {
                file.SaveAs(HttpContext.Server.MapPath("~/Images/" + reviewImage.Image));

                db.ReviewImages.Add(reviewImage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ReviewId = new SelectList(db.Reviews, "Id", "Text", reviewImage.ReviewId);
            return View(reviewImage);
        }

        // GET: ReviewImages/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReviewImage reviewImage = db.ReviewImages.Find(id);
            if (reviewImage == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReviewId = new SelectList(db.Reviews, "Id", "Text", reviewImage.ReviewId);
            return View(reviewImage);
        }

        // POST: ReviewImages/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,ReviewId,Image")] ReviewImage reviewImage)
        {
            HttpPostedFileBase file = Request.Files["Image"];
            if (file != null && file.ContentLength > 0)
            {
                reviewImage.Image = file.FileName;
            }
            ModelState.Clear();
            TryValidateModel(reviewImage);
            if (ModelState.IsValid)
            {
                file.SaveAs(HttpContext.Server.MapPath("~/Images/" + reviewImage.Image));

                db.Entry(reviewImage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ReviewId = new SelectList(db.Reviews, "Id", "Text", reviewImage.ReviewId);
            return View(reviewImage);
        }

        // GET: ReviewImages/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReviewImage reviewImage = db.ReviewImages.Find(id);
            if (reviewImage == null)
            {
                return HttpNotFound();
            }
            return View(reviewImage);
        }

        // POST: ReviewImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            ReviewImage reviewImage = db.ReviewImages.Find(id);
            db.ReviewImages.Remove(reviewImage);
            db.SaveChanges();

            System.IO.File.Delete(HttpContext.Server.MapPath("~/Images/" + reviewImage.Image));
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
