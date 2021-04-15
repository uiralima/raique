using Raique.JWT.Protocols;

namespace JWTTests.Implementations
{
    public class JWTConfigMock : IJWTConfig
    {
        public int Expires => 0;

        public string Secret => "c9824df439ba43b4bec77d3bb4910e1e";
    }
}
