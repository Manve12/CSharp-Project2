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
        /// <summary>
        /// Render initial gallery
        /// </summary>
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
                    return RenderGallery(pageId);
                }
                return RenderGallery(1, searchId);
            }
            if (searchId.Length == 7) {
                //get query string
                string queryString = searchId[5];
                string queryPageNumber = searchId[6];
                int pageNumber = 1;
                bool pageNumberParsedToInt = int.TryParse(queryPageNumber, out pageNumber);
                if (!pageNumberParsedToInt)
                {
                    return HttpNotFound();
                }
                return RenderGallery(pageNumber, queryString);
            }
            else
            {
                searchId = 1;
                return RenderGallery(searchId);
            }
        }
        
        /// <summary>
        /// Render gallery depending on the page number or page number and search query
        /// </summary>
        /// <param name="PageNumber">Page number to display</param>
        /// <param name="SearchQuery">Search API for specific images</param>
        public ActionResult RenderGallery(int PageNumber, string SearchQuery = null)
        {
            List<JToken> photoList;

            if (SearchQuery != null)
            {
                photoList = GetPhotos("search/photos", PageNumber, SearchQuery);
            } else
            {
                photoList = GetPhotos("photos", PageNumber);
            }

            //Send data to view
            GalleryViewModel model = new GalleryViewModel();

            model.PhotoList = photoList;
            model.RandomImageUrl = GetRandomImageUrl();
            model.PageNumbers = _getListOfPageNumbers(PageNumber);
            model.CommentList = _getPhotoCommentList(photoList);

            if (SearchQuery != null)
            {
                model.PageNumberPrefix = SearchQuery;
            }

            return View(model);
        }

        /// <summary>
        /// Returns a list form of the URL
        /// </summary>
        private static dynamic _getID()
        {
            return (System.Web.HttpContext.Current.Request.Url.AbsoluteUri)
                                .Split('/');
        }

        /// <summary>
        /// Call api for retrieve data
        /// </summary>
        /// <param name="urlSection">Url section for api to direct too</param>
        /// <param name="pageNumber">Page number to retrieve</param>
        /// <param name="searchQuery">Search query if required by url section</param>
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
        
        /// <summary>
        /// Gets random image URL from API
        /// </summary>
        public static string GetRandomImageUrl()
        {
            dynamic jsonData = _callApi("photos/random");
            
            return jsonData["urls"]["full"].Value;
        }

        /// <summary>
        /// Get list of photos
        /// </summary>
        /// <param name="apiDirectory">API directory (URL SECTION) to search by</param>
        /// <param name="pageNumber">Page number to search by</param>
        /// <param name="searchQuery">Search query if required by api directory</param>
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

        /// <summary>
        /// Get the number of comments for the image
        /// </summary>
        /// <param name="photoId">Photo id to search database by</param>
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

        /// <summary>
        /// Get comments for a photo
        /// </summary>
        /// <param name="PhotoList">Photo id to search database by</param>
        private List<PhotoModel> _getPhotoCommentList(List<JToken> PhotoList)
        {
            var photoCommentList = new List<PhotoModel>();

            foreach (var photo in PhotoList)
            {
                photoCommentList.Add(
                    new PhotoModel
                    {
                        PhotoId = photo["id"].ToString(),
                        CommentAmount = _getCommentAmount(photo["id"].ToString())
                    });
            }

            return photoCommentList;
        }

        /// <summary>
        /// Get list of page numbers
        /// </summary>
        /// <param name="pageNumber">Page number to calculate from</param>
        private List<int> _getListOfPageNumbers(int pageNumber)
        {
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
            return listOfPageNumbers;
        }

        /// <summary>
        /// Check if a value is numeric
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <returns></returns>
        private static Boolean isNumeric(string value)
        {
            return value.All(Char.IsDigit);
        }
    }
}