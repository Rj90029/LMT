using LMT.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMT.Services
{
    public class LeaveStatusServices
    {
        EmployeeEntity context = new EmployeeEntity();
        EmployeeEntityTableAdapters.EMPLOYEETableAdapter employTableAdapter = new EmployeeEntityTableAdapters.EMPLOYEETableAdapter();
        EmployeeEntityTableAdapters.USERENTRYTableAdapter employeeTableAdapter = new EmployeeEntityTableAdapters.USERENTRYTableAdapter();
        public List<LeaveStatus> GetDetails()
        {
            List<LeaveStatus> details = new List<LeaveStatus>();
            LeaveStatus leaveStatus;
            employTableAdapter.Fill(context.EMPLOYEE);
            employeeTableAdapter.Fill(context.USERENTRY);
            var result = from e in context.USERENTRY join d in context.EMPLOYEE on e.EMP_ID equals d.EMP_ID
                         orderby e.STATUS descending,d.EMPNAME,e.VACATIONDATE
                         select new { Name = d.EMPNAME, vacation = e.VACATIONDATE, status=e.STATUS, vactionType=e.VACATIONTYPE };

            foreach (var r in result)
            {
                leaveStatus = new LeaveStatus();
                leaveStatus.Name = r.Name;
                leaveStatus.VacationType = r.vactionType;
                leaveStatus.VacationDate = r.vacation.ToString("d/M/yyyy");
                leaveStatus.Status = r.status;
                details.Add(leaveStatus);
            }

            return details;
        }

    }
}