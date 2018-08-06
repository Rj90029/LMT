using LMT.Models.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace LMT.Services
{
    public class UserEntryServices
    {
        EmployeeEntity context = new EmployeeEntity();
        EmployeeEntityTableAdapters.USERENTRYTableAdapter userentryTableAdapter = new EmployeeEntityTableAdapters.USERENTRYTableAdapter();
        EmployeeEntityTableAdapters.EMPLOYEETableAdapter employeeTableAdapter = new EmployeeEntityTableAdapters.EMPLOYEETableAdapter();
        
        UserEntryPartial userEntryPartial = new UserEntryPartial();
        CheckBoxListItems checkBoxListItems;
      
        public List<string> GetWeekends()
        {
            List<string> fridayList = new List<string>();
            DateTime startDate = new DateTime(DateTime.Today.Year, 1, 1);

            while (startDate.Year <= DateTime.Today.AddYears(1).Year)
            {
                if (startDate.DayOfWeek == DayOfWeek.Friday)
                {
                    fridayList.Add(startDate.ToString("d/M/yyyy"));
                    startDate = startDate.AddDays(7);
                }
                else
                {
                    startDate = startDate.AddDays(1);
                }

            }

            return fridayList;
        }

        public string GetSelectedWeekend(DateTime date)
        {
            DateTime selectedWeeked = new DateTime();
            DateTime today;
            if(date == null)
            {
                today = DateTime.Today;
            }
            else
            {
                today = date;
            }

            int daysUntilFriday = (((int)DayOfWeek.Friday - (int)today.DayOfWeek + 7) % 7);
            selectedWeeked = today.AddDays(daysUntilFriday);
            return selectedWeeked.ToString("d/M/yyyy");

        }

        public List<CheckBoxListItems> GetSelectedLeaveDates(string user, DateTime date)
        {
            //List<KeyValuePair<DateTime, string>> vacationDates = new List<KeyValuePair<DateTime, string>>();
            userentryTableAdapter.Fill(context.USERENTRY);
            employeeTableAdapter.Fill(context.EMPLOYEE);

            var res = from r in context.USERENTRY
                      join e in context.EMPLOYEE
                            on r.EMP_ID equals e.EMP_ID
                      where e.EMPNAME == user && r.WEEKENDDATE == date
                      select new { VacationType = r.VACATIONTYPE, VacationDate = r.VACATIONDATE };
            userEntryPartial.SelectedLeaveItems = new List<CheckBoxListItems>();

            foreach (var r in res)
            {
                if (r.VacationType == "LEAVE")
                {
                    checkBoxListItems = new CheckBoxListItems();
                    checkBoxListItems.CheckBoxText = r.VacationDate.ToString("d/M/yyyy");
                    checkBoxListItems.CheckBoxValue = r.VacationDate.ToString("d/M/yyyy");
                    userEntryPartial.SelectedLeaveItems.Add(checkBoxListItems);
                }

            }

            return userEntryPartial.SelectedLeaveItems;
        }

        public List<CheckBoxListItems> GetSelectedWFHDates(string user, DateTime date)
        {
            userentryTableAdapter.Fill(context.USERENTRY);
            employeeTableAdapter.Fill(context.EMPLOYEE);

            var res = from r in context.USERENTRY
                      join e in context.EMPLOYEE
                            on r.EMP_ID equals e.EMP_ID
                      where e.EMPNAME == user && r.WEEKENDDATE == date
                      select new { VacationType = r.VACATIONTYPE, VacationDate = r.VACATIONDATE };

            userEntryPartial.SelectedWFHItems = new List<CheckBoxListItems>();

            foreach (var r in res)
            {

                if (r.VacationType == "WFH")
                {
                    checkBoxListItems = new CheckBoxListItems();
                    checkBoxListItems.CheckBoxText = r.VacationDate.ToString("d/M/yyyy");
                    checkBoxListItems.CheckBoxValue = r.VacationDate.ToString("d/M/yyyy");
                    userEntryPartial.SelectedWFHItems.Add(checkBoxListItems);
                }
            }

            return userEntryPartial.SelectedWFHItems;
        }
       
        public List<CheckBoxListItems> GetSelectedSecondShiftDates(string user, DateTime date)
        {
            //List<KeyValuePair<DateTime, string>> vacationDates = new List<KeyValuePair<DateTime, string>>();
            userentryTableAdapter.Fill(context.USERENTRY);
            employeeTableAdapter.Fill(context.EMPLOYEE);

            var res = from r in context.USERENTRY
                      join e in context.EMPLOYEE
                            on r.EMP_ID equals e.EMP_ID
                      where e.EMPNAME == user && r.WEEKENDDATE == date
                      select new { VacationType = r.VACATIONTYPE, VacationDate = r.VACATIONDATE };

            userEntryPartial.SelectedSecondShiftItems = new List<CheckBoxListItems>();

            foreach (var r in res)
            {

                if (r.VacationType == "SECONDSHIFT")
                {
                    checkBoxListItems = new CheckBoxListItems();
                    checkBoxListItems.CheckBoxText = r.VacationDate.ToString("d/M/yyyy");
                    checkBoxListItems.CheckBoxValue = r.VacationDate.ToString("d/M/yyyy");
                    userEntryPartial.SelectedSecondShiftItems.Add(checkBoxListItems);
                }
            }

            return userEntryPartial.SelectedSecondShiftItems;
        }
   
        public List<CheckBoxListItems> GetSelectedStandByDates(string user, DateTime date)
        {
            //List<KeyValuePair<DateTime, string>> vacationDates = new List<KeyValuePair<DateTime, string>>();
            userentryTableAdapter.Fill(context.USERENTRY);
            employeeTableAdapter.Fill(context.EMPLOYEE);

            var res = from r in context.USERENTRY
                      join e in context.EMPLOYEE
                            on r.EMP_ID equals e.EMP_ID
                      where e.EMPNAME == user && r.WEEKENDDATE == date
                      select new { VacationType = r.VACATIONTYPE, VacationDate = r.VACATIONDATE };

            userEntryPartial.SelectedStandByItems = new List<CheckBoxListItems>();

            foreach (var r in res)
            {

                if (r.VacationType == "STANDBY")
                {
                    checkBoxListItems = new CheckBoxListItems();
                    checkBoxListItems.CheckBoxText = r.VacationDate.ToString("d/M/yyyy");
                    checkBoxListItems.CheckBoxValue = r.VacationDate.ToString("d/M/yyyy");
                    userEntryPartial.SelectedStandByItems.Add(checkBoxListItems);
                }
            }

            return userEntryPartial.SelectedStandByItems;
        }
        public List<CheckBoxListItems> GetAvailableLeaveDates(DateTime date)
        {
            //List<KeyValuePair<DateTime, string>> vacationDates = new List<KeyValuePair<DateTime, string>>();
            userentryTableAdapter.Fill(context.USERENTRY);
            employeeTableAdapter.Fill(context.EMPLOYEE);

            userEntryPartial.AvailableLeaveItems = new List<CheckBoxListItems>();

            for (var day = date.AddDays(-6); day.Date <= date.Date; day = day.AddDays(1))
            {
                checkBoxListItems = new CheckBoxListItems();
                checkBoxListItems.CheckBoxText = day.ToString("d/M/yyyy");
                checkBoxListItems.CheckBoxValue = day.ToString("d/M/yyyy");

                userEntryPartial.AvailableLeaveItems.Add(checkBoxListItems);
            }

            return userEntryPartial.AvailableLeaveItems;
        }
        public List<CheckBoxListItems> GetAvailableWFHDates(DateTime date)
        {
            //List<KeyValuePair<DateTime, string>> vacationDates = new List<KeyValuePair<DateTime, string>>();
            userentryTableAdapter.Fill(context.USERENTRY);
            employeeTableAdapter.Fill(context.EMPLOYEE);

            userEntryPartial.AvailableWFHItems = new List<CheckBoxListItems>();

            for (var day = date.AddDays(-6); day.Date <= date.Date; day = day.AddDays(1))
            {

                checkBoxListItems = new CheckBoxListItems();
                checkBoxListItems.CheckBoxText = day.ToString("d/M/yyyy");
                checkBoxListItems.CheckBoxValue = day.ToString("d/M/yyyy");

                userEntryPartial.AvailableWFHItems.Add(checkBoxListItems);
            }

            return userEntryPartial.AvailableWFHItems;
        }
        public List<CheckBoxListItems> GetAvailableSecondShiftDates(DateTime date)
        {
            //List<KeyValuePair<DateTime, string>> vacationDates = new List<KeyValuePair<DateTime, string>>();
            userentryTableAdapter.Fill(context.USERENTRY);
            employeeTableAdapter.Fill(context.EMPLOYEE);

            userEntryPartial.AvailableSecondShiftItems = new List<CheckBoxListItems>();

            for (var day = date.AddDays(-6); day.Date <= date.Date; day = day.AddDays(1))
            {

                checkBoxListItems = new CheckBoxListItems();
                checkBoxListItems.CheckBoxText = day.ToString("d/M/yyyy");
                checkBoxListItems.CheckBoxValue = day.ToString("d/M/yyyy");

                userEntryPartial.AvailableSecondShiftItems.Add(checkBoxListItems);
            }

            return userEntryPartial.AvailableSecondShiftItems;
        }
        public List<CheckBoxListItems> GetAvailableStandByDates(DateTime date)
        {
            //List<KeyValuePair<DateTime, string>> vacationDates = new List<KeyValuePair<DateTime, string>>();
            userentryTableAdapter.Fill(context.USERENTRY);
            employeeTableAdapter.Fill(context.EMPLOYEE);

            userEntryPartial.AvailableStandByItems = new List<CheckBoxListItems>();

            for (var day = date.AddDays(-6); day.Date <= date.Date; day = day.AddDays(1))
            {

                checkBoxListItems = new CheckBoxListItems();
                checkBoxListItems.CheckBoxText = day.ToString("d/M/yyyy");
                checkBoxListItems.CheckBoxValue = day.ToString("d/M/yyyy");

                userEntryPartial.AvailableStandByItems.Add(checkBoxListItems);
            }

            return userEntryPartial.AvailableStandByItems;
        }

        public string InsertLeaves(System.Web.Mvc.FormCollection fc, OleDbConnection con)
        {
            userentryTableAdapter.Fill(context.USERENTRY);
            employeeTableAdapter.Fill(context.EMPLOYEE);

            string[] LeaveDate;
            string[] StandByDate;
            string[] WFHDate;
            string[] SecondShiftDate;
            string name = fc["Name"];
            string hdn1, hdn2, hdn3, hdn4;
            hdn1 = fc["hdnLeaveStatus"];
            hdn2 = fc["hdnStandByStatus"];
            hdn3 = fc["hdnWFHStatus"];
            hdn4 = fc["hdnSecondShiftStatus"];
            string weekend = fc["WeekendSelected"];
            var emp = from e in context.EMPLOYEE
                      where e.EMPNAME == name
                      select e.EMP_ID;
            string emp_id = emp.FirstOrDefault<string>();
            DateTime dt = DateTime.ParseExact(weekend, "d/M/yyyy", CultureInfo.InvariantCulture);
            string status = "";
            try
            {
                if (fc["PostedLeaveItem.ItemIds"] != null && fc["PostedLeaveItem.ItemIds"] != "")
                {
                    LeaveDate = fc["PostedLeaveItem.ItemIds"].Split(',');

                    con.Open();
                    string command = "DELETE From USERENTRY WHERE EMP_ID = '" + emp_id + "' AND WEEKENDDATE = #" + dt.ToShortDateString() + "# AND VACATIONTYPE = 'LEAVE'";
                    OleDbDataAdapter adp = new OleDbDataAdapter();
                    OleDbCommand cmd = new OleDbCommand(command, con);
                    cmd.ExecuteNonQuery();

                    int count = LeaveDate.Count();
                    for (int i = 0; i < count; i++)
                    {
                        DateTime vac = DateTime.ParseExact(LeaveDate[i], "d/M/yyyy", CultureInfo.InvariantCulture);
                        userentryTableAdapter.Insert(emp_id, "LEAVE", vac,hdn1 , dt);

                    }
                    con.Close();

                }
                else
                {
                    string command = "DELETE From USERENTRY WHERE EMP_ID = '" + emp_id + "' AND WEEKENDDATE = #" + dt.ToShortDateString() + "# AND VACATIONTYPE = 'LEAVE'";
                    con.Open();
                    OleDbDataAdapter adp = new OleDbDataAdapter();
                    OleDbCommand cmd = new OleDbCommand(command, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                if (fc["PostedStandByItem.ItemIds"] != null && fc["PostedStandByItem.ItemIds"] != "")
                {
                    StandByDate = fc["PostedStandByItem.ItemIds"].Split(',');
                    con.Open();
                    string command = "DELETE From USERENTRY WHERE EMP_ID = '" + emp_id + "' AND WEEKENDDATE = #" + dt.ToShortDateString() + "# AND VACATIONTYPE = 'STANDBY'";
                    OleDbDataAdapter adp = new OleDbDataAdapter();
                    OleDbCommand cmd = new OleDbCommand(command, con);
                    cmd.ExecuteNonQuery();

                    int count = StandByDate.Count();
                    for (int i = 0; i < count; i++)
                    {
                        DateTime vac = DateTime.ParseExact(StandByDate[i], "d/M/yyyy", CultureInfo.InvariantCulture);
                        userentryTableAdapter.Insert(emp_id, "STANDBY", vac, hdn2, dt);

                    }
                    con.Close();
                }
                else
                {
                    string command = "DELETE From USERENTRY WHERE EMP_ID = '" + emp_id + "' AND WEEKENDDATE = #" + dt.ToShortDateString() + "# AND VACATIONTYPE = 'STANDBY'";
                    con.Open();
                    OleDbDataAdapter adp = new OleDbDataAdapter();
                    OleDbCommand cmd = new OleDbCommand(command, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                if (fc["PostedWFHItem.ItemIds"] != null && fc["PostedWFHItem.ItemIds"] != "")
                {
                    WFHDate = fc["PostedWFHItem.ItemIds"].Split(',');

                    con.Open();
                    string command = "DELETE From USERENTRY WHERE EMP_ID = '" + emp_id + "' AND WEEKENDDATE = #" + dt.ToShortDateString() + "#AND VACATIONTYPE = 'WFH'";
                    OleDbDataAdapter adp = new OleDbDataAdapter();
                    OleDbCommand cmd = new OleDbCommand(command, con);
                    cmd.ExecuteNonQuery();

                    int count = WFHDate.Count();
                    for (int i = 0; i < count; i++)
                    {
                        DateTime vac = DateTime.ParseExact(WFHDate[i], "d/M/yyyy", CultureInfo.InvariantCulture);
                        userentryTableAdapter.Insert(emp_id, "WFH", vac, hdn3, dt);

                    }
                    con.Close();
                }
                else
                {
                    string command = "DELETE From USERENTRY WHERE EMP_ID = '" + emp_id + "' AND WEEKENDDATE = #" + dt.ToShortDateString() + "# AND VACATIONTYPE = 'WFH'";
                    con.Open();
                    OleDbDataAdapter adp = new OleDbDataAdapter();
                    OleDbCommand cmd = new OleDbCommand(command, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                if (fc["PostedSecondShiftItem.ItemIds"] != null && fc["PostedSecondShiftItem.ItemIds"] != "")
                {
                    SecondShiftDate = fc["PostedSecondShiftItem.ItemIds"].Split(',');

                    con.Open();
                    string command = "DELETE From USERENTRY WHERE EMP_ID = '" + emp_id + "' AND WEEKENDDATE = #" + dt.ToShortDateString() + "# AND VACATIONTYPE = 'SECONDSHIFT'";
                    OleDbDataAdapter adp = new OleDbDataAdapter();
                    OleDbCommand cmd = new OleDbCommand(command, con);
                    cmd.ExecuteNonQuery();

                    int count = SecondShiftDate.Count();
                    for (int i = 0; i < count; i++)
                    {
                        DateTime vac = DateTime.ParseExact(SecondShiftDate[i], "d/M/yyyy", CultureInfo.InvariantCulture);
                        userentryTableAdapter.Insert(emp_id, "SECONDSHIFT", vac, hdn4, dt);

                    }
                    con.Close();
                }
                else
                {
                    string command = "DELETE From USERENTRY WHERE EMP_ID = '" + emp_id + "' AND WEEKENDDATE = #" + dt.ToShortDateString() + "# AND VACATIONTYPE = 'SECONDSHIFT'";
                    con.Open();
                    OleDbDataAdapter adp = new OleDbDataAdapter();
                    OleDbCommand cmd = new OleDbCommand(command, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                status = "Request Submitted Successfully!";
            }
            catch(Exception ex)
            {
                status = ex.Message;
            }

            return status;
        }

        public string GetLeaveStatus(string user, DateTime date)
        {
            employeeTableAdapter.Fill(context.EMPLOYEE);
            userentryTableAdapter.Fill(context.USERENTRY);

            var res = (from u in context.USERENTRY
                       join e in context.EMPLOYEE
                             on u.EMP_ID equals e.EMP_ID
                       where e.EMPNAME == user && u.WEEKENDDATE == date && u.VACATIONTYPE == "LEAVE"
                       select u.STATUS).FirstOrDefault<string>();
            if (res == null)
            {
                res = "PENDING";
            }
            return res;

        }
        public string GetStandByStatus(string user, DateTime date)
        {
            employeeTableAdapter.Fill(context.EMPLOYEE);
            userentryTableAdapter.Fill(context.USERENTRY);

            var res = (from u in context.USERENTRY
                       join e in context.EMPLOYEE
                             on u.EMP_ID equals e.EMP_ID
                       where e.EMPNAME == user && u.WEEKENDDATE == date && u.VACATIONTYPE == "STANDBY"
                       select u.STATUS).FirstOrDefault<string>();
            if (res == null)
            {
                res = "PENDING";
            }
            return res;
        }
        public string GetSecondShiftStatus(string user, DateTime date)
        {
            employeeTableAdapter.Fill(context.EMPLOYEE);
            userentryTableAdapter.Fill(context.USERENTRY);

            var res = (from u in context.USERENTRY
                       join e in context.EMPLOYEE
                             on u.EMP_ID equals e.EMP_ID
                       where e.EMPNAME == user && u.WEEKENDDATE == date && u.VACATIONTYPE == "SECONDSHIFT"
                       select u.STATUS).FirstOrDefault<string>();
            if (res == null)
            {
                res = "PENDING";
            }
            return res;
        }
        public string GetWFHStatus(string user, DateTime date)
        {
            employeeTableAdapter.Fill(context.EMPLOYEE);
            userentryTableAdapter.Fill(context.USERENTRY);

            var res = (from u in context.USERENTRY
                       join e in context.EMPLOYEE
                             on u.EMP_ID equals e.EMP_ID
                       where e.EMPNAME == user && u.WEEKENDDATE == date && u.VACATIONTYPE == "WFH"
                       select u.STATUS).FirstOrDefault<string>();
            if (res == null)
            {
                res = "PENDING";
            }
            return res;
        }

        public string ApproveReject(string user, string date, string type, string act)
        {
            userentryTableAdapter.Fill(context.USERENTRY);
            employeeTableAdapter.Fill(context.EMPLOYEE);
            string status = "";
            try
            {
                var emp = from e in context.EMPLOYEE
                          where e.EMPNAME == user
                          select e.EMP_ID;
                string emp_id = emp.FirstOrDefault<string>();
                DateTime dt = DateTime.ParseExact(date, "d/M/yyyy", CultureInfo.InvariantCulture);
                string con = ConfigurationManager.ConnectionStrings["TrackerConnectionString"].ToString();
                OleDbConnection conn = new OleDbConnection(con);
                conn.Open();
                string command = "UPDATE USERENTRY SET STATUS = '" + act + "' WHERE EMP_ID = '" + emp_id + "' AND WEEKENDDATE = #" + dt.ToShortDateString() + "# AND VACATIONTYPE = '" + type + "'";
                OleDbCommand cmd = new OleDbCommand(command, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                status = "Record updated successfully";

            }
            catch (Exception ex)
            {
                status = ex.Message;
            }
            return status;
        }
    }
}