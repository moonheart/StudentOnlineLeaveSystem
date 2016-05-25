using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using LeaveSystem.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LeaveSystem.Web.DAL
{
    public class RoleStore : RoleStore<Role, string, UserRole>, IQueryableRoleStore<Role>
    {
        public RoleStore()
            : base((DbContext) new IdentityDbContext())
        {
            this.DisposeContext = true;
        }

        public RoleStore(DbContext context)
            : base(context)
        {
        }
    }
}