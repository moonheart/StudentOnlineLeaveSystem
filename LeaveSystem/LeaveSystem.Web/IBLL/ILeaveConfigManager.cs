using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.IBLL
{
    public interface ILeaveConfigManager : IDisposable
    {
        Task<LeaveConfig> GetConfigAsync();

        LeaveConfig GetConfig();

        Task SetConfigAsync(LeaveConfig config);

        void SetConfig(LeaveConfig config);

    }
}
