using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using BOL;
using System.Threading.Tasks;

namespace BAL
{
    public class roleData : IroleData
    {
        private readonly IUnitOfWork _unitofwork;
        public roleData(IUnitOfWork unitOfWork)
        {
            _unitofwork = unitOfWork;
        }
        public async Task<IEnumerable<role>> GetAll()
        {
            var result = await _unitofwork.roles.GetData();
            
            return result;
        }
        public async Task<role> Insert(role u)
        {
            var result = await _unitofwork.roles.AddData(u);
            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);
        }
        public async Task<role> Edit(role u)
        {

            var result = await _unitofwork.roles.EditData(u);
            var resultcheck = await _unitofwork.CompleteAsync(); 
            return await Task.Run(() => (resultcheck) ? result : null);
        }
        public async Task<role> GetById(int id)
        {
            var result  = await _unitofwork.roles.GetDataById(id);
            return result;
        }
        public async Task<role> Delete(int id)
        {
            var result = await _unitofwork.roles.DeleteData(id);
            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);

        }
    }
}
