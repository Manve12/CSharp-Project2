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
using WebApp.Models;

namespace WebApp.Controllers
{
    public class GalleryController : Controller
    {
        // GET: Gallery
        public ActionResult Index()
        {
            ViewBag.PhotoList = GetPhotos();
           
            ViewData["RandomImageUrl"] = GetRandomImageUrl();

            return View();
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

        public static List<JToken> GetPhotos(int pageNumber = 3)
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