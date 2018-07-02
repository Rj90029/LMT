using LMT.Models.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMT.Services
{
    public class LoginService
    {
        EmployeeEntity context = new EmployeeEntity();
        EmployeeEntityTableAdapters.EMPLOYEETableAdapter employeeTableAdapter = new EmployeeEntityTableAdapters.EMPLOYEETableAdapter();
        EmployeeEntityTableAdapters.USERSTableAdapter usersTableAdapter = new EmployeeEntityTableAdapters.USERSTableAdapter();
        public List<string> Login(string UserName, string Password)
        {
            employeeTableAdapter.Fill(context.EMPLOYEE);
            usersTableAdapter.Fill(context.USERS);
            var result = from u in context.USERS
                         join e in context.EMPLOYEE
                                on u.EMP_ID equals e.EMP_ID
                         where e.EMPNAME == UserName && u.PASSWORD == Password
                         orderby e.EMPNAME
                         select new { User = e.EMPNAME, Role = u.ROLE };
            List<string> ls = new List<string>();
            foreach (var r in result)
            {
                ls.Add(r.User.ToString());
                ls.Add(r.Role.ToString());
            }
            return ls;
        }
        public List<string> GetNames()
        {
            List<string> UserList = new List<string>();
            employeeTableAdapter.Fill(context.EMPLOYEE);
            var result = from e in context.EMPLOYEE
                         orderby e.EMPNAME
                         select new { Name = e.EMPNAME };

            foreach (var r in result)
            {
                UserList.Add(r.Name);
            }

            return UserList;

        }

        public string ResetPassword(ResetPassword model)
        {
            EmployeeEntityTableAdapters.USERSTableAdapter usersTableAdapter = new EmployeeEntityTableAdapters.USERSTableAdapter();
            EmployeeEntityTableAdapters.EMPLOYEETableAdapter employeeTableAdapter = new EmployeeEntityTableAdapters.EMPLOYEETableAdapter();
            EmployeeEntity context = new EmployeeEntity();
            usersTableAdapter.Fill(context.USERS);
            employeeTableAdapter.Fill(context.EMPLOYEE);
            string old_pwd = (from e in context.EMPLOYEE
                              join u in context.USERS
                                        on e.EMP_ID equals u.EMP_ID
                             where e.EMPNAME == model.UserName
                              select u.PASSWORD).First<string>();
            string status = "";
            if(old_pwd == model.LastPassword)
            {
                string emp_id = (from e in context.EMPLOYEE
                                 where e.EMPNAME == model.UserName
                                 select e.EMP_ID).First<string>();
                string con = ConfigurationManager.ConnectionStrings["TrackerConnectionString"].ToString();
                OleDbConnection conn = new OleDbConnection(con);
                conn.Open();
                string command = "Update [USERS] SET [PASSWORD] ='" + model.Password + "' Where EMP_ID='" + emp_id + "'";
                OleDbCommand cmd = new OleDbCommand(command, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                status = "Password Reset Successfull.";
            }
            else
            {
                status = "One of the password you entered does not match!";
            }
            return status;
        }
    }
}