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

        /// <summary>
        /// Redirect to new action with search parameters
        /// </summary>
        /// <param name="search">Search view model</param>
        [HttpPost]
        public ActionResult Search(SearchViewModel search)
        {
            return RedirectToAction("Index", "Gallery", new { id = search.SearchInput });
        }

        /// <summary>
        /// Redirect to new action with previous search parameters
        /// </summary>
        /// <param name="search">Search view model</param>
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