using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMT.Models.Model
{
    public class LeaveStatus
    {
        public string Name { get; set; }
       [Display(Name ="Request Date")]
        public string VacationDate { get; set; }
        public string Status { get; set; }
        [Display(Name ="Request Type")]
        public string VacationType { get; set; }
    }
}