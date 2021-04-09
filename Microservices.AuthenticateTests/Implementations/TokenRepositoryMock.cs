using Raique.Microservices.Authenticate.Protocols;
using System.Threading.Tasks;

namespace Raique.Microservices.AuthenticateTests.Implementations
{
    public class TokenRepositoryMock : ITokenRepository
    {
        public static TokenRepositoryMock CreateRepository() => new TokenRepositoryMock();
        private TokenRepositoryMock()
        {
            CloseCount = 0;
            CreateCount = 0;
            GetUserIdByTokenCount = 0;
        }
        public int CloseCount { get; private set; }
        public int CreateCount { get; private set; }
        public int GetUserIdByTokenCount { get; private set; }
        public async Task Close(string device, string token)
        {
            await Task.Run(() => CloseCount++);
        }

        public async Task Create(int userId, string device, string token)
        {
            await Task.Run(() => CreateCount++);
        }

        public async Task<int> GetUserIdByToken(string device, string token)
        {
            return await Task.Run(() =>
            {
                GetUserIdByTokenCount++;
                if ((token == "TOKEN") && (device == "DEVICE"))
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            });
        }
    }
}
