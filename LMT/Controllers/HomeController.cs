using LMT.Models.Model;
using LMT.Services;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMT.Controllers
{
    public class HomeController : SecureController
    {
        public ActionResult LeaveStatus()
        {

            LeaveStatusServices dashboardService = new LeaveStatusServices();
            List<LeaveStatus> details = dashboardService.GetDetails();
            return View(details);
        }

        public ActionResult Dashboard()
        {
            string name = Session["Name"].ToString();
            DashboardService dashboardService = new DashboardService();
            List<Dashboard> details = dashboardService.GetDetails(name);
            return View(details);
        }
        public ActionResult EmployeeDetails(string user)
        {

            EmployeeService EmployeeService = new EmployeeService();
            Employee details = EmployeeService.GetDetails(user.Trim());
            return View(details);
        }


        public ActionResult UpdateEmployee(string name)
        {
            EmployeeService EmployeeService = new EmployeeService();
            Employee employee = new Employee();

            employee = EmployeeService.GetDetails(name);
            ViewBag.CompetencyList = new MultiSelectList(employee.CompetencyList);
            ViewBag.DesigList = new SelectList(employee.DesignationList);
            ViewBag.AppListX = new MultiSelectList(employee.Applications);
            return View(employee);

        }

        [HttpPost]
        public ActionResult UpdateEmployee(Employee e, string target)
        {
            EmployeeService EmployeeService = new EmployeeService();
            string result;
            if (target == "Update")
                result = EmployeeService.UpdateDetails(e);
            else
                result = EmployeeService.AddDetails(e);
            ViewBag.result = result;
            e.DesignationList = EmployeeService.PopulateDesigLists();
            e.Applications = EmployeeService.PopulateAppsLists();
            e.CompetencyList = EmployeeService.PopulateCompetencyLists();

            ViewBag.AppListX = new MultiSelectList(e.Applications);
            ViewBag.CompetencyList = new MultiSelectList(e.CompetencyList);
            ViewBag.DesigList = new SelectList(e.DesignationList);

            return View(e);
        }


      

        public ActionResult UserEntry(string user, string date,string status)
        {
            UserEntryServices userEntryServices = new UserEntryServices();
            UserEntry userEntry = new UserEntry();
            userEntry.Weekend = userEntryServices.GetWeekends();
            userEntry.Name = user.Trim();
            ViewBag.Weekend = new SelectList(userEntry.Weekend);
            DateTime dt;
            if (date == "" || date == null)
            {
                date = DateTime.Today.ToString("d/M/yyyy");
            }
            dt = DateTime.ParseExact(date, "d/M/yyyy", CultureInfo.InvariantCulture);
            userEntry.WeekendSelected = userEntryServices.GetSelectedWeekend(dt);
            DateTime Weekend = DateTime.ParseExact(userEntry.WeekendSelected, "d/M/yyyy", CultureInfo.InvariantCulture);
            userEntry.PartialModel = new Models.Model.UserEntryPartial();
            userEntry.PartialModel.AvailableLeaveItems = userEntryServices.GetAvailableLeaveDates(Weekend);
            userEntry.PartialModel.AvailableSecondShiftItems = userEntryServices.GetAvailableSecondShiftDates(Weekend);
            userEntry.PartialModel.AvailableStandByItems = userEntryServices.GetAvailableStandByDates(Weekend);
            userEntry.PartialModel.AvailableWFHItems = userEntryServices.GetAvailableWFHDates(Weekend);

            userEntry.PartialModel.SelectedLeaveItems = userEntryServices.GetSelectedLeaveDates(userEntry.Name, Weekend);
            userEntry.PartialModel.SelectedSecondShiftItems = userEntryServices.GetSelectedSecondShiftDates(userEntry.Name, Weekend);
            userEntry.PartialModel.SelectedStandByItems = userEntryServices.GetSelectedStandByDates(userEntry.Name, Weekend);
            userEntry.PartialModel.SelectedWFHItems = userEntryServices.GetSelectedWFHDates(userEntry.Name, Weekend);

            userEntry.PartialModel.LeaveStatus = userEntryServices.GetLeaveStatus(userEntry.Name, Weekend);
            userEntry.PartialModel.StandByStatus = userEntryServices.GetStandByStatus(userEntry.Name, Weekend);
            userEntry.PartialModel.WFHStatus = userEntryServices.GetWFHStatus(userEntry.Name, Weekend);
            userEntry.PartialModel.SecondShiftStatus = userEntryServices.GetSecondShiftStatus(userEntry.Name, Weekend);

            // SelectList status = new SelectList();
            var list = new List<SelectListItem>{
                new SelectListItem {Text = "PENDING", Value = "PENDING" },
                new SelectListItem {Text = "REJECTED", Value = "REJECTED" },
                new SelectListItem {Text = "APPROVED", Value = "APPROVED" }
            };

            ViewBag.Status = list;
            ViewBag.Result = status;
            return View(userEntry);
        }
        [HttpPost]
        public ActionResult UserEntry(FormCollection fc)
        {
            UserEntryServices userEntryServices = new UserEntryServices();
            OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + Server.MapPath("~/App_Data/Tracker.accdb"));
            string status = userEntryServices.InsertLeaves(fc, con);
            return RedirectToAction("UserEntry", new { user = fc["Name"].Trim(), date = fc["WeekendSelected"],status= status });
        }
        public ActionResult PartialUserEntry(string user, string date)
        {
            UserEntryServices userEntryServices = new UserEntryServices();
            UserEntryPartial userEntryPartial = new UserEntryPartial();

            DateTime dt = DateTime.ParseExact(date, "d/M/yyyy", CultureInfo.InvariantCulture);

            userEntryPartial.AvailableLeaveItems = userEntryServices.GetAvailableLeaveDates(dt);
            userEntryPartial.AvailableSecondShiftItems = userEntryServices.GetAvailableSecondShiftDates(dt);
            userEntryPartial.AvailableStandByItems = userEntryServices.GetAvailableStandByDates(dt);
            userEntryPartial.AvailableWFHItems = userEntryServices.GetAvailableWFHDates(dt);

            userEntryPartial.SelectedLeaveItems = userEntryServices.GetSelectedLeaveDates(user, dt);
            userEntryPartial.SelectedSecondShiftItems = userEntryServices.GetSelectedSecondShiftDates(user, dt);
            userEntryPartial.SelectedStandByItems = userEntryServices.GetSelectedStandByDates(user, dt);
            userEntryPartial.SelectedWFHItems = userEntryServices.GetSelectedWFHDates(user, dt);

            userEntryPartial.LeaveStatus = userEntryServices.GetLeaveStatus(user, dt);
            userEntryPartial.StandByStatus = userEntryServices.GetStandByStatus(user, dt);
            userEntryPartial.WFHStatus = userEntryServices.GetWFHStatus(user, dt);
            userEntryPartial.SecondShiftStatus = userEntryServices.GetSecondShiftStatus(user, dt);

            // SelectList status = new SelectList();
            var list = new List<SelectListItem>{
                new SelectListItem {Text = "PENDING", Value = "PENDING" },
                new SelectListItem {Text = "REJECTED", Value = "REJECTED" },
                new SelectListItem {Text = "APPROVED", Value = "APPROVED" }
            };

            ViewBag.Status = list;

            return PartialView("PartialUserEntry", userEntryPartial);
        }
        public ActionResult ApproveReject(string user, string date, string type, string act)
        {
            UserEntryServices userEntryServices = new UserEntryServices();
            string status = userEntryServices.ApproveReject(user.Trim(), date, type, act);
            System.Threading.Thread.Sleep(2000);
            return RedirectToAction("UserEntry", new { user = user.Trim(),date = date,status = status});
        }
       
    }
}