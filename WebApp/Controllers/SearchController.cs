using System.Diagnostics;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public PartialViewResult Index()
        {
            return PartialView("_Index");
        }

        [HttpPost]
        public ActionResult Search()
        {
            return RedirectToAction("Index", "Gallery");
        }

        [HttpPost]
        public ActionResult Close(string returnUrlId)
        {
            Debug.WriteLine(returnUrlId);
            int id;

            if (!int.TryParse(returnUrlId, out id)) {
                id = 1;
            }

            Debug.WriteLine(id);
            return RedirectToAction("Index","Gallery", new { id = id });

        }
    }
}