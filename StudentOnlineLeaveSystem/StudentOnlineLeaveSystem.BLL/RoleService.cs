using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentOnlineLeaveSystem.DAL;
using StudentOnlineLeaveSystem.IBLL;
using StudentOnlineLeaveSystem.IDAL;
using StudentOnlineLeaveSystem.Models;

namespace StudentOnlineLeaveSystem.BLL
{
    public class RoleService : BaseService<Role>, IRoleService
    {
        public RoleService()
            : base(RepositoryFactory.RoleRepository)
        {
        }

        public Role Find(int roleId)
        {
            return CurrentRepository.Find(r => r.RoleId == roleId);
        }

        public IQueryable<Role> FindPageList(int pageIndex, int pageSize, out int totalRecord, int order)
        {
            IQueryable<Role> roles = CurrentRepository.Entities;
            switch (order)
            {
                default:
                    roles = roles.OrderBy(r => r.RoleId);
                    break;
            }
            totalRecord = roles.Count();
            return PageList(roles, pageIndex, pageSize);
        }
    }
}
