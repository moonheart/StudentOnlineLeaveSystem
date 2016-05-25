using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.IBLL
{
    public interface ICheckManager : IBaseManager<Check>
    {
        Task<ICollection<Check>> GetTeachersCheckAsync(User user);

        ICollection<Check> GetTeachersCheck(User user);

        Task<ICollection<Check>> GetTeachersNoCheckAsync(User user);

        ICollection<Check> GetTeachersNoCheck(User user);

        Task<ICollection<Check>> GetToCheckAsync(User user);

    }
}