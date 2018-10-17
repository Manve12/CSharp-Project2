using System.Collections.Generic;

namespace WebApp.Models
{
    public class CommentViewModel
    {
        public List<Comment> Comments { get; set; }
        public dynamic PhotoData { get; set; }
        public dynamic UserData { get; set; }
    }
}