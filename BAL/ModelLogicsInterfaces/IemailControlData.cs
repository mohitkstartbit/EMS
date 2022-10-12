using BOL;
using BOL.Responses;
using System.Threading.Tasks;

namespace BAL
{
    public interface IemailControlData
    {
        Task<statusResponse> ChangePassword(forget fo);
        Task<statusResponse> CheckId(string id);
        statusResponse SendEmail(string emailAddress, string body, string subject);
        Task<statusResponse> resetPassword(employee emp);
    }
}