using Raique.Microservices.Authenticate.Domain;
using System.Threading.Tasks;

namespace Raique.Microservices.Authenticate.Protocols
{
    public interface IAppRepository
    {
        Task<App> GetByKey(string key);
        Task<App> GetByName(string appName);
        Task<int> Create(string name, string key);
    }
}
