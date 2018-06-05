using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LMT.Models.Model;

namespace LMT.Services
{
    public class DashboardService
    {
        // To display dashboard for shalabh
        EmployeeEntity context = new EmployeeEntity();
        EmployeeEntityTableAdapters.EMPLOYEETableAdapter employeeTableAdapter = new EmployeeEntityTableAdapters.EMPLOYEETableAdapter();
        public List<Dashboard> GetDetails(string name)
        {
            List<Dashboard> details = new List<Dashboard>();
            Dashboard dash;
            employeeTableAdapter.Fill(context.EMPLOYEE);
            var result = from e in context.EMPLOYEE
                         orderby e.EMPNAME
                         select new { Name = e.EMPNAME, Phone = e.PHONE };

            foreach(var r in result)
            {
                dash = new Dashboard();
                dash.Name = r.Name;
                dash.ContactNumber = r.Phone;
                details.Add(dash);
            }

            return details;
        }
    }
}