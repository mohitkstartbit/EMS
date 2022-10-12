using System;
using System.Collections.Generic;
using System.Text;
using BOL;
using System.Linq;
using DAL;
using System.Threading.Tasks;

namespace BAL
{
    public class loginData : IloginData
    {
        private readonly IUnitOfWork _unitOfWork;


        public loginData(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<login> getByEid(int id)
        {
            await _unitOfWork.roles.GetData();
            var data = await _unitOfWork.logins.GetByExpression(x=>x.employee_id == id);
            return data;
        }
        public async Task<login> getByRefreshtoken(string id,int employee_id)
        {
            await _unitOfWork.roles.GetData();
            var data = await _unitOfWork.logins.GetByExpression(x => x.refreshtoken == id && x.employee_id == employee_id);
            return data;
        }
        public async Task<IEnumerable<login>> GetAll()
        {
            await _unitOfWork.roles.GetData();
            await _unitOfWork.employees.GetData();
            return await _unitOfWork.logins.GetData();
        }
        public async Task<login> Insert(login u)
        {
            var result = await _unitOfWork.logins.AddData(u);
           
            var resultcheck = await _unitOfWork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);
        }
        public async Task<login> Update(login u)
        {
            var result = await _unitOfWork.logins.EditData(u);
         
            var resultcheck = await _unitOfWork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);

        }
        public async Task<login> GetById(int id)
        {
            return await _unitOfWork.logins.GetDataById(id);
        }
        public async Task<login> Delete(int id)
        {
            var result = await _unitOfWork.logins.DeleteData(id);
            var resultcheck = await _unitOfWork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);
        }

    }
}
