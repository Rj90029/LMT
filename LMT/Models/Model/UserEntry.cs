using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMT.Models.Model
{
    public class UserEntry
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public List<string> Weekend { get; set; }
        public string WeekendSelected { get; set; }
        public UserEntryPartial PartialModel { get; set; }
    }
}