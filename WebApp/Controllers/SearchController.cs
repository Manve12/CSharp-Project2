using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Web.Mvc;
using WebApp.Models;

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
        public ActionResult Search(SearchViewModel search)
        {
            return RedirectToAction("Index", "Gallery", new { id = search.SearchInput });
        }

        [HttpPost]
        public ActionResult Close(SearchViewModel search)
        {
            if (search.QueryId != null)
            {
                return RedirectToAction("Index", "Gallery", new { id = search.SearchId, queryId = search.QueryId });
            }
            else
            {
                return RedirectToAction("Index", "Gallery", new { id = search.SearchId });
            }
        }
    }
}