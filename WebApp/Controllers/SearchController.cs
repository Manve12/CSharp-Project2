using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
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

        private ActionResult handleSearch(string searchInput)
        {
            return RedirectToAction("Index", "Gallery", new { id = searchInput });
        }

        private ActionResult handleSearch(string searchId, string queryId)
        {
            if (queryId != null)
            {
                return RedirectToAction("Index", "Gallery", new { id = searchId, queryId = queryId });
            }
            else
            {
                return RedirectToAction("Index", "Gallery", new { id = searchId });
            }
        }

        [HttpPost]
        public ActionResult Search(FormCollection form)
        {
            return handleSearch(form["searchInput"]);
        }

        [HttpPost]
        public ActionResult Close(FormCollection form)
        {
            var g = form["searchId"];
            var t = form["queryId"];
            return handleSearch(form["searchId"], form["queryId"]);
        }
    }
}