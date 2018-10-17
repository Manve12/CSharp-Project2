using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace WebApp.Models
{
    public class GalleryViewModel
    {
        public List<JToken> PhotoList { get; set; }
        public string RandomImageUrl { get; set; }
        public string PageNumberPrefix { get; set; }
        public List<int> PageNumbers { get; set; }
        public List<PhotoModel> CommentList { get; set; }
    }
}