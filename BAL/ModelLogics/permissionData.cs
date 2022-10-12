using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using BOL;
using System.Threading.Tasks;

namespace BAL
{
    public class permissionData : IpermissionData
    {
        private readonly IUnitOfWork _unitofwork;
        public permissionData(IUnitOfWork unitOfWork)
        {
            _unitofwork = unitOfWork;
        }
        public async Task<IEnumerable<permission>> GetAll()
        {
            return await _unitofwork.permissions.GetData();
        }
        public async Task<permission> Insert(permission u)
        {
            var result = await _unitofwork.permissions.AddData(u);
            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);

        }
        public async Task<permission> Update(permission u)
        {
            var result = await _unitofwork.permissions.EditData(u);

            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);
        }
        public async Task<permission> GetById(int id)
        {
            return await _unitofwork.permissions.GetDataById(id);
        }
        public async Task<permission> Delete(int id)
        {
            var result = await _unitofwork.permissions.DeleteData(id);

            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);
        }
    }
}
