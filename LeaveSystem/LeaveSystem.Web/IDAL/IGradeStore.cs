using System.Collections.Generic;
using System.Threading.Tasks;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.IDAL
{
    public interface IGradeStore : IBaseStore<Grade>
    {
        Task<Grade> FindGradeByNumberAsync(string gradeNum);

    }
}
