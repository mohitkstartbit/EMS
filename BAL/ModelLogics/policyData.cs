
using BOL.DatabaseModels;
using DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class policyData : IPolicyData
    {
        private readonly IUnitOfWork _unitofwork;
        public policyData(IUnitOfWork unitOfWork)
        {
            _unitofwork = unitOfWork;
        }
        public async Task<IEnumerable<Policy>> GetAll()
        {
            return await _unitofwork.Policies.GetData();
        }

        public async Task<Policy> GetById(int id)
        {
            return await _unitofwork.Policies.GetDataById(id);
        }

        public async Task<Policy> Insert(Policy u)
        {
            var result = await _unitofwork.Policies.AddData(u);
            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);
        }

        public async Task<Policy> Update(Policy u)
        {
            var result = await _unitofwork.Policies.EditData(u);
            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);
        }
    }
}
