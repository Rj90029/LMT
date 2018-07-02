using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMT.Models.Model
{
    public class ResetPassword
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required!")]
        [Display(Name = "Employee Name")]
        public string UserName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Password is required!")]
        [Display(Name = "Last Password")]
        public string LastPassword { get; set; }

        public List<string> UserList { get; set; }
        [Display(Name = "New Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required!")]
        public string Password { get; set; }
        [Display(Name ="Confirm Password")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="Confirm Password is required!")]
        [Compare("Password",ErrorMessage ="Password and Confirm Password must match!")]
        public string ConfirmPassword { get; set; }
    }
}