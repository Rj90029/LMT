using LMT.Models.Model;
using LMT.Services;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Globalization;
using System.Web.Mvc;
using System.Security.Permissions;
using System.Web;
using System.Drawing;
using System.IO;
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




        public ActionResult UserEntry(string user, string date, string status)
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
        public ActionResult UserEntry(System.Web.Mvc.FormCollection fc, HttpPostedFileBase[] FileUpload,HttpPostedFileBase[] FileUpload1)
        {
            UserEntryServices userEntryServices = new UserEntryServices();
            string status = "";
            try
            {
                if (FileUpload.Length>0 && FileUpload[0]!=null)
                {
                    var fileName = new string[FileUpload.Length];
                    var ext = new string[FileUpload.Length];
                    var name = new string[FileUpload.Length];
                    var myFile = new string[FileUpload.Length];
                    var path = new string[FileUpload.Length];
                    for(int i = 0; i < FileUpload.Length; i++)
                    {
                        fileName[i] = Path.GetFileName(FileUpload[i].FileName);
                        ext[i] = Path.GetExtension(FileUpload[0].FileName); 
                        name[i] = fc["Name"].Trim() + "_" + fc["WeekendSelected"].Replace("/", "") + "_SS"+(i+1);
                        myFile[i] = name[i] + ext[i];
                        path[i] = Path.Combine(Server.MapPath("~/Images/"), myFile[i]);
                        FileUpload[i].SaveAs(path[i]);
                    }
                }
                if (FileUpload1.Length > 0 && FileUpload1[0] != null)
                {
                    var fileName = new string[FileUpload1.Length];
                    var ext = new string[FileUpload1.Length];
                    var name = new string[FileUpload1.Length];
                    var myFile = new string[FileUpload1.Length];
                    var path = new string[FileUpload1.Length];
                    for (int i = 0; i < FileUpload1.Length; i++)
                    {
                        fileName[i] = Path.GetFileName(FileUpload1[i].FileName);
                        ext[i] = Path.GetExtension(FileUpload1[0].FileName);
                        name[i] = fc["Name"].Trim() + "_" + fc["WeekendSelected"].Replace("/", "") + "_SB" + (i + 1);
                        myFile[i] = name[i] + ext[i];
                        path[i] = Path.Combine(Server.MapPath("~/Images/"), myFile[i]);
                        FileUpload1[i].SaveAs(path[i]);
                    }
                }

                OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + Server.MapPath("~/App_Data/Tracker.accdb"));
                status = userEntryServices.InsertLeaves(fc, con);
                return RedirectToAction("UserEntry", new { user = fc["Name"].Trim(), date = fc["WeekendSelected"], status = status });

            }
            catch(Exception ex)
            {
                status = "Files not uploaded successfully.";
            }

            return RedirectToAction("UserEntry", new { user = fc["Name"].Trim(), date = fc["WeekendSelected"], status = status });

        }
        [HttpPost]
        public JsonResult ViewImagesSS(UserEntry model)
        {
            var Avdate = model.WeekendSelected.Split('/');
            var Seldate = DateTime.Parse(Avdate[1] + '/' + Avdate[0] + '/' + Avdate[2]);

            while (Seldate.DayOfWeek != DayOfWeek.Friday)
            {
                Seldate =Seldate.AddDays(1);
            }

            string date = Seldate.Day.ToString() + Seldate.Month.ToString() + Seldate.Year.ToString();
            int len = Directory.GetFiles(Server.MapPath("~/Images"), model.Name + "_" + date + "_SS*").Length;
            string[] files = new string[len];
            string[] pathArray = new string[len];
            files = Directory.GetFiles(Server.MapPath("~/Images"), model.Name + "_" + date + "_SS*");
            for(int i = 0; i < len; i++)
            {
                pathArray[i] = "../Images/"+ Path.GetFileName(files[i]);
            }
            return Json(new {
                path = pathArray
            });

        }
        [HttpPost]
        public JsonResult ViewImagesSB(UserEntry model)
        {
            var Avdate = model.WeekendSelected.Split('/');
            var Seldate = DateTime.Parse(Avdate[1] + '/' + Avdate[0] + '/' + Avdate[2]);

            while (Seldate.DayOfWeek != DayOfWeek.Friday)
            {
                Seldate = Seldate.AddDays(1);
            }

            string date = Seldate.Day.ToString() + Seldate.Month.ToString() + Seldate.Year.ToString();
            int len = Directory.GetFiles(Server.MapPath("~/Images"), model.Name + "_" + date + "_SB*").Length;
            string[] files = new string[len];
            string[] pathArray = new string[len];
            files = Directory.GetFiles(Server.MapPath("~/Images"), model.Name + "_" + date + "_SB*");
            for (int i = 0; i < len; i++)
            {
                pathArray[i] = "../Images/" + Path.GetFileName(files[i]);
            }
            return Json(new
            {
                path = pathArray
            });

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
            return RedirectToAction("UserEntry", new { user = user.Trim(), date = date, status = status });
        }

    }
}