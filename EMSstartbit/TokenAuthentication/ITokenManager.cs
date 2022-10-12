using BOL.ApiModels;
using BOL.Responses;
using System.Threading.Tasks;

namespace EMSstartbit.TokenAuthentication
{
    public interface ITokenManager
    {
        object Ecoding { get; }

        Task<AuthResponse> Authenticate(AuthModel au);
        Task<string> NewToken(string username);
        Task<string> VerifyToken(string token);
         Task<string> GenerateRefreshToken();
    }
}