using System.Diagnostics;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class SettingsController : Controller
    {
        // GET: Settings
        public ActionResult Index()
        {
            if (Request.Cookies["ImageBlur"] == null)
            {
                var cookie = new HttpCookie("ImageBlur");
                cookie["blurGallery"] = "true";
                Response.Cookies.Add(cookie);
                cookie = Request.Cookies["ImageBlur"];
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(int? difference)
        {
            HttpCookie cookie;
            
            if (Request.Cookies["ImageBlur"] != null)
            {
                cookie = Request.Cookies["ImageBlur"];
                
                if (cookie["blurGallery"] == "true")
                {
                    cookie["blurGallery"] = "false";
                    Response.Cookies.Add(cookie);
                } else
                {
                    cookie["blurGallery"] = "true";
                    Response.Cookies.Add(cookie);
                }
            }
            return View();
        }
    }
}