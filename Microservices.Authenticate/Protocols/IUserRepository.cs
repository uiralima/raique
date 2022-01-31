using Raique.Microservices.Authenticate.Domain;
using System.Threading.Tasks;

namespace Raique.Microservices.Authenticate.Protocols
{
    public interface IUserRepository
    {
        Task<User> GetByKeyToApp(string key, string appKey);
        Task<int> Create(User user);
        Task<User> GetById(int userId);
        Task ChangePassword(int userId, string password);
        Task<bool> ChangePasswordByCode(string userName, string code, string password);
    }
}
