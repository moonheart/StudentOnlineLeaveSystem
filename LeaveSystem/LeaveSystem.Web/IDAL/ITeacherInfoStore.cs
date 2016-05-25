using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.IDAL
{
    public interface ITeacherInfoStore
    {
        Task<TeacherInfo> GetTeacherInfoAsync(string userId);

        Task AddTeacherInfo(string userId, TeacherInfo info);

        Task DeleteTeacherInfo(TeacherInfo teacherInfo);

    }
}
