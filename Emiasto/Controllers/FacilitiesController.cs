using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

using PagedList;

using Emiasto.DAL;
using Emiasto.Models;

namespace Emiasto.Controllers
{
    public class FacilitiesController : Controller
    {
        private CityContext db = new CityContext();

        // GET: Facilities
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CategorySortParm = sortOrder == "Category" ? "category_desc" : "Category";
            ViewBag.AddressSortParm = sortOrder == "Address" ? "address_desc" : "Address";
            ViewBag.WebsiteSortParm = sortOrder == "Website" ? "website_desc" : "Website";
            ViewBag.PhoneSortParm = sortOrder == "Phone" ? "phone_desc" : "Phone";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            var facilities = db.Facilities.Include(f => f.Category);
            if (!string.IsNullOrEmpty(searchString))
            {
                facilities = facilities.Where(s => s.Name.Contains(searchString) || s.Category.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    facilities = facilities.OrderByDescending(f => f.Name);
                    break;
                case "Category":
                    facilities = facilities.OrderBy(f => f.Category.Name);
                    break;
                case "category_desc":
                    facilities = facilities.OrderByDescending(f => f.Category.Name);
                    break;
                case "Address":
                    facilities = facilities.OrderBy(f => f.Address);
                    break;
                case "address_desc":
                    facilities = facilities.OrderByDescending(f => f.Address);
                    break;
                case "Website":
                    facilities = facilities.OrderBy(f => f.Website);
                    break;
                case "website_desc":
                    facilities = facilities.OrderByDescending(f => f.Website);
                    break;
                case "Phone":
                    facilities = facilities.OrderBy(f => f.Phone);
                    break;
                case "phone_desc":
                    facilities = facilities.OrderByDescending(f => f.Phone);
                    break;
                default:
                    facilities = facilities.OrderBy(f => f.Name);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(facilities.ToPagedList(pageNumber, pageSize));
        }

        // GET: Facilities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Facility facility = db.Facilities.Find(id);
            if (facility == null)
            {
                return HttpNotFound();
            }
            return View(facility);
        }

        // GET: Facilities/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: Facilities/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,CategoryId,Name,Address,Website,Phone")] Facility facility)
        {
            if (ModelState.IsValid)
            {
                db.Facilities.Add(facility);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", facility.CategoryId);
            return View(facility);
        }

        // GET: Facilities/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Facility facility = db.Facilities.Find(id);
            if (facility == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", facility.CategoryId);
            return View(facility);
        }

        // POST: Facilities/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,CategoryId,Name,Address,Website,Phone")] Facility facility)
        {
            if (ModelState.IsValid)
            {
                db.Entry(facility).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", facility.CategoryId);
            return View(facility);
        }

        // GET: Facilities/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Facility facility = db.Facilities.Find(id);
            if (facility == null)
            {
                return HttpNotFound();
            }
            return View(facility);
        }

        // POST: Facilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Facility facility = db.Facilities.Find(id);
            db.Facilities.Remove(facility);
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
