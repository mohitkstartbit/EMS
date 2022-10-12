using BOL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BAL
{
    public interface IrolePermissionData
    {
        Task<RolePermission> Delete(int id);
        Task<IEnumerable<RolePermission>> GetAll();
        Task<RolePermission> GetById(int id);
        Task<RolePermission> Insert(RolePermission u);
        Task<RolePermission> Update(RolePermission u);
        Task<List<permission>> GetByroleid(int id);
        Task<List<int>> GetPermissionIdsByroleid(int id);
        Task<IEnumerable<RolePermission>> DeleteByRoleAndPermissionids(int roleid, IEnumerable<int> pids);
        Task<IEnumerable<RolePermission>> AddByRoleAndPermissionids(int roleid, IEnumerable<int> pids);
    }
}