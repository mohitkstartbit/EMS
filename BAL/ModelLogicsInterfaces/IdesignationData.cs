using BOL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BAL
{
    public interface IdesignationData
    {
        Task<designation> Delete(int id);
        Task<IEnumerable<designation>> GetAll();
        Task<designation> GetById(int id);
        Task<designation> Insert(designation u);
        Task<designation> Update(designation u);
    }
}