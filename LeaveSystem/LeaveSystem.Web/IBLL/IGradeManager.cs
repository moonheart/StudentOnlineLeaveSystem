using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.IBLL
{
    public interface IGradeManager : IBaseManager<Grade>
    {
        Task<Grade> FindGradeByGradeNumberAsync(string num);

    }
}
