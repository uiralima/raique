using Raique.Database.SqlServer.Contracts;
using System.Data.Common;
using System.Data.SqlClient;

namespace Raique.Database.SqlServer.Implementations
{
    public class SqlServerSession : Raique.Database.Contracts.Session
    {
        public SqlServerSession(IDatabaseConfig config) : base($"Server={config.Server};Database={config.Database};User Id={config.UserName};Password={config.Password};")
        { }
        protected override DbCommand GetCommand()
        {
            return new SqlCommand();
        }

        protected override DbConnection GetConnection()
        {
            return new SqlConnection();
        }

        protected override DbParameter GetParameter(string name, object value)
        {
            return new SqlParameter(name, value);
        }
    }
}
