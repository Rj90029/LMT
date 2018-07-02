using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMT.Models.Model
{
    public class EmployeeMaster
    {
        [Display(Name="Employee ID")]
        public string EmpID { get; set; }
        
        public string ATTUID { get; set; }
        public string Name { get; set; }
        public string Email{ get; set; }
        public string Phone { get; set; }
    }
}