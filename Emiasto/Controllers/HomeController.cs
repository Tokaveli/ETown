using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

using Emiasto.DAL;
using Emiasto.ViewModels;

namespace Emiasto.Controllers
{
    public class HomeController : Controller
    {
        private CityContext db = new CityContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            IQueryable<ReleaseDateGroup> data =
                from review in db.Reviews group review by review.Release into dateGroup
                select new ReleaseDateGroup()
                {
                    ReleaseDate = dateGroup.Key,
                    ReviewCount = dateGroup.Count()
                };
            return View(data.ToList());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult SendMail(string receiver)
        {
            IQueryable<ReleaseDateGroup> data =
                from review in db.Reviews
                group review by review.Release into dateGroup
                select new ReleaseDateGroup()
                {
                    ReleaseDate = dateGroup.Key,
                    ReviewCount = dateGroup.Count()
                };

            string body = "Reviews Statistics\n\nRelease Date\t\tReviews\n";

            foreach (var item in data)
            {
                body += item.ReleaseDate.Value.ToString("dd.MM.yyyy") + "\t\t";
                body += item.ReviewCount + "\n";
            }

            var sender = ConfigurationManager.AppSettings["smtpSender"];
            var host = ConfigurationManager.AppSettings["smtpHost"];
            var password = ConfigurationManager.AppSettings["smtpPassword"];

            var mail = new MailMessage(sender, receiver)
            {
                Subject = "Reviews Statistics",
                Body = body
            };

            var client = new SmtpClient
            {
                Host = host,
                Credentials = new NetworkCredential(sender, password),
                EnableSsl = true
            };
            
            client.Send(mail);

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}