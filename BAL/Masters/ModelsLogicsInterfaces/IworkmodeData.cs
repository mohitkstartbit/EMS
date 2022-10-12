using BOL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BAL
{
    public interface IworkmodeData
    {
        Task<workmode> Delete(int id);
        Task<IEnumerable<workmode>> GetAll();
        Task<workmode> Insert(workmode u);
    }
}