using BOL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BAL
{
    public interface IbloodgroupData
    {
        Task<bloodgroup> Delete(int id);
        Task<IEnumerable<bloodgroup>> GetAll();
        Task<bloodgroup> Insert(bloodgroup u);
    }
}