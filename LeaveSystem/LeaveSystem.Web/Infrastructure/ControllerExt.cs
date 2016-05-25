using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using Microsoft.AspNet.Identity;

namespace LeaveSystem.Web.Infrastructure
{
    public class ControllerExt : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userid = User.Identity.GetUserId();

            base.OnActionExecuting(filterContext);
        }
    }
}