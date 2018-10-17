using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
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

                //instantiate new model
                CommentViewModel commentViewModel = new CommentViewModel();

                using (var db = new ApplicationDbContext())
                {
                    
                    //get comments    
                    Comments comments = db.Comments.FirstOrDefault(c => c.PhotoId == id);

                    if (comments != null)
                    {
                        commentViewModel.Comments = comments.UserComments;
                    } else
                    {
                        commentViewModel.Comments = null;
                    }

                    dynamic photoData = GetPhoto(id);

                    dynamic userData = GetUser(photoData["user"]["username"].ToString());

                    if (userData == null)
                    {
                        return HttpNotFound();
                    }

                    commentViewModel.PhotoData = photoData;
                    commentViewModel.UserData = userData;
                }
                return View(commentViewModel);
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

            var username = User.Identity.Name;

            if (commentInput.Length != 0)
            {                
                using (var db = new ApplicationDbContext())
                {
                    //select image from database
                    Comments _comments = db.Comments.FirstOrDefault(c => c.PhotoId == photoId);

                    if (_comments == null)
                    {
                        db.Comments.Add(new Comments { PhotoId = photoId, UserComments = new List<Comment>() });
                        db.SaveChanges();
                        _comments = db.Comments.FirstOrDefault(c => c.PhotoId == photoId);

                    }

                    var comment = new Comment
                    {
                        UserComment = commentInput,
                        UserName = username
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