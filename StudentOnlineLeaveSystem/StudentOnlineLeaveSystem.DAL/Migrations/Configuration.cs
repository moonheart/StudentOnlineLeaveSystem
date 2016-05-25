using System.Collections.Generic;
using StudentOnlineLeaveSystem.Models;

namespace StudentOnlineLeaveSystem.DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<StudentOnlineLeaveSystem.DAL.StudentOnlineLeaveSystemDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(StudentOnlineLeaveSystem.DAL.StudentOnlineLeaveSystemDbContext context)
        {
            //var users = new List<User>
            //{
            //    new User {UserName = "月心月士",DisplayName = "月心月士",Email = "adsa@ge.dd",LastLoginTime = DateTime.Now,LoginIp = "::1",Password = "jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=",RegistrationTime = DateTime.Now,Status = 0}
            //};
            //users.ForEach(u => context.Users.AddOrUpdate(p => p.UserId, u));
            //context.SaveChanges();
        }
    }
}
