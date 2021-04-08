
using Raique.Database.SqlServer.Implementations;

namespace Raique.Database.SqlServer.Contracts
{
    public abstract class Base
    {
        private readonly IDatabaseConfig _config;
        protected Base(IDatabaseConfig config)
        {
            _config = config;
        }
        protected virtual Raique.Database.Contracts.ISession CreateSession()
        {
            return new SqlServerSession(_config);
        }
    }
}
