using Raique.Microservices.Authenticate.Domain;

namespace Raique.Microservices.Authenticate.Protocols
{
    public interface IAppRepository
    {
        App GetByKey(string key);
        App GetByName(string appName);
        int Create(string name, string key);
    }
}
