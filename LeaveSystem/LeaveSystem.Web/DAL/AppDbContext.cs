using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using LeaveSystem.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LeaveSystem.Web.DAL
{
    public class AppDbContext : IdentityDbContext<User, Role, string, IdentityUserLogin, UserRole, IdentityUserClaim>
    {
        public AppDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer<AppDbContext>(null);
            //取消注释可以取消加载导航属性
            //Configuration.ProxyCreationEnabled = true;
            //Configuration.LazyLoadingEnabled = true;
        }

        public static AppDbContext Create()
        {
            return new AppDbContext();
        }

        public DbSet<IdentityUserLogin> UserLogins { get; set; }
        public DbSet<IdentityUserClaim> UserClaims { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<LoginLog> LoginLogs { get; set; }
        public DbSet<Department> Departments { get; set; }

        public DbSet<Major> Majors { get; set; }

        public DbSet<Leave> Leaves { get; set; }

        public DbSet<Check> Checks { get; set; }

        public DbSet<Grade> Grades { get; set; }

        public DbSet<Office> Offices { get; set; }

        public DbSet<LeaveConfig> LeaveConfigs { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<StudentInfo> StudentInfos { get; set; }

        public DbSet<TeacherInfo> TeacherInfos { get; set; }
        public DbSet<LessonInfo> LessonInfos { get; set; }
        public DbSet<LessonAsign> LessonAsigns { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            // Configure Asp Net Identity Tables
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>().Property(u => u.PasswordHash).HasMaxLength(500);
            modelBuilder.Entity<User>().Property(u => u.SecurityStamp).HasMaxLength(500);
            modelBuilder.Entity<User>().Property(u => u.PhoneNumber).HasMaxLength(50);

            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<UserRole>().ToTable("UserRole");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaim");
            modelBuilder.Entity<IdentityUserClaim>().Property(u => u.ClaimType).HasMaxLength(150);
            modelBuilder.Entity<IdentityUserClaim>().Property(u => u.ClaimValue).HasMaxLength(500);

            //modelBuilder.Entity<Leave>().ToTable("Leave");
            //modelBuilder.Entity<Check>().ToTable("Check");
        }
    }
}