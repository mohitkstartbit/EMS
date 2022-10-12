using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using BOL;
using System.Linq;
using System.Threading.Tasks;

namespace BAL
{
    public class shiftData : IshiftData
    {
        private readonly IUnitOfWork _unitofwork;
        public shiftData(IUnitOfWork unitOfWork)
        {
            _unitofwork = unitOfWork;
        }
        public async Task<IEnumerable<shift>> GetAll()
        {
            return await _unitofwork.shifts.GetData();
        }
        public async Task<shift> Insert(shift u)
        {
            var result = await _unitofwork.shifts.AddData(u);
            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);

        }
        public async Task<shift> Update(shift u)
        {
            var result = await _unitofwork.shifts.EditData(u);
            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);

        }
        public async Task<shift> GetById(int id)
        {
            return await _unitofwork.shifts.GetDataById(id);
        }
        public async Task<shift> Delete(int id)
        {
            var result = await _unitofwork.shifts.DeleteData(id);
            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);
        }
    }
}
