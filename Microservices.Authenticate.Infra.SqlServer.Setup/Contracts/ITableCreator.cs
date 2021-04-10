using System.Threading.Tasks;

namespace Raique.Microservices.Authenticate.Infra.SqlServer.Setup.Contracts
{
    public interface ITableCreator
    {
        string TableName { get; }
        Task Create();
    }
}
