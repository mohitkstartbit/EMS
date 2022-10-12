
using BOL.DatabaseModels;
using DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class emailData : IEmailData
    {
        private readonly IUnitOfWork _unitofwork;
        public emailData(IUnitOfWork unitOfWork)
        {
            _unitofwork = unitOfWork;
        }
        public async Task<IEnumerable<EmailTemplate>> GetAll()
        {
            return await _unitofwork.EmailTemplates.GetData();
        }

        public async Task<EmailTemplate> GetById(int id)
        {
            return await _unitofwork.EmailTemplates.GetDataById(id);
        }

        public async Task<EmailTemplate> Insert(EmailTemplate u)
        {
            var result = await _unitofwork.EmailTemplates.AddData(u);
            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);
        }
        public async Task<EmailTemplate> GetByEmailType(string type)
        {
            var x = await _unitofwork.EmailTemplates.GetByExpression(u => u.email_type == type);
            return x;
        }
        public async Task<EmailTemplate> Update(EmailTemplate u)
        {
            var result = await _unitofwork.EmailTemplates.EditData(u);
            var resultcheck = await _unitofwork.CompleteAsync();
            return await Task.Run(() => (resultcheck) ? result : null);
        }
    }
}
