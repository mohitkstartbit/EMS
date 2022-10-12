using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using BOL;
using System.Threading.Tasks;
using System.Linq;

namespace BAL
{
    public class rolePermissionData : IrolePermissionData
    {
        private readonly IUnitOfWork _unitofwork;
        public rolePermissionData(IUnitOfWork unitOfWork)
        {
            _unitofwork = unitOfWork;
        }
        public async Task<IEnumerable<RolePermission>> GetAll()
        {

            await _unitofwork.permissions.GetData();
            await _unitofwork.roles.GetData();
            return await _unitofwork.RolePermissions.GetData();
        }
        public async Task<RolePermission> Insert(RolePermission u)
        {
            var result = await _unitofwork.RolePermissions.AddData(u);

            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);

        }
        public async Task<RolePermission> Update(RolePermission u)
        {
            var result = await _unitofwork.RolePermissions.EditData(u);

            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);
        }
        public async Task<RolePermission> GetById(int id)
        {
            return await _unitofwork.RolePermissions.GetDataById(id);
        }
        public async Task<RolePermission> Delete(int id)
        {
            var result = await _unitofwork.RolePermissions.DeleteData(id);
            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);
        }
        public async Task<List<permission>> GetByroleid(int id)
        {

            List<permission> vs = new List<permission>();
            var x = await this.GetAll();
            await Task.Run(() =>
            {
                foreach (var el in x)
                {
                    if (el.role_id == id)
                    {

                        vs.Add(el.permissions);

                    }
                }
            });
            return vs;
        }
        public async Task<List<int>> GetPermissionIdsByroleid(int id)
        {
            List<int> vs = new List<int>();
            var x = await this.GetAll();
            await Task.Run(() =>
            {
                foreach (var el in x)
                {
                    if (el.role_id == id)
                    {

                        vs.Add(el.permission_id);

                    }
                }
            });
            return vs;
        }
        public async Task<IEnumerable<RolePermission>> DeleteByRoleAndPermissionids(int roleid, IEnumerable<int> pids)
        {
            var x = await this.GetAll();
            var result = await Task.Run(() => { return x.Where(u => u.role_id == roleid && pids.Contains(u.permission_id)).Select(u => u.role_permission_id); });
            var resultedlist = await _unitofwork.RolePermissions.DeleteMultipleData(result);
            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? resultedlist : null);
        }
        public async Task<IEnumerable<RolePermission>> AddByRoleAndPermissionids(int roleid, IEnumerable<int> pids)
        {
            List<RolePermission> rplist = new List<RolePermission>();
            await Task.Run(() =>
            {
                foreach (var i in pids)
                {
                    RolePermission r = new RolePermission();
                    r.permission_id = i;
                    r.role_id = roleid;
                    rplist.Add(r);
                }
            });
            var result = await _unitofwork.RolePermissions.AddMultipleData(rplist);
            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);
        }
    }
}
