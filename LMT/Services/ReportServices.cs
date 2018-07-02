using LMT.Models.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace LMT.Services
{
    public class ReportServices
    {
        EmployeeEntity context = new EmployeeEntity();
        EmployeeEntityTableAdapters.EMPLOYEETableAdapter employeeTableAdapters =
            new EmployeeEntityTableAdapters.EMPLOYEETableAdapter();
        EmployeeEntityTableAdapters.USERENTRYTableAdapter userentryTableAdapter =
            new EmployeeEntityTableAdapters.USERENTRYTableAdapter();
        public List<EmployeeMaster> MasterReport()
        {
            employeeTableAdapters.Fill(context.EMPLOYEE);
            var employee = from e in context.EMPLOYEE
                           orderby e.EMPNAME
                           select new { EmpID = e.EMP_ID, ATTUID = e.ATTUID, Name = e.EMPNAME, Phone = e.PHONE, Email = e.EMAIL };
            List<EmployeeMaster> list = new List<EmployeeMaster>();
            EmployeeMaster emp;
            foreach (var e in employee)
            {
                emp = new EmployeeMaster();
                emp.EmpID = e.EmpID.ToString();
                emp.ATTUID = e.ATTUID.ToString();
                emp.Name = e.Name.ToString();
                emp.Phone = e.Phone.ToString();
                emp.Email = e.Email.ToString();
                list.Add(emp);
            }
            return list;
        }
        public List<string> GetNames()
        {
            employeeTableAdapters.Fill(context.EMPLOYEE);
            List<string> list = (from e in context.EMPLOYEE
                                 orderby e.EMPNAME
                                select e.EMPNAME).ToList();
            return list;
        }
        public List<string> GetTypeList()
        {
            userentryTableAdapter.Fill(context.USERENTRY);
            List<string> list = (from u in context.USERENTRY
                                 select u.VACATIONTYPE).Distinct().ToList();
            return list;
        }
        public List<DetailedReport> GetDetailedReport(DetailedReport model)
        {
            employeeTableAdapters.Fill(context.EMPLOYEE);
            userentryTableAdapter.Fill(context.USERENTRY);
            List<DetailedReport> list = new List<DetailedReport>();
            if(model.UserName == "ALL" && model.StatusSelected == "ALL")
            {
                var employee = from e in context.EMPLOYEE
                               join u in context.USERENTRY
                                    on e.EMP_ID equals u.EMP_ID
                               where u.VACATIONTYPE == model.RequestType && DateTime.ParseExact(model.FromDate, "d/M/yyyy", CultureInfo.InvariantCulture)<= u.VACATIONDATE
                                                                         && DateTime.ParseExact(model.ToDate, "d/M/yyyy", CultureInfo.InvariantCulture) >= u.VACATIONDATE
                               orderby u.STATUS descending,e.EMPNAME,u.VACATIONDATE
                               select new { EmpID = e.EMP_ID, EmpName = e.EMPNAME, RequestDate = u.VACATIONDATE, Status = u.STATUS };
                DetailedReport dr;
                foreach(var e in employee)
                {
                    dr = new DetailedReport();
                    dr.EmpID = e.EmpID;
                    dr.UserName = e.EmpName;
                    dr.RequestDate = e.RequestDate.ToString("d/M/yyyy");
                    dr.StatusSelected = e.Status;

                    list.Add(dr);
                }
            }
            if (model.UserName == "ALL" && model.StatusSelected != "ALL")
            {
                var employee = from e in context.EMPLOYEE
                               join u in context.USERENTRY
                                    on e.EMP_ID equals u.EMP_ID
                               where u.VACATIONTYPE == model.RequestType && DateTime.ParseExact(model.FromDate, "d/M/yyyy", CultureInfo.InvariantCulture) <= u.VACATIONDATE
                                                                         && DateTime.ParseExact(model.ToDate, "d/M/yyyy", CultureInfo.InvariantCulture) >= u.VACATIONDATE
                                                                         && u.STATUS == model.StatusSelected
                               orderby u.STATUS descending, e.EMPNAME, u.VACATIONDATE
                               select new { EmpID = e.EMP_ID, EmpName = e.EMPNAME, RequestDate = u.VACATIONDATE, Status = u.STATUS };
                DetailedReport dr;
                foreach (var e in employee)
                {
                    dr = new DetailedReport();
                    dr.EmpID = e.EmpID;
                    dr.UserName = e.EmpName;
                    dr.RequestDate = e.RequestDate.ToString("d/M/yyyy");
                    dr.StatusSelected = e.Status;

                    list.Add(dr);
                }
            }
            if (model.UserName != "ALL" && model.StatusSelected == "ALL")
            {
                var employee = from e in context.EMPLOYEE
                               join u in context.USERENTRY
                                    on e.EMP_ID equals u.EMP_ID
                               where u.VACATIONTYPE == model.RequestType && DateTime.ParseExact(model.FromDate, "d/M/yyyy", CultureInfo.InvariantCulture) <= u.VACATIONDATE
                                                                         && DateTime.ParseExact(model.ToDate, "d/M/yyyy", CultureInfo.InvariantCulture) >= u.VACATIONDATE
                                                                         && e.EMPNAME == model.UserName
                               orderby u.STATUS descending, e.EMPNAME, u.VACATIONDATE
                               select new { EmpID = e.EMP_ID, EmpName = e.EMPNAME, RequestDate = u.VACATIONDATE, Status = u.STATUS };
                DetailedReport dr;
                foreach (var e in employee)
                {
                    dr = new DetailedReport();
                    dr.EmpID = e.EmpID;
                    dr.UserName = e.EmpName;
                    dr.RequestDate = e.RequestDate.ToString("d/M/yyyy");
                    dr.StatusSelected = e.Status;

                    list.Add(dr);
                }
            }
            if (model.UserName != "ALL" && model.StatusSelected != "ALL")
            {
                var employee = from e in context.EMPLOYEE
                               join u in context.USERENTRY
                                    on e.EMP_ID equals u.EMP_ID
                               where u.VACATIONTYPE == model.RequestType && DateTime.ParseExact(model.FromDate, "d/M/yyyy", CultureInfo.InvariantCulture) <= u.VACATIONDATE
                                                                         && DateTime.ParseExact(model.ToDate, "d/M/yyyy", CultureInfo.InvariantCulture) >= u.VACATIONDATE
                                                                         && u.STATUS == model.StatusSelected
                                                                         && e.EMPNAME == model.UserName
                               orderby u.STATUS descending, e.EMPNAME, u.VACATIONDATE
                               select new { EmpID = e.EMP_ID, EmpName = e.EMPNAME, RequestDate = u.VACATIONDATE, Status = u.STATUS };
                DetailedReport dr;
                foreach (var e in employee)
                {
                    dr = new DetailedReport();
                    dr.EmpID = e.EmpID;
                    dr.UserName = e.EmpName;
                    dr.RequestDate = e.RequestDate.ToString("d/M/yyyy");
                    dr.StatusSelected = e.Status;

                    list.Add(dr);
                }
            }
            return list;
        }
    }
}