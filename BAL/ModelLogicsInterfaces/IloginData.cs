using BOL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BAL
{
    public interface IloginData
    {
        Task<login> Delete(int id);
        Task<IEnumerable<login>> GetAll();
        Task<login> getByEid(int id);
        Task<login> GetById(int id);
        Task<login> Insert(login u);
        Task<login> Update(login u);
        Task<login> getByRefreshtoken(string id, int employee_id);
    }
}