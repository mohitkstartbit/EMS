using BOL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BAL
{
    public interface IroleData
    {
        Task<role> Delete(int id);
        Task<IEnumerable<role>> GetAll();
        Task<role> GetById(int id);
        Task<role> Insert(role u);
        Task<role> Edit(role u);
    }
}