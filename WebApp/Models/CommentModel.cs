using System.Collections.Generic;

namespace WebApp.Models
{
    public class Comments {
        public string PhotoId { get; set; }
        public int CommentsId { get; set; }

        public virtual List<Comment> UserComments { get; set; }
    }

    public class Comment {
        public int CommentId { get; set; }
        public string UserComment { get; set; }
        
        public int CommentsId { get; set; }
        public virtual Comments Comments { get; set; }
    }
}