using BOL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BAL
{
    public interface IshiftData
    {
        Task<shift> Delete(int id);
        Task<IEnumerable<shift>> GetAll();
        Task<shift> GetById(int id);
        Task<shift> Insert(shift u);
        Task<shift> Update(shift u);
    }
}