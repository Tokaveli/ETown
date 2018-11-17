using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Emiasto.DAL;
using Emiasto.Models;

namespace Emiasto.Controllers
{
    public class FacilityImagesController : Controller
    {
        private CityContext db = new CityContext();

        // GET: FacilityImages
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var facilityImages = db.FacilityImages.Include(f => f.Facility);
            return View(facilityImages.ToList());
        }

        // GET: FacilityImages/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FacilityImage facilityImage = db.FacilityImages.Find(id);
            if (facilityImage == null)
            {
                return HttpNotFound();
            }
            return View(facilityImage);
        }

        // GET: FacilityImages/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.FacilityId = new SelectList(db.Facilities, "Id", "Name");
            return View();
        }

        // POST: FacilityImages/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,FacilityId,Image")] FacilityImage facilityImage)
        {
            HttpPostedFileBase file = Request.Files["Image"];
            if (file != null && file.ContentLength > 0)
            {
                facilityImage.Image = file.FileName;
            }
            ModelState.Clear();
            TryValidateModel(facilityImage);
            if (ModelState.IsValid)
            {
                file.SaveAs(HttpContext.Server.MapPath("~/Images/" + facilityImage.Image));

                db.FacilityImages.Add(facilityImage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FacilityId = new SelectList(db.Facilities, "Id", "Name", facilityImage.FacilityId);
            return View(facilityImage);
        }

        // GET: FacilityImages/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FacilityImage facilityImage = db.FacilityImages.Find(id);
            if (facilityImage == null)
            {
                return HttpNotFound();
            }
            ViewBag.FacilityId = new SelectList(db.Facilities, "Id", "Name", facilityImage.FacilityId);
            return View(facilityImage);
        }

        // POST: FacilityImages/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,FacilityId,Image")] FacilityImage facilityImage)
        {
            HttpPostedFileBase file = Request.Files["Image"];
            if (file != null && file.ContentLength > 0)
            {
                facilityImage.Image = file.FileName;
            }
            ModelState.Clear();
            TryValidateModel(facilityImage);
            if (ModelState.IsValid)
            {
                file.SaveAs(HttpContext.Server.MapPath("~/Images/" + facilityImage.Image));

                db.Entry(facilityImage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FacilityId = new SelectList(db.Facilities, "Id", "Name", facilityImage.FacilityId);
            return View(facilityImage);
        }

        // GET: FacilityImages/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FacilityImage facilityImage = db.FacilityImages.Find(id);
            if (facilityImage == null)
            {
                return HttpNotFound();
            }
            return View(facilityImage);
        }

        // POST: FacilityImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            FacilityImage facilityImage = db.FacilityImages.Find(id);
            db.FacilityImages.Remove(facilityImage);
            db.SaveChanges();

            System.IO.File.Delete(HttpContext.Server.MapPath("~/Images/" + facilityImage.Image));
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
