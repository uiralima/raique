using Raique.Microservices.Authenticate.Domain;
using Raique.Microservices.Authenticate.Protocols;
using System;

namespace Raique.Microservices.AuthenticateTests.Implementations
{
    public class TokenInfoExtractorMock : ITokenInfoExtractor
    {
        public static TokenInfoExtractorMock Create() => new TokenInfoExtractorMock();
        public (User, string, string) Extract(string token)
        {
            return (new User
            {
                UserId = 1,
                AppKey = "CHAVE",
                Key = "key"
            },
            "DEVICE", "CHAVE");
        }
    }
}
