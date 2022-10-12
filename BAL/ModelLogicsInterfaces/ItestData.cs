using BOL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BAL
{
    public interface ItestData
    {
        Task<test> Delete(int id);
        Task<IEnumerable<test>> GetAll();
        Task<test> GetById(int id);
        Task<test> Insert(test u);
        Task<test> Update(test u);
    }
}