using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMT.Models.Model
{
    public class Login
    {
        [Required(AllowEmptyStrings =false,ErrorMessage ="Username is required!")]
        [Display(Name ="Employee Name")]
        public string UserName { get; set; }
        public List<string> UserList { get; set; }
        [Display(Name ="Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required!")]
        public string Password { get; set; }
        public string Role { get; set; }

        public string Error { get; set; }

    }
}