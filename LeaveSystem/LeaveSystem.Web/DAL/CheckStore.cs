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
    public class CheckStore : BaseStore<Check>, ICheckStore
    {

        public CheckStore(DbContext context)
            : base(context)
        {
        }
    }
}