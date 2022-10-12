using BOL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BAL
{
    public interface IuserPermissionData
    {
        Task<UserPermission> Delete(int id);
        Task<IEnumerable<UserPermission>> GetAll();
        Task<IEnumerable<permission>> GetByEid(int id);
        Task<UserPermission> GetById(int id);
        Task<UserPermission> Insert(UserPermission u);
        Task<UserPermission> Update(UserPermission u);
        Task<IEnumerable<UserPermission>> DeleteMultiple(IEnumerable<int> id);
        Task<IEnumerable<int>> GetAllUserPermisssionIdByEid(int id);
        Task<IEnumerable<UserPermission>> InsertMultiple(IEnumerable<UserPermission> u);
        Task<IEnumerable<UserPermission>> DeleteByRoleAndPermissionids(int roleid, IEnumerable<int> pids);
        Task<IEnumerable<UserPermission>> AddByRoleAndPermissionidsAndEmp(IEnumerable<int> emplist, int roleid, IEnumerable<int> pids);
        Task<IEnumerable<UserPermission>> GetAllUserPermisssionByEid(int id);
        Task<IEnumerable<UserPermission>> DeleteByEmployeeAndPermissionids(int empid, IEnumerable<int> pids);
    }
}