using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace LMT.Models.Model
{
    public class Employee
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Your EmpID"), StringLength(6)]
        public string EmpId { get; set; }

        [Required, StringLength(10), RegularExpression("[0-9]{10}", ErrorMessage = "Please enter 10 digit phone no")]
        public string Phone { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage ="Enter the Name")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter the Attuid"), StringLength(6)]
        public string Attuid { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter the DOJ"), DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public string DOJ { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public string DOR { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "please enter email address")]
        public string Email { get; set; }

        [Display(Name ="Supporting Application")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "please choose Supporting Application")]
        public List<string> SuppApp { get; set; }

        public List<string> Applications { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "please choose Competency")]
        public List<string> Competency { get; set; }
        public List<string> CompetencyList { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "please select a designation")]
        public string Designation { get; set; }
        public List<string> DesignationList { get; set; }
       
    }
}