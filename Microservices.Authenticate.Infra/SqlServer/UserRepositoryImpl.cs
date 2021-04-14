using Raique.Database.Contracts;
using Raique.Database.SqlServer.Contracts;
using Raique.Microservices.Authenticate.Domain;
using System.Threading.Tasks;

namespace Raique.Microservices.Authenticate.Infra.SqlServer
{
    public class UserRepositoryImpl : Raique.Database.SqlServer.Contracts.Base,
        Raique.Microservices.Authenticate.Protocols.IUserRepository
    {
        public UserRepositoryImpl(IDatabaseConfig config) : base(config)
        {
        }

        public async Task ChangePassword(int userId, string password)
        {
            string query = "UPDATE [dbo].[User] SET [Password] = @password WHERE [UserId] = @userId";
            using (var session = CreateSession())
            {
                session.SetQuery(query)
                    .AddParameter("@userId", userId)
                    .AddParameter("@password", password);
                await session.ExecuteNonQueryAsync();
            }
        }

        public async Task<int> Create(User user)
        {
            string query = @"
                            INSERT INTO 
                                    [dbo].[User] 
                                        ([Key],
                                        [Password],
	                                    [CheckKey],
	                                    [AppKey],
	                                    [IsInvalid])
                                    VALUES
                                        (@key,
                                        @password,
                                        @checkKey,
                                        @appKey,
                                        1)
                                SELECT SCOPE_IDENTITY()";
            using (var session = CreateSession())
            {
                session.SetQuery(query)
                    .AddParameter("@key", user.Key)
                    .AddParameter("@password", user.Password)
                    .AddParameter("@checkKey", user.CheckKey)
                    .AddParameter("@appKey", user.AppKey);
                using (var reader = await session.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                }
            }
            return 0;
        }

        public async Task<User> GetById(int userId)
        {
            string query = @"SELECT 
                                [UserId],
                                [Key],
                                [Password],
	                            [CheckKey],
	                            [AppKey]
                            FROM [dbo].[User] 
                            WHERE [UserId] = @userId AND [IsInvalid] = 1";
            using (var session = CreateSession())
            {
                session.SetQuery(query)
                    .AddParameter("@userId", userId);
                using (var reader = await session.ExecuteReaderAsync())
                {
                    return await ReadFromSession(session);
                }
            }
        }

        public async Task<User> GetByKeyToApp(string key, string appKey)
        {
            string query = @"SELECT 
                                [UserId],
                                [Key],
                                [Password],
	                            [CheckKey],
	                            [AppKey]
                            FROM [dbo].[User] 
                            WHERE [Key] = @key AND [AppKey] = @appKey AND [IsInvalid] = 1";
            using (var session = CreateSession())
            {
                session.SetQuery(query)
                    .AddParameter("@key", key)
                    .AddParameter("@appKey", appKey);
                return await ReadFromSession(session);
            }
        }

        private async Task<User> ReadFromSession(ISession session)
        {
            using (var reader = await session.ExecuteReaderAsync())
            {
                if (reader.Read())
                {
                    int i = 0;
                    return new User
                    {
                        UserId = reader.GetInt32(i++),
                        Key = reader.GetString(i++),
                        Password = reader.GetString(i++),
                        CheckKey = reader.GetString(i++),
                        AppKey = reader.GetString(i++)
                    };
                }
            }
            return User.Invalid<User>();
        }
    }
}