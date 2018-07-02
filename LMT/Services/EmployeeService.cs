using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LMT.Models.Model;
using System.Collections;
using System.Web.Mvc;
using System.Globalization;

namespace LMT.Services
{
    public class EmployeeService
    {
        
        EmployeeEntity context = new EmployeeEntity();
        EmployeeEntityTableAdapters.EMPLOYEETableAdapter employeeTableAdapter = 
            new EmployeeEntityTableAdapters.
            EMPLOYEETableAdapter();
        EmployeeEntityTableAdapters.USERSTableAdapter usersTableAdapter = new 
            EmployeeEntityTableAdapters.USERSTableAdapter();
        public Employee GetDetails(string name)
        {
            

            Employee e = new Employee();

            e.Applications = PopulateAppsLists();
            e.CompetencyList = PopulateCompetencyLists();
            e.DesignationList = PopulateDesigLists();
            e.DOJ = DateTime.Today.ToString("d/M/yyyy");
            e.DOR = "01/01/2099";


            if (name == null)
            {
                return e;
            }

            else
            {

                employeeTableAdapter.Fill(context.EMPLOYEE);

                var result = from row in context.EMPLOYEE
                             where row.EMPNAME == name
                             select new
                             {
                                 EmpID = row.EMP_ID,
                                 AttuId = row.ATTUID,
                                 Name = row.EMPNAME,
                                 Phone = row.PHONE,
                                 Email = row.EMAIL,
                                 doj = row.DOJ,
                                 dor = row.DOR,
                                 position = row.POSITION,
                                 Competency = row.COMPETENCY,
                                 SupApps = row.SUPPAPP,
                             };

                foreach (var r in result)
                {
                    e.Name = r.Name;
                    e.Phone = r.Phone;
                    e.Email = r.Email;

                    e.EmpId = r.EmpID;
                    e.Attuid = r.AttuId;
                    e.DOJ = r.doj.ToString("d/M/yyyy");
                    e.DOR = r.dor.ToString("d/M/yyyy");

                    e.Competency = r.Competency.Split('|').ToList();

                    e.SuppApp = r.SupApps.Split('|').ToList();
                    //e.SuppApp.Add("DOTNET");

                    e.Designation = r.position;

                }

                return e;
            }
        }


        public List<string> PopulateAppsLists()
        {
            EmployeeEntityTableAdapters.APPLICATIONSTableAdapter AppTable =
                new EmployeeEntityTableAdapters.APPLICATIONSTableAdapter();
            AppTable.Fill(context.APPLICATIONS);
            var result = from row in context.APPLICATIONS
                         orderby row.APPLICATIONS
                         select new
                         {
                             Applist = row.APPLICATIONS,
                         };
            List<string> applist = new List<string>();
            foreach (var item in result)
            {
                applist.Add(item.Applist);
            }

            return applist;
            
        }



        public List<string> PopulateCompetencyLists()
        {
            EmployeeEntityTableAdapters.COMPETENCYTableAdapter AppTable =
                new EmployeeEntityTableAdapters.COMPETENCYTableAdapter();
            AppTable.Fill(context.COMPETENCY);
            var result = from row in context.COMPETENCY
                         orderby row.COMPETENCY
                         select new
                         {
                             CompetencyList = row.COMPETENCY
                         };
            List<string> CompetencyList =new List<string>();
            foreach (var item in result)
            {
                CompetencyList.Add(item.CompetencyList);
            }
            return CompetencyList;

        }


        public List<string> PopulateDesigLists()
        {
            EmployeeEntityTableAdapters.POSITIONSTableAdapter DesigTable = 
                new EmployeeEntityTableAdapters.POSITIONSTableAdapter();
            DesigTable.Fill(context.POSITIONS);
            var result = from row in context.POSITIONS
                         orderby row.POSITION
                         select new { DesigList = row.POSITION};
            List<string> DesigList = new List<string>();
            foreach(var item in result)
            {
                DesigList.Add(item.DesigList);
            }
            return DesigList;

        }
        
        public string AddDetails(Employee emp)
        {
            string competency=string.Join("|",emp.Competency);
            string apps = string.Join("|", emp.SuppApp);
            if (emp.DOR == null) emp.DOR = "01/01/2099";
            DateTime doj = DateTime.ParseExact(emp.DOJ, "d/M/yyyy", CultureInfo.InvariantCulture);
            DateTime dor = DateTime.ParseExact(emp.DOR, "d/M/yyyy", CultureInfo.InvariantCulture);
            try
            { 
                employeeTableAdapter.Insert(emp.EmpId,emp.Phone, emp.Attuid, doj, dor, emp.Email, apps, competency, emp.Designation, emp.Name);
                if(emp.Designation == "DELIVERY MANAGER")
                {
                    string password = emp.EmpId;
                    usersTableAdapter.Insert(emp.EmpId, password, "DM");
                }
                else if (emp.Designation == "SR DELIVERY MANAGER")
                {
                    string password = emp.EmpId;
                    usersTableAdapter.Insert(emp.EmpId, password, "SDM");
                }
                else
                {
                    string password = emp.EmpId;
                    usersTableAdapter.Insert(emp.EmpId, password, "USER");
                }
                return ("Insert Successful");
            }
            catch(Exception ex)
            {
                return (ex.Message);

            }
        }
        public string UpdateDetails(Employee emp)
        {
            string competency = string.Join("|", emp.Competency);
            string apps = string.Join("|", emp.SuppApp);


            //EmployeeEntity.EMPLOYEERow rw = context.EMPLOYEE.FindByEMP_ID(emp.EmpId);
            //rw.PHONE = emp.Phone;
            //rw.DOJ = emp.DOJ;
            //rw.DOR = emp.DOR;
            //rw.EMAIL = emp.Email;
            //rw.SUPPAPP = apps;
            //rw.COMPETENCY = competency;
            //rw.POSITION = emp.Designation;
            //Employee e = new Employee();
            DateTime doj = DateTime.ParseExact(emp.DOJ, "d/M/yyyy", CultureInfo.InvariantCulture);
            DateTime dor = DateTime.ParseExact(emp.DOR, "d/M/yyyy", CultureInfo.InvariantCulture);
            employeeTableAdapter.Update(emp.Phone, emp.Attuid, doj, dor, emp.Email, apps, competency, emp.Designation, emp.Name, emp.EmpId);
                

                return ("Update Successful");
           
        }
    }
   
}
