using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using BOL;
using System.Linq;
using System.Threading.Tasks;

namespace BAL
{
    public class bloodgroupData : IbloodgroupData
    {
        private readonly IUnitOfWork _unitofwork;
        public bloodgroupData(IUnitOfWork unitOfWork)
        {
            _unitofwork = unitOfWork;
        }
        public async Task<IEnumerable<bloodgroup>> GetAll()
        {
            return await _unitofwork.bloodgroups.GetData();
        }
        public async Task<bloodgroup> Insert(bloodgroup u)
        {
            var result = await _unitofwork.bloodgroups.AddData(u);
            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);

        }
        public async Task<bloodgroup> Delete(int id)
        {
            var result = await _unitofwork.bloodgroups.DeleteData(id);
            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);
        }
    }
}
