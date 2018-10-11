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
    public class GalleryController : Controller
    {
        // GET: Gallery
        public ActionResult Index()
        {
            var pageNumber = _getID();

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

            //Send data to view
            ViewBag.PhotoList = GetPhotos(pageNumber);
            ViewData["RandomImageUrl"] = GetRandomImageUrl();
            ViewBag.PageNumbers = listOfPageNumbers;
            
            return View();
        }

        private static int _getID()
        {
            var lastElement = (System.Web.HttpContext.Current.Request.Url.AbsoluteUri).Split('/').Last();
            int id;
            bool isNumeric;
            isNumeric = int.TryParse(lastElement,out id);
            if (isNumeric)
            {
                return id;
            }
            return 1;
        }

        private static dynamic _callApi(string urlSection, int pageNumber = 1)
        {
            var url = "https://api.unsplash.com/" + urlSection + "?page=" + pageNumber + "&client_id=451f6851f8873903472e4ae17e9bac7afbe0a6ef4959cdca25b3ca195fdf22d3";
            var jsonData = new WebClient().DownloadString(url);
            dynamic obj = JsonConvert.DeserializeObject(jsonData);
            
            return obj;
        }
        
        public static string GetRandomImageUrl()
        {
            dynamic jsonData = _callApi("photos/random");
            
            return jsonData["urls"]["full"].Value;
        }

        public static List<JToken> GetPhotos(int pageNumber)
        {
            JArray listOfPhotos = JArray.Parse(_callApi("photos",pageNumber).ToString());
            var photos = new List<JToken>();
            foreach (var i in listOfPhotos)
            {
                photos.Add(i);
            }
            return photos;
        }
    }
}