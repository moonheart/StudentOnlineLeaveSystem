using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeaveSystem.Web.Models;
using Microsoft.AspNet.Identity;

namespace LeaveSystem.Web.IDAL
{
    public interface ILoginLogStore<TUser, in TKey> :
        IDisposable
        where TUser : class ,IUser<TKey>
    {
        Task AddLoginLogAsync(TUser user, LoginLog loginLog);

        Task<List<LoginLog>> GetLoginLogsAsync(TUser user);

        Task<TUser> FindAsync(LoginLog loginLog);
    }
}
