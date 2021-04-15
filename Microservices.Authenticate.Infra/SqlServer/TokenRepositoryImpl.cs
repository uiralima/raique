using Raique.Database.SqlServer.Contracts;
using Raique.Microservices.Authenticate.Protocols;
using System.Threading.Tasks;

namespace Raique.Microservices.Authenticate.Infra.SqlServer
{
    public class TokenRepositoryImpl : Base, ITokenRepository
    {
        public TokenRepositoryImpl(IDatabaseConfig config) : base(config)
        {
        }

        public async Task Close(string device, string token)
        {
            string query = @"DELETE Token WHERE TokenStr = @token AND Device = @device";
            using(var session = CreateSession())
            {
                session.SetQuery(query)
                    .AddParameter("@token", token)
                    .AddParameter("@device", device);
                await session.ExecuteNonQueryAsync();
            }
        }

        public async Task Create(int userId, string device, string token)
        {
            string query = @"
                            DELETE Token WHERE UserId = @userId AND Device = @device 
                            INSERT INTO Token(TokenStr, UserId, Device, CreatedDate, LastPing)
                                VALUES(@token, @userId, @device, @getdate, @getdate)";
            using (var session = CreateSession())
            {
                session.SetQuery(query)
                    .AddParameter("@userId", userId)
                    .AddParameter("@device", device)
                    .AddParameter("@token", token)
                    .AddParameter("@getdate", Raique.Library.DateUtilities.Now);
                await session.ExecuteNonQueryAsync();
            }
        }

        public async Task<int> GetUserIdByToken(string device, string token)
        {
            string query = @"
                            UPDATE Token SET LastPing = @getdate WHERE TokenStr = @token AND Device = @device
                            SELECT UserId FROM Token WHERE TokenStr = @token AND Device = @device
                            ";
            using (var session = CreateSession())
            {
                session.SetQuery(query)
                    .AddParameter("@token", token)
                    .AddParameter("@device", device)
                    .AddParameter("@getdate", Raique.Library.DateUtilities.Now);
                using(var reader = await session.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                }
            }
            return 0;
        }
    }
}
