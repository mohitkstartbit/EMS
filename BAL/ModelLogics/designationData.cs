using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using BOL;
using System.Threading.Tasks;

namespace BAL
{
    public class designationData : IdesignationData
    {
        private readonly IUnitOfWork _unitofwork;
        public designationData(IUnitOfWork unitOfWork)
        {
            _unitofwork = unitOfWork;
        }
        public async Task<IEnumerable<designation>> GetAll()
        {
            return await _unitofwork.designations.GetData();

        }
        public async Task<designation> Insert(designation u)
        {
            var result = await _unitofwork.designations.AddData(u);

            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);

        }
        public async Task<designation> Update(designation u)
        {
            var result = await _unitofwork.designations.EditData(u);

            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);

        }
        public async Task<designation> GetById(int id)
        {
            return await _unitofwork.designations.GetDataById(id);
        }
        public async Task<designation> Delete(int id)
        {
            var result = await _unitofwork.designations.DeleteData(id);
            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);
        }
    }
}
