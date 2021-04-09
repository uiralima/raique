using System.Threading.Tasks;

namespace Raique.Microservices.Authenticate.Protocols
{
    public interface ITokenRepository
    {
        Task Create(int userId, string device, string token);
        Task<int> GetUserIdByToken(string device, string token);
        Task Close(string device, string token);
    }
}
