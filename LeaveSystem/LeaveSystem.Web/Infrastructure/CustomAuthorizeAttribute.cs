using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeaveSystem.Web.BLL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace LeaveSystem.Web.Infrastructure
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public Permissions Permisson { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (base.AuthorizeCore(httpContext))
            {
                var user = httpContext.User;
                var rolemanager = httpContext.GetOwinContext().GetUserManager<RoleManager>();
                var roles = rolemanager.GetRolesOfUser(user.Identity.GetUserId());
                user.Identity.GetUserId();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}