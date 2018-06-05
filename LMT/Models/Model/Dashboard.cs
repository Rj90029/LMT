using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMT.Models.Model
{
    public class Dashboard
    {
        public string Name { get; set; }
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }
    }
}