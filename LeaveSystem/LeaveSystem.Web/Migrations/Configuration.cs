using System.Collections.Generic;
using LeaveSystem.Web.BLL;
using LeaveSystem.Web.DAL;
using LeaveSystem.Web.Infrastructure;
using LeaveSystem.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebGrease.Css.Extensions;

namespace LeaveSystem.Web.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AppDbContext context)
        {
            //初始化
            var userManager = new UserManager(new UserStore(context));
            var roleManager = new RoleManager(new RoleStore(context));
            var departmentManaegr = new DepartmentManager(new DepartmentStore(context));

            string number = "000000";
            string name = "管理员";
            string password = "123456";

            if (!roleManager.RoleExists("Administrator"))
            {
                roleManager.Create(new Role("Administrator") { Description = "这个是管理员" });
            }
            if (!roleManager.RoleExists("Teacher"))
            {
                roleManager.Create(new Role("Teacher") { Description = "这个是教师" });
            }
            if (!roleManager.RoleExists("Student"))
            {
                roleManager.Create(new Role("Student") { Description = "这个是学生" });
            }

            var user = userManager.FindByName(number);
            if (user == null)
            {
                userManager.Create(new User { UserName = number, Name = name }, password);
                user = userManager.FindByName(number);
            }
            if (!userManager.IsInRole(user.Id, "Administrator"))
            {
                userManager.AddToRole(user.Id, "Administrator");
            }

            var userList1 = new[]
            {
                new  User("851900"){Name = "教师00"}, 
                new  User("851901"){Name = "教师01"}, 
                new  User("851902"){Name = "教师02"}, 
                new  User("851903"){Name = "教师03"}, 
                new  User("851904"){Name = "教师04"}, 
                new  User("851905"){Name = "教师05"}, 
                new  User("851906"){Name = "教师06"}, 
                new  User("851907"){Name = "教师07"}, 
                new  User("851908"){Name = "教师08"}, 
                new  User("851909"){Name = "教师09"}, 
            };
            var userList2 = new[]
            {
                new  User("201210409120"){Name = "学生00"}, 
                new  User("201210409121"){Name = "学生01"}, 
                new  User("201210409122"){Name = "学生02"}, 
                new  User("201210409123"){Name = "学生03"}, 
                new  User("201210409124"){Name = "学生04"}, 
                new  User("201210409125"){Name = "学生05"}, 
                new  User("201210409126"){Name = "学生06"}, 
                new  User("201210409127"){Name = "学生07"}, 
                new  User("201210409128"){Name = "学生08"}, 
                new  User("201210409129"){Name = "学生09"}, 
            };

            foreach (var user1 in userList1)
            {
                var tu = userManager.FindByName(user1.UserName);
                if (tu == null)
                {
                    userManager.Create(new User { UserName = user1.UserName, Name = user1.Name }, password);
                    tu = userManager.FindByName(user1.UserName);
                }
                if (!userManager.IsInRole(tu.Id, "Teacher"))
                {
                    userManager.AddToRole(tu.Id, "Teacher");
                }
            }

            foreach (var user1 in userList2)
            {
                var tu = userManager.FindByName(user1.UserName);
                if (tu == null)
                {
                    userManager.Create(new User { UserName = user1.UserName, Name = user1.Name }, password);
                    tu = userManager.FindByName(user1.UserName);
                }
                if (!userManager.IsInRole(tu.Id, "Student"))
                {
                    userManager.AddToRole(tu.Id, "Student");
                }
            }

            new[]
            {
                new Department{Name = "信息科学与工程学院"}, 
                new Department{Name = "药学与生物工程学院"}, 
                new Department{Name = "美术与影视学院"}, 
                new Department{Name = "医学院（护理学院）"}, 
                new Department{Name = "师范学院"}, 
                new Department{Name = "外国语学院"}, 
                new Department{Name = "机械工程学院"}, 
                new Department{Name = "建筑与土木工程学院"}, 
                new Department{Name = "旅游与经济管理学院"}, 
                new Department{Name = "文学与新闻传播学院"}, 
                new Department{Name = "政治学院.法学系"}, 
                new Department{Name = "体育学院"}, 
            }.ForEach(e => context.Departments.AddOrUpdate(d => d.Name, e));
            context.SaveChanges();

            new[]
            {
                new Grade{GradeNum = "2010"}, 
                new Grade{GradeNum = "2011"}, 
                new Grade{GradeNum = "2012"}, 
                new Grade{GradeNum = "2013"}, 
                new Grade{GradeNum = "2014"}, 
                new Grade{GradeNum = "2015"}, 
                new Grade{GradeNum = "2016"}, 
                new Grade{GradeNum = "2017"}, 
                new Grade{GradeNum = "2018"}, 
            }.ForEach(e => context.Grades.AddOrUpdate(d => d.GradeNum, e));
            context.SaveChanges();

            var tdepart = departmentManaegr.FindDepartmentByNameAsync("信息科学与工程学院").Result;
            new[]
            {
                new Office(){Department = tdepart,Description = "行政行政行政行政",Name = "行政"},
                new Office(){Department = tdepart,Description = "党务党务党务党务",Name = "党务"},
                new Office(){Department = tdepart,Description = "学院办公室学院办公室",Name = "学院办公室"},
                new Office(){Department = tdepart,Description = "教务办公室教务办公室",Name = "教务办公室"},
                new Office(){Department = tdepart,Description = "学生工作办公室学生工作办公室",Name = "学生工作办公室"},
            }.ForEach(e => context.Offices.AddOrUpdate(d => d.Name, e));
            context.SaveChanges();

            new[]
            {
                new Major(){Department = tdepart,Name = "软件工程"},
                new Major(){Department = tdepart,Name = "数字媒体技术"},
                new Major(){Department = tdepart,Name = "计算机科学"},
                new Major(){Department = tdepart,Name = "物联网工程"},
                new Major(){Department = tdepart,Name = "网络工程"},
                new Major(){Department = tdepart,Name = "信息科学与技术"},
            }.ForEach(e => context.Majors.AddOrUpdate(d => d.Name, e));
            context.SaveChanges();

            new[]
            {
                new LeaveConfig()
                {
                    Id = 1,
                    LeastDayToSign = 1,
                    LeastResumeDay = 1,
                    ResumeClassroom = "10507",
                    LeastSickLeaveDay = 2,
                    MaxLeaveDayInOneMonth = 10,
                    MaxLeaveClassInOneMonth = 20,
                    MaxLeaveTimeInOneMonth = 5
                }
            }.ForEach(e => context.LeaveConfigs.AddOrUpdate(d => d.Id, e));
            context.SaveChanges();

            new[]
            {
                new LessonInfo()
                {
                    BelongDepartment = tdepart,
                    LessonName = "软件工程"
                }
            }.ForEach(e => context.LessonInfos.AddOrUpdate(d => d.LessonName));



        }
    }
}
