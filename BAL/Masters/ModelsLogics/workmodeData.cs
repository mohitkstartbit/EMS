using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using BOL;
using System.Linq;
using System.Threading.Tasks;

namespace BAL
{
    public class workmodeData : IworkmodeData
    {
        private readonly IUnitOfWork _unitofwork;
        public workmodeData(IUnitOfWork unitOfWork)
        {
            _unitofwork = unitOfWork;
        }
        public async Task<IEnumerable<workmode>> GetAll()
        {
            return await _unitofwork.workmodes.GetData();
        }
        public async Task<workmode> Insert(workmode u)
        {
            var result = await _unitofwork.workmodes.AddData(u);
            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);

        }
        public async Task<workmode> Delete(int id)
        {
            var result = await _unitofwork.workmodes.DeleteData(id);
            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);
        }
    }
}
