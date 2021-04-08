using Raique.Microservices.Authenticate.Protocols;

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
        public void Close(string device, string token)
        {
            CloseCount++;
        }

        public void Create(int userId, string device, string token)
        {
            CreateCount++;
        }

        public int GetUserIdByToken(string device, string token)
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
        }
    }
}
