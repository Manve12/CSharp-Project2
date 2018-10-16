using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class GalleryController : Controller
    {
        // GET: Gallery
        public ActionResult Index()
        {
            
            dynamic searchId = _getID();
            
            if (searchId.Length == 6)
            {
                searchId = searchId[5];
                if (isNumeric(searchId))
                {
                    int pageId;
                    int.TryParse(searchId, out pageId);
                    return RenderGalleryByNumber(pageId);
                }
                return RenderGalleryByText(searchId, 1);
            }
            if (searchId.Length == 7) {
                //get query string
                string queryString = searchId[5];
                string queryPageNumber = searchId[6];
                int pageNumber = 1;
                int.TryParse(queryPageNumber, out pageNumber);
                return RenderGalleryByText(queryString, pageNumber);
            }
            else
            {
                searchId = 1;
                return RenderGalleryByNumber(searchId);
            }
        }

        public ActionResult RenderGalleryByText(string searchQuery, int pageNumber)
        {
            //depending on the page number work the next set available
            var listOfPageNumbers = new List<int>();

            var minimumPageNumber = 0;

            if ((pageNumber - 4) > 0)
            {
                minimumPageNumber = pageNumber - 4;
            }

            for (int i = minimumPageNumber; i < minimumPageNumber + 10; i++)
            {
                listOfPageNumbers.Add(i);
            }

            var photoList = GetPhotos("search/photos",pageNumber,searchQuery);

            //get photo comments and store in a new list
            var photoCommentList = new List<PhotoModel>();

            foreach (var photo in photoList)
            {
                photoCommentList.Add(
                    new PhotoModel
                    {
                        PhotoId = photo["id"].ToString(),
                        CommentAmount = _getCommentAmount(photo["id"].ToString())
                    });
            }

            //Send data to view

            ViewBag.PhotoList = photoList;
            ViewData["RandomImageUrl"] = GetRandomImageUrl();
            ViewBag.PageNumbers = listOfPageNumbers;
            ViewBag.CommentList = photoCommentList;

            return View();
        }

        public ActionResult RenderGalleryByNumber(int pageNumber)
        {
            //depending on the page number work the next set available
            var listOfPageNumbers = new List<int>();

            var minimumPageNumber = 0;

            if ((pageNumber - 4) > 0)
            {
                minimumPageNumber = pageNumber - 4;
            }

            for (int i = minimumPageNumber; i < minimumPageNumber + 10; i++)
            {
                listOfPageNumbers.Add(i);
            }

            //get all of the photos
            var photoList = GetPhotos("photos",pageNumber);

            //get photo comments and store in a new list
            var photoCommentList = new List<PhotoModel>();

            foreach (var photo in photoList)
            {
                photoCommentList.Add(
                    new PhotoModel
                    {
                        PhotoId = photo["id"].ToString(),
                        CommentAmount = _getCommentAmount(photo["id"].ToString())
                    });
            }

            //Send data to view

            ViewBag.PhotoList = photoList;
            ViewData["RandomImageUrl"] = GetRandomImageUrl();
            ViewBag.PageNumbers = listOfPageNumbers;
            ViewBag.CommentList = photoCommentList;

            return View();
        }

        private static dynamic _getID()
        {
            return (System.Web.HttpContext.Current.Request.Url.AbsoluteUri)
                                .Split('/');
        }

        private static dynamic _callApi(string urlSection, int pageNumber = 1, string searchQuery = null)
        {
            string url;
            string jsonData;
            dynamic obj;
            if (searchQuery == null)
            {
                url = "https://api.unsplash.com/" + urlSection + "?page=" + pageNumber + "&client_id=451f6851f8873903472e4ae17e9bac7afbe0a6ef4959cdca25b3ca195fdf22d3";
                jsonData = new WebClient().DownloadString(url);
                obj = JsonConvert.DeserializeObject(jsonData);
            }
            else
            {
                url = "https://api.unsplash.com/"+urlSection+"?query="+searchQuery+"&page="+pageNumber+"&client_id=451f6851f8873903472e4ae17e9bac7afbe0a6ef4959cdca25b3ca195fdf22d3";
                jsonData = new WebClient().DownloadString(url);

                JObject o = JObject.Parse(jsonData);
                var h = o["results"].ToString();

                obj = JsonConvert.DeserializeObject(h);            
            }

            return obj;
        }
        
        public static string GetRandomImageUrl()
        {
            dynamic jsonData = _callApi("photos/random");
            
            return jsonData["urls"]["full"].Value;
        }

        public static List<JToken> GetPhotos(string apiDirectory, int pageNumber, string searchQuery = null)
        {
            JArray listOfPhotos;
            if (searchQuery == null)
            {
                listOfPhotos = JArray.Parse(_callApi(apiDirectory, pageNumber).ToString());
            } else
            {
                dynamic t = _callApi(apiDirectory, pageNumber, searchQuery).ToString();
                listOfPhotos = JArray.Parse(t);
            }
            var photos = new List<JToken>();
            foreach (var i in listOfPhotos)
            {
                photos.Add(i);
            }
            return photos;
        }

        private static int _getCommentAmount(string photoId)
        {
            using (var db = new ApplicationDbContext())
            {
                var photo = db.Comments.FirstOrDefault(p => p.PhotoId == photoId);

                if (photo == null)
                {
                    return 0;
                }
                return photo.UserComments.Count();
                
            }
        }

        private static Boolean isNumeric(string value)
        {
            return value.All(Char.IsDigit);
        }
    }
}