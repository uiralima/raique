using Raique.Database.Contracts;
using Raique.Database.SqlServer.Contracts;
using Raique.Microservices.Authenticate.Domain;
using System.Threading.Tasks;

namespace Raique.Microservices.Authenticate.Infra.SqlServer
{
    public class AppRepossitoryImpl : Raique.Database.SqlServer.Contracts.Base, Raique.Microservices.Authenticate.Protocols.IAppRepository
    {
        public AppRepossitoryImpl(IDatabaseConfig config) : base(config)
        {
        }

        public async Task<int> Create(string name, string key)
        {
            string query = "INSERT INTO [dbo].[App] ([AppKey], [Fullname], [IsInvalid]) VALUES (@appKey, @fullname, 1)";
            using(var session = CreateSession())
            {
                session.SetQuery(query)
                    .AddParameter("@appKey", key)
                    .AddParameter("@fullname", name);
                return await session.ExecuteNonQueryAsync();
            }
        }

        public async Task<App> GetByKey(string key)
        {
            string query = "SELECT [AppKey], [Fullname] FROM [dbo].[App] WHERE [AppKey] = @appKey AND [IsInvalid] = 1";
            using (var session = CreateSession())
            {
                session.SetQuery(query)
                    .AddParameter("@appKey", key);
                return await ReadAsync(session);
            }
        }

        public async Task<App> GetByName(string appName)
        {
            string query = "SELECT [AppKey], [Fullname] FROM [dbo].[App] WHERE [Fullname] = @fullname AND [IsInvalid] = 1";
            using (var session = CreateSession())
            {
                session.SetQuery(query)
                    .AddParameter("@fullname", appName);
                return await ReadAsync(session);
            }
            
        }

        private async Task<App> ReadAsync(ISession session)
        {
            using (var reader = await session.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    int i = 0;
                    return new App
                    {
                        Key = reader.GetString(i++),
                        Name = reader.GetString(i++)
                    };
                }
            }
            return App.Invalid<App>();
        }
    }
}
