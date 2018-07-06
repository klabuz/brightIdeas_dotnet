using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ideas.Models
{
    public  class Post : BaseEntity {

        public int postId { get;set; }
        public string content { get;set; }

        public bool currentUser { get;set; }

        [ForeignKey("creator")]
        public int creatorId {get;set;}
        public User creator {get;set;}

        public List<Like> likes {get;set;}

        public Post(){
            likes = new List<Like>();
        }
    }
}