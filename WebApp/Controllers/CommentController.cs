using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class CommentController : Controller
    {
        // GET: Comment
        [HttpGet]
        public ActionResult Index(string id)
        {
            if (id.Length == 0)
            {
                return HttpNotFound();
            }

            try
            {
                dynamic photoData = GetPhoto(id);

                dynamic userData = GetUser(photoData["user"]["username"].ToString());

                if (userData == null)
                {
                    return HttpNotFound();
                }

                ViewBag.PhotoData = photoData;
                ViewBag.UserData = userData;

                return View();
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult Index(string commentInput, string[] param = null)
        {

            return HttpNotFound();
        }

        private static JToken GetPhoto(string id)
        {
            try
            {
                var url = "https://api.unsplash.com/photos/" + id + "?client_id=451f6851f8873903472e4ae17e9bac7afbe0a6ef4959cdca25b3ca195fdf22d3";
                var jsonData = new WebClient().DownloadString(url);
                dynamic obj = JsonConvert.DeserializeObject(jsonData);
                return obj;
            } catch (Exception)
            {
                return null;
            }
        }

        private static JToken GetUser(string username)
        {
            var url = "https://api.unsplash.com/users/" + username + "?client_id=451f6851f8873903472e4ae17e9bac7afbe0a6ef4959cdca25b3ca195fdf22d3";
            var jsonData = new WebClient().DownloadString(url);
            dynamic obj = JsonConvert.DeserializeObject(jsonData);
            return obj;
        }
    }
}