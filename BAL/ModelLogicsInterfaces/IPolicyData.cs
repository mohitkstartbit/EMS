using BOL.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public interface IPolicyData
    {
        Task<IEnumerable<Policy>> GetAll();
        Task<Policy> GetById(int id);
        Task<Policy> Insert(Policy u);
        Task<Policy> Update(Policy u);
    }
}
