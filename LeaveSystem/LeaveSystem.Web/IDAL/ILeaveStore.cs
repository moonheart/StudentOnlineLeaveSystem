using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using LeaveSystem.Web.Models;
using Microsoft.AspNet.Identity;

namespace LeaveSystem.Web.IDAL
{
    public interface ILeaveStore :
        IBaseStore<Leave>
    {
        IQueryable<Leave> FindLeavesByUserIdAsync(string userId);

    }
}