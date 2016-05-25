using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using LeaveSystem.Web.DAL;
using LeaveSystem.Web.IBLL;
using LeaveSystem.Web.Infrastructure;
using LeaveSystem.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace LeaveSystem.Web.BLL
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class UserManager : UserManager<User>, IStudentManager, ITeacherManager
    {
        protected UserStore UserStore;

        public override IQueryable<User> Users
        {
            get { return base.Users.OrderBy(e => e.Name); }
        }

        public IQueryable<TeacherInfo> TeacherInfos
        {
            get { return UserStore.TeacherInfos; }
        }

        public IQueryable<StudentInfo> StudentInfos
        {
            get { return UserStore.StudentInfos; }
        }


        public UserManager(UserStore store)
            : base(store)
        {
            UserStore = store;
        }

        public static UserManager Create(
                IdentityFactoryOptions<UserManager> options,
                IOwinContext context)
        {

            AppDbContext db = context.Get<AppDbContext>();
            //UserStore<T> 是 包含在 Microsoft.AspNet.Identity.EntityFramework 中，它实现了 UserManger 类中与用户操作相关的方法。 
            //也就是说UserStore<T>类中的方法（诸如：FindById、FindByNameAsync...）通过EntityFramework检索和持久化UserInfo到数据库中
            UserManager manager = new UserManager(new UserStore(db));

            //自定义的User Validator
            manager.UserValidator = new CustomUserValidator(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };

            //自定义的Password Validator
            manager.PasswordValidator = new CustomPasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            };
            return manager;
        }

        ///// <summary>
        ///// 删除用户
        ///// </summary>
        ///// <param name="user"></param>
        ///// <returns></returns>
        //public async Task DeleteUserAsync(User user)
        //{
        //    await DeleteUserAsync(user);
        //}

        public async Task<IEnumerable<User>> GetNoClassStudentsAsync()
        {
            var list = Users.Include(e => e.StudentInfo).Include(e => e.StudentInfo.BelongClass).Where(u => u.StudentInfo.BelongClass == null).ToList();
            var res = new List<User>();
            foreach (var user in list)
            {
                if (await IsInRoleAsync(user.Id, "Student"))
                {
                    res.Add(user);
                }
            }
            return res;
        }

        #region 角色增删重写

        public override async Task<IdentityResult> AddToRoleAsync(string userId, string role)
        {
            var result = await base.AddToRoleAsync(userId, role);
            if (result.Succeeded)
            {
                if (role == "Teacher")
                {
                    await UserStore.AddTeacherInfo(userId, new TeacherInfo());
                }
                else if (role == "Student")
                {
                    await UserStore.AddStudentInfo(userId, new StudentInfo());
                }
                return IdentityResult.Success;
            }
            return result;
        }

        public override async Task<IdentityResult> AddToRolesAsync(string userId, params string[] roles)
        {
            var result = await base.AddToRolesAsync(userId, roles);
            if (result.Succeeded)
            {
                foreach (var role in roles)
                {
                    if (role == "Teacher")
                    {
                        await UserStore.AddTeacherInfo(userId, new TeacherInfo());
                    }
                    else if (role == "Student")
                    {
                        await UserStore.AddStudentInfo(userId, new StudentInfo());
                    }
                }
                return IdentityResult.Success;
            }
            return result;
        }

        public override async Task<IdentityResult> RemoveFromRoleAsync(string userId, string role)
        {
            var result = await base.RemoveFromRoleAsync(userId, role);
            if (result.Succeeded)
            {
                if (role == "Teacher")
                {
                    var teacher = await UserStore.GetTeacherInfoAsync(userId);
                    await UserStore.DeleteTeacherInfo(teacher);
                }
                else if (role == "Student")
                {
                    var student = await UserStore.GetStudentInfoAsync(userId);
                    await UserStore.DeleteStudentInfo(student);
                }
                return IdentityResult.Success;
            }
            return result;
        }

        public override async Task<IdentityResult> RemoveFromRolesAsync(string userId, params string[] roles)
        {
            var result = await base.RemoveFromRolesAsync(userId, roles);
            if (result.Succeeded)
            {
                foreach (var role in roles)
                {
                    if (role == "Teacher")
                    {
                        var teacher = await UserStore.GetTeacherInfoAsync(userId);
                        await UserStore.DeleteTeacherInfo(teacher);
                    }
                    else if (role == "Student")
                    {
                        var student = await UserStore.GetStudentInfoAsync(userId);
                        await UserStore.DeleteStudentInfo(student);
                    }
                }
                return IdentityResult.Success;
            }
            return result;
        }

        #endregion

        public async Task<IEnumerable<User>> GetNoClassTeachersAsync()
        {
            var list =
                Users.Where(e => e.TeacherInfo != null)
                    .Include(e => e.TeacherInfo.ManageClass)
                    .Where(u => u.TeacherInfo.ManageClass == null);
            //var newlist=list.ToList();
            var res = new List<User>();
            foreach (var user in list)
            {
                if (await IsInRoleAsync(user.Id, "Teacher"))
                {
                    res.Add(user);
                }
            }
            return res;
        }

        public IQueryable<User> GetStudentsOfClassAsync(Class class1)
        {
            if (class1 == null)
                throw new ArgumentNullException("class1");
            return Users.Where(e => e.StudentInfo.BelongClass.Id == class1.Id);
        }

        public async Task<IEnumerable<User>> GetNoPositionTeachersAsync()
        {
            var list = Users.Include(e => e.TeacherInfo).Include(e => e.TeacherInfo.Position).Where(u => u.TeacherInfo.Position == null).ToList();
            var res = new List<User>();
            foreach (var user in list)
            {
                if (await IsInRoleAsync(user.Id, "Teacher"))
                {
                    res.Add(user);
                }
            }
            return res;
        }

        public async Task<IEnumerable<User>> GetInPositionTeachersAsync()
        {
            var list = Users.Include(e => e.TeacherInfo).Include(e => e.TeacherInfo.Position).Where(u => u.TeacherInfo.Position != null).ToList();
            var res = new List<User>();
            foreach (var user in list)
            {
                if (await IsInRoleAsync(user.Id, "Teacher"))
                {
                    res.Add(user);
                }
            }
            return res;
        }

        public async Task AddStudentsToClassAsync(Class class1, string[] studentsid)
        {
            if (class1 == null)
                throw new ArgumentNullException("class1");
            if (studentsid == null)
                throw new ArgumentNullException("studentsid");
            var list = new List<User>();
            foreach (var student in studentsid)
            {
                var user = await UserStore.FindByIdAsync(student);
                if (user != null)
                    list.Add(user);
            }
            await UserStore.AddStudentsToClassAsync(class1, list.ToArray());

        }

        public async Task RemoveStudentsFromClassAsync(string[] studentsid)
        {
            if (studentsid == null)
                throw new ArgumentNullException("studentsid");
            var list = Users.Where(
                e => studentsid.Contains(e.Id) &&
                    e.StudentInfo != null);
            await UserStore.RemoveStudentsFromClassAsync(list.ToArray());
        }

        public async Task RemoveAllStudentsFromClassAsync(Class class1)
        {
            if (class1 == null)
                throw new ArgumentNullException("class1");
            var list = Users.Where(
                e => e.StudentInfo != null &&
                    e.StudentInfo.BelongClass != null &&
                    e.StudentInfo.BelongClass.Id == class1.Id);
            await UserStore.RemoveStudentsFromClassAsync(list.ToArray());
        }

        public async Task AddStudentToClassAsync(Class class1, User student)
        {
            if (class1 == null)
                throw new ArgumentNullException("class1");
            if (student == null)
                throw new ArgumentNullException("student");
            await UserStore.AddStudentToClassAsync(class1, student);
        }

        /// <summary>
        /// 写入登陆记录
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="log">登陆记录</param>
        /// <returns></returns>
        public async Task<IdentityResult> WriteLoginLogAsync(User user, LoginLog log)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (log == null)
                throw new ArgumentNullException("log");
            await this.UserStore.AddLoginLogAsync(user, log);
            var result = IdentityResult.Success;
            return result;
        }
    }
}
