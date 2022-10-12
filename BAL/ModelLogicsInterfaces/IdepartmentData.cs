using BOL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BAL
{
    public interface IdepartmentData
    {
        Task<department> Delete(int id);
        Task<IEnumerable<department>> GetAll();
        Task<department> GetById(int id);
        Task<department> Insert(department u);
        Task<department> Update(department u);
    }
}