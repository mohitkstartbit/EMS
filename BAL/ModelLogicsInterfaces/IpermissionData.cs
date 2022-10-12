using BOL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BAL
{
    public interface IpermissionData
    {
        Task<permission> Delete(int id);
        Task<IEnumerable<permission>> GetAll();
        Task<permission> GetById(int id);
        Task<permission> Insert(permission u);
        Task<permission> Update(permission u);
    }
}