using Raique.Microservices.Authenticate.Domain;

namespace Raique.Microservices.Authenticate.Protocols
{
    public interface ITokenCreator
    {
        string Create(User user);
    }
}
