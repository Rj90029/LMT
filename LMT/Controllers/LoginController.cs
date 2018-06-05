using LMT.Models.Model;
using LMT.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMT.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            Login model = new Models.Model.Login();
            LoginService ls = new LoginService();
            ViewBag.UserList = new SelectList(ls.GetNames());
            if (Session["LoggedIn"] != null)
            {
                Session.Abandon();
                Session.Clear();
            }
            model.UserName = "";
            model.Error = "";
            return View(model);
        }
        [HttpPost]
        public ActionResult Login(Login model)
        {
            string UserName = model.UserName; string Password = model.Password;
            LoginService ls = new LoginService();

            if (UserName != null && Password != null)
            {
                List<string> user = ls.Login(UserName, Password);

                if (user.Count != 0)
                {
                    Session["Role"] = user[1];
                    Session["Name"] = user[0];
                    Session["LoggedIn"] = 1;
                    return RedirectToAction("Dashboard", "Home");
                }
            }
            model.Error = "Your Username Or Password does not match, Please contact Salabh Anand.";
            ViewBag.UserList = new SelectList(ls.GetNames());
            return View(model);
        }

        public ActionResult ResetPassword()
        {
            ResetPassword model = new ResetPassword();
            LoginService ls = new LoginService();
            ViewBag.UserList = new SelectList(ls.GetNames());
            model.UserName = "";
            return View(model);
        }
        [HttpPost]
        public ActionResult ResetPassword(ResetPassword model)
        {
            LoginService ls = new LoginService();
            ls.ResetPassword(model);

            return RedirectToAction("Login");
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");

        }
    }
}