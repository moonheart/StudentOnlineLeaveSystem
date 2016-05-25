using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.IDAL
{
    public interface IStudentInfoStore
    {
        Task<StudentInfo> GetStudentInfoAsync(string userId);

        Task AddStudentInfo(string userId, StudentInfo info);

        Task DeleteStudentInfo(StudentInfo studentInfo);

    }
}
