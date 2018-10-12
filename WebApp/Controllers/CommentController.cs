using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

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
                
                using (var db = new ApplicationDbContext())
                {
                    //get comments    
                    Comments comments = db.Comments.FirstOrDefault(c => c.Name == id);

                    if (comments != null)
                    {
                        ViewBag.Comments = comments.UserComments;
                    } else
                    {
                        ViewBag.Comments = null;
                    }

                    dynamic photoData = GetPhoto(id);

                    dynamic userData = GetUser(photoData["user"]["username"].ToString());

                    if (userData == null)
                    {
                        return HttpNotFound();
                    }

                    ViewBag.PhotoData = photoData;
                    ViewBag.UserData = userData;
                }
                return View();
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }

        /// <summary>
        /// Add new comment after post
        /// </summary>
        /// <param name="commentInput">comment to add to db</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(string commentInput, string[] param = null)
        {
            var photoId = (System.Web.HttpContext.Current.Request.Url.AbsoluteUri).Split('/').Last();

            if (commentInput.Length != 0)
            {                
                using (var db = new ApplicationDbContext())
                {
                    //select image from database
                    Comments _comments = db.Comments.FirstOrDefault(c => c.Name == photoId);

                    if (_comments == null)
                    {
                        db.Comments.Add(new Comments { Name = photoId });
                        db.SaveChanges();
                        _comments = db.Comments.FirstOrDefault(c => c.Name == photoId);
                    }
                    
                    var comment = new Comment
                    {
                        UserComment = commentInput
                    };

                    _comments.UserComments.Add(comment);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index", new { id = photoId });
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