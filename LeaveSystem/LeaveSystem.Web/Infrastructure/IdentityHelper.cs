using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using LeaveSystem.Web.BLL;
using Microsoft.AspNet.Identity.Owin;

namespace LeaveSystem.Web.Infrastructure
{
    public static class IdentityHelper
    {
        public static MvcHtmlString GetUserName(this HtmlHelper html, string id)
        {
            UserManager userManager = HttpContext.Current.GetOwinContext().GetUserManager<UserManager>();
            return new MvcHtmlString(userManager.FindByIdAsync(id).Result.Name);
        }
        public static MvcHtmlString ClaimType(this HtmlHelper html, string claimType)
        {
            FieldInfo[] fields = typeof(ClaimTypes).GetFields();
            foreach (FieldInfo field in fields)
            {
                if (field.GetValue(null).ToString() == claimType)
                {
                    return new MvcHtmlString(field.Name);
                }
            }
            return new MvcHtmlString(string.Format("{0}",
            claimType.Split('/', '.').Last()));
        }
    }
}