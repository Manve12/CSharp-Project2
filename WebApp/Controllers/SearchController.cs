using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public PartialViewResult Index()
        {
            return PartialView("_Index.html");
        }
    }
}