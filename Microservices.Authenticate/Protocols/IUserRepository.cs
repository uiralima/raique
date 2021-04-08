using Raique.Microservices.Authenticate.Domain;

namespace Raique.Microservices.Authenticate.Protocols
{
    public interface IUserRepository
    {
        User GetByKeyToApp(string key, string appKey);
        int Create(User user);
        User GetById(int userId);
        void ChangePassword(int userId, string password);
    }
}
