using System;
using System.ComponentModel.DataAnnotations;

namespace ideas.Models
{
    public class Register : BaseEntity{

        [Required]
        [MinLength(2, ErrorMessage="Your name must contain at least 2 characters.")]
        [Display(Name="name")]
        public string name { get;set; }

        [Required]
        [MinLength(2, ErrorMessage="Your alias name must contain at least 2 characters.")]
        [RegularExpression("^[a-zA-Z]+$")]
        [Display(Name="alias")]
        public string alias { get;set; }

        [Required]
        [EmailAddress]
        [Display(Name="Email Address")]
        public string email { get;set; }

        [Required]
        [MinLength(8, ErrorMessage="Your password must contain at least 8 characters.")]
        [Compare("cw_password", ErrorMessage="Passwords don't match.")]
        [DataType(DataType.Password)]
        [Display(Name="Password")]
        public string password { get;set; }

        [Required]
        [Compare("password", ErrorMessage="Passwords don't match.")]
        [DataType(DataType.Password)]
        [Display(Name="Confirm Password")]
        public string cw_password { get;set; }


    }

        public class Login : BaseEntity{

        [Required]
        [EmailAddress]
        
        public string email { get;set; }

        [Required]
        [MinLength(8, ErrorMessage="Your password must contain at least 8 characters.")]
        
        [DataType(DataType.Password)]
        
        public string password { get;set; }
    }

    public class UserViewModel : BaseEntity{

        public Register Reg { get;set; }
        public Login Log { get;set; }
    }
    
}