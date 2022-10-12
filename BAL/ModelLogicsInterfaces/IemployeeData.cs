using BOL;
using BOL.DerivedClasses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BAL
{
    public interface IemployeeData
    {
        Task<employee> Delete(int id);
        Task<IEnumerable<employee>> GetAll();
        Task<employee> GetById(int id);
        Task<employee> GetByEmailId(string id);
        Task<employee> Insert(employee u);
        Task<employee> Update(employee u);
        Task<IEnumerable<int>> GetEmployeeIdswithRoleids(int roleid);
        Task<employee> GetEmployeefromEmpFiles(employeeFiles empfiles);
    }
}