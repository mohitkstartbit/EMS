using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using BOL;
using System.Linq;
using System.Threading.Tasks;

namespace BAL
{
    public class departmentData : IdepartmentData
    {
        private readonly IUnitOfWork _unitofwork;
        public departmentData(IUnitOfWork unitOfWork)
        {
            _unitofwork = unitOfWork;
        }
        public async Task<IEnumerable<department>> GetAll()
        {
            return await _unitofwork.departments.GetData();
        }
        public async Task<department> Insert(department u)
        {
            var result = await _unitofwork.departments.AddData(u);
            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);

        }
        public async Task<department> Update(department u)
        {
            var result = await _unitofwork.departments.EditData(u);
            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);


        }
        public async Task<department> GetById(int id)
        {
            return await _unitofwork.departments.GetDataById(id);
        }
        public async Task<department> Delete(int id)
        {
            var result = await _unitofwork.departments.DeleteData(id);
            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);
        }
    }
}
