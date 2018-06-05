using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMT.Controllers
{
    public class SecureController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["LoggedIn"] == null)
            {
                filterContext.Result = RedirectToAction("Login", "Login");
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }

        }
    }
    }
