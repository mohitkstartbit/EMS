using BOL.DatabaseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BAL
{
    public interface IEmailData
    {
        Task<IEnumerable<EmailTemplate>> GetAll();
        Task<EmailTemplate> GetById(int id);
            Task<EmailTemplate> GetByEmailType(string type);
        Task<EmailTemplate> Insert(EmailTemplate u);
        
        Task<EmailTemplate> Update(EmailTemplate u);
    }
}