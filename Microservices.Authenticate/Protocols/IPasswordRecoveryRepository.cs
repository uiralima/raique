using System.Threading.Tasks;

namespace Raique.Microservices.Authenticate.Protocols
{
    public interface IPasswordRecoveryRepository
    {
        Task Create(int userId, string code);
    }
}
