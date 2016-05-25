using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using LeaveSystem.Web.IDAL;
using LeaveSystem.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LeaveSystem.Web.DAL
{
    public class UserStore :
        UserStore<User, Role, string, IdentityUserLogin, UserRole, IdentityUserClaim>,
        IUserStore<User>,
        IUserStore<User, string>,
        ILoginLogStore<User, string>,
        IDisposable,
        IStudentInfoStore,
        ITeacherInfoStore
    {
        private DbSet<LoginLog> _loginLogs;
        private EntityStore<User> _userStore;
        private EntityStore<LoginLog> _loginLogStore;
        private EntityStore<TeacherInfo> _teacherInfoStore;
        private EntityStore<StudentInfo> _studentInfoStore;


        public IQueryable<StudentInfo> StudentInfos
        {
            get { return _studentInfoStore.EntitySet; }
        }

        public IQueryable<TeacherInfo> TeacherInfos
        {
            get { return _teacherInfoStore.EntitySet; }
        }


        public UserStore(DbContext context)
            : base(context)
        {
            _userStore = new EntityStore<User>(context);
            _loginLogStore = new EntityStore<LoginLog>(context);
            _loginLogs = Context.Set<LoginLog>();
            _teacherInfoStore = new EntityStore<TeacherInfo>(context);
            _studentInfoStore = new EntityStore<StudentInfo>(context);
        }

        public async Task AddLoginLogAsync(User user, LoginLog loginLog)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (loginLog == null)
                throw new ArgumentNullException("loginLog");
            //DbSet<LoginLog> dbSet = _loginLogs;
            LoginLog instance = new LoginLog(loginLog.LoginIp, loginLog.LoginTime);
            instance.UserId = user.Id;
            //instance.UserId = loginLog.UserId;
            LoginLog entity = instance;
            _loginLogStore.Create(entity);
            //dbSet.Add(entity);
            await SaveChangesAsync();
        }
        private async Task SaveChangesAsync()
        {
            if (this.AutoSaveChanges)
            {
                int num = await this.Context.SaveChangesAsync();
            }
        }

        public Task<List<LoginLog>> GetLoginLogsAsync(User user)
        {
            if ((object)user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult(user.LoginLogs.Select(l => new LoginLog(l.LoginIp, l.LoginTime)).ToList());
        }

        public Task<List<User>> GetStudentsOfClass(Class class1)
        {
            if (class1 == null)
                throw new ArgumentNullException("class1");
            return Task.FromResult(Users.Include(e => e.StudentInfo).Where(u => u.StudentInfo.BelongClass.Id == class1.Id).ToList());
        }

        public async Task AddStudentsToClassAsync(Class class1, User[] users)
        {
            foreach (var user in users)
            {
                user.StudentInfo.BelongClass = class1;
                _userStore.Update(user);
            }
            await Context.SaveChangesAsync();
        }
        public async Task RemoveStudentsFromClassAsync(User[] users)
        {
            foreach (var user in users)
            {
                user.StudentInfo.BelongClass = null;
                _userStore.Update(user);
            }
            await Context.SaveChangesAsync();
        }
        public async Task AddStudentToClassAsync(Class class1, User user)
        {
            user.StudentInfo.BelongClass = class1;
            _userStore.Update(user);
            await Context.SaveChangesAsync();
        }


        public async Task<User> FindAsync(LoginLog loginLog)
        {
            if (loginLog == null)
                throw new ArgumentNullException("loginLog");
            LoginLog log = this._loginLogs.FirstOrDefault(l => l.Id == loginLog.Id);
            User user;
            if (log != null)
            {
                user = await this.GetUserAggregateAsync(u => u.Id.Equals(log.UserId));
            }
            else
            {
                user = default(User);
            }
            return user;
        }

        public Task<StudentInfo> GetStudentInfoAsync(string userId)
        {
            return _studentInfoStore.EntitySet.SingleOrDefaultAsync(e => e.Student.Id == userId);
        }

        public async Task AddStudentInfo(string userId, StudentInfo info)
        {
            var user = await this.FindByIdAsync(userId);
            user.StudentInfo = new StudentInfo();
            await this.Context.SaveChangesAsync();
        }

        public async Task DeleteStudentInfo(StudentInfo studentInfo)
        {
            if (studentInfo == null)
                throw new ArgumentNullException("studentInfo");
            _studentInfoStore.Delete(studentInfo);
            await this.SaveChangesAsync();
        }

        public Task<TeacherInfo> GetTeacherInfoAsync(string userId)
        {
            return _teacherInfoStore.EntitySet.SingleOrDefaultAsync(e => e.Teacher.Id == userId);
        }

        public async Task AddTeacherInfo(string userId, TeacherInfo info)
        {
            var user = await this.FindByIdAsync(userId);
            user.TeacherInfo = new TeacherInfo();
            await this.Context.SaveChangesAsync();
        }

        public async Task DeleteTeacherInfo(TeacherInfo teacherInfo)
        {
            if (teacherInfo == null)
                throw new ArgumentNullException("teacherInfo");
            _teacherInfoStore.Delete(teacherInfo);
            await this.SaveChangesAsync();
        }
    }
}