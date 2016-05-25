using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.IBLL
{
    public interface IPositionManager : IBaseManager<Position>
    {
        Task<Position> FindPositionByNameAsync(string name);

        IQueryable<Position> FindNoOfficePositions();

        Task SetPositionsOfficeAsync(int[] ids, Office office);

        Task ResetPositionsOfficeAsync(int[] ids);
    }
}
