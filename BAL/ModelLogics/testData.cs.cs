using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using BOL;
using System.Linq;
using System.Threading.Tasks;

namespace BAL
{
    public class testData : ItestData
    {
        private readonly IUnitOfWork _unitofwork;
        public testData(IUnitOfWork unitOfWork)
        {
            _unitofwork = unitOfWork;
        }
        public async Task<IEnumerable<test>> GetAll()
        {
            return await _unitofwork.tests.GetData();
        }
        public async Task<test> Insert(test u)
        {
            var result = await _unitofwork.tests.AddData(u);
            var resultcheck =await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);

        }
        public async Task<test> Update(test u)
        {
            var result = await _unitofwork.tests.EditData(u);
            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);
        }
        public async Task<test> GetById(int id)
        {
            return await _unitofwork.tests.GetDataById(id);
        }
        public async Task<test> Delete(int id)
        {
            var result = await _unitofwork.tests.DeleteData(id);
            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);
        }
    }
}
