using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using LeaveSystem.Web.IDAL;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.DAL
{
    public class ClassStore : BaseStore<Class>, IClassStore
    {
        public ClassStore(DbContext context)
            : base(context)
        {
        }

        public async Task<bool> IsClassExistAsync(Expression<Func<Class, bool>> lamda)
        {
            return await EntityStore.EntitySet.AnyAsync(lamda);
        }
    }
}