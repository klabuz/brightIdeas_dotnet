using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ideas.Models {
 public abstract class BaseEntity {

       public DateTime created_at {get; set;}
       public DateTime updated_at {get; set;}

 }
	public class User : BaseEntity{
            [Key]
		public int userId {get; set;}   
		public string name {get; set;}	
		public string alias {get; set;}	
        public string email {get; set;}
        public string password {get; set;}
        
        [InverseProperty("creator")]
        public List<Post> created { get;set; }

        public List<Like> liked { get;set; }
        
        public User(){
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
            created = new List<Post>();
            liked = new List<Like>();
        }

	}
}