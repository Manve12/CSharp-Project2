using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Search
    {
        public int total { get; set; }
        public int total_pages { get; set; }
        public List<JArray> results { get; set;}
    }
}