using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMT.Models.Model
{
    public class UserEntryPartial
    {
        public List<CheckBoxListItems> AvailableLeaveItems { get; set; }
        public List<CheckBoxListItems> SelectedLeaveItems { get; set; }
        public PostedItems PostedLeaveItem { get; set; }

        public string LeaveStatus { get; set; }
        //------------------------------------------------------------------------//

        public List<CheckBoxListItems> AvailableSecondShiftItems { get; set; }
        public List<CheckBoxListItems> SelectedSecondShiftItems { get; set; }
        public PostedItems PostedSecondShiftItem { get; set; }
        public string SecondShiftStatus { get; set; }
        //-----------------------------------------------------------------------//
        public List<CheckBoxListItems> AvailableWFHItems { get; set; }
        public List<CheckBoxListItems> SelectedWFHItems { get; set; }
        public PostedItems PostedWFHItem { get; set; }
        public string WFHStatus { get; set; }
        //-----------------------------------------------------------------------//
        public List<CheckBoxListItems> AvailableStandByItems { get; set; }
        public List<CheckBoxListItems> SelectedStandByItems { get; set; }
        public PostedItems PostedStandByItem { get; set; }
        public string StandByStatus { get; set; }
    }

    public class CheckBoxListItems
    {
        public string CheckBoxText { get; set; }
        public string CheckBoxValue { get; set; }
    }

    public class PostedItems
    {
        public DateTime[] ItemIds { get; set; }
    }
}