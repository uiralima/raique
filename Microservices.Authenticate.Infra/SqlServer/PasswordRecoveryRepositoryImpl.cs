using Raique.Database.SqlServer.Contracts;
using System.Threading.Tasks;

namespace Raique.Microservices.Authenticate.Infra.SqlServer
{
    public class PasswordRecoveryRepositoryImpl : Raique.Database.SqlServer.Contracts.Base,
        Raique.Microservices.Authenticate.Protocols.IPasswordRecoveryRepository
    {
        public PasswordRecoveryRepositoryImpl(IDatabaseConfig config) : base(config)
        {
        }

        public async Task Create(int userId, string code)
        {
            string query =
                    @"
                        DELETE PasswordRecovery WHERE CreatedDate < @limitDate OR [UserId] = @userId 
                        INSERT INTO PasswordRecovery (Code, UserId, CreatedDate) VALUES (@code, @userId, @createdDate)
                    ";
            using (var session = CreateSession())
            {
                session.SetQuery(query)
                    .AddParameter("@code", code)
                    .AddParameter("@userId", userId)
                    .AddParameter("@createdDate", Raique.Library.DateUtilities.Now)
                    .AddParameter("@limitDate", Raique.Library.DateUtilities.Now.AddDays(-2));
                await session.ExecuteNonQueryAsync();
            }
        }
    }
}
