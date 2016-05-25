using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LeaveSystem.Web.Models
{
    public class UserRole:IdentityUserRole<string>
    {
        public string Id;
    }
}