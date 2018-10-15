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

        [HttpPost]
        public ActionResult Search(string SearchInput)
        {
            return RedirectToAction("Index", "Gallery", new { id = SearchInput});
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