using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using LeaveSystem.Web.IDAL;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.DAL
{
    public class PositionStore : BaseStore<Position>, IPositionStore
    {
        public PositionStore(DbContext context)
            : base(context)
        {
        }

        public async Task<Position> FindPositionByNameAsync(string name)
        {
            return await EntityStore.EntitySet.SingleOrDefaultAsync(e => e.Name == name);
        }
    }
}