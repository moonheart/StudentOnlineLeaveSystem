using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentOnlineLeaveSystem.Models;

namespace StudentOnlineLeaveSystem.DAL
{
    /// <summary>
    /// 数据上下文
    /// </summary>
    public class StudentOnlineLeaveSystemDbContext : DbContext
    {
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRoleRelation> UserRoleRelations { get; set; }
        public DbSet<UserConfig> UserConfig { get; set; }
        public DbSet<Leave> Leaves { get; set; }

        public StudentOnlineLeaveSystemDbContext()
            : base("StudentOnlineLeaveSystemDbContext")
        {

        }
    }
}
