using Raique.Microservices.Authenticate.Domain;
using Raique.Microservices.Authenticate.Protocols;

namespace Raique.Microservices.AuthenticateTests.Implementations
{
    public class TokenCreatorMock : ITokenCreator
    {
        public static TokenCreatorMock Create() => new TokenCreatorMock();
        public int CreateCount { get; private set; }
        private TokenCreatorMock()
        {
            CreateCount = 0;
        }
        public string Create(User user, string device, string appKey)
        {
            CreateCount++;
            return "TOKEN";
        }
    }
}
