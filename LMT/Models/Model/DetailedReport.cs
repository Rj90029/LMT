using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMT.Models.Model
{
    public class DetailedReport
    {
        public string EmpID { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required!")]
        [Display(Name = "Employee Name")]
        public string UserName { get; set; }
        public List<string> UserList { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Request type is required!")]
        [Display(Name = "Request Type")]
        public string RequestType { get; set; }
        public List<string> TypeList { get; set; }
        [Display(Name ="Status")]
        public string StatusSelected { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select a date")]
        [Display(Name = "From date")]
        public string FromDate { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select a date!")]
        [Display(Name = "To date")]
        public string ToDate { get; set; }
        public string RequestDate { get; set; }
    }
}