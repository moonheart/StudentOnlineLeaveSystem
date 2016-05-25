using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.IBLL
{
    public interface IStudentManager
    {
        IQueryable<User> GetStudentsOfClassAsync(Class class1);

    }
}