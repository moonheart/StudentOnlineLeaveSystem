using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using Microsoft.AspNet.Identity;

namespace LeaveSystem.Web.Infrastructure
{
    public static class MyIdentityExtensions
    {
        public static string GetName(this IIdentity identity)
        {
            if (identity == null)
                throw new ArgumentNullException("identity");
            ClaimsIdentity identity1 = identity as ClaimsIdentity;
            if (identity1 != null)
            {
                return IdentityExtensions.FindFirstValue(identity1, "Name");
                //return IdentityExtensions.FindFirstValue(identity1, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            }
            return (string)null;
        }

    }
}