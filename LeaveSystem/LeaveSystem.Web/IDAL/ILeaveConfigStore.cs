using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.IDAL
{
    public interface ILeaveConfigStore : IDisposable
    {
        Task<LeaveConfig> GetConfigAsync();

        LeaveConfig GetConfig();

        Task SetConfigAsync(LeaveConfig config);

        void SetConfig(LeaveConfig config);

    }
}