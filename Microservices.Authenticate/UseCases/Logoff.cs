using Raique.Library;
using Raique.Microservices.Authenticate.Protocols;
using System;

namespace Raique.Microservices.Authenticate.UseCases
{
    public class Logoff
    {
        private readonly string _device;
        private readonly string _token;
        private readonly ITokenRepository _tokenRepository;

        public static void Execute(ITokenRepository tokenRepository,
            string token, string device) =>
            new Logoff(tokenRepository, token, device).Do();
        public Logoff(ITokenRepository tokenRepository, string token, string device)
        {
            _device = device;
            _token = token;
            _tokenRepository = tokenRepository;
            ValidateInstance();
        }
        private void ValidateInstance()
        {
            _tokenRepository.ThrowIfNull("tokenRepository");
            _device.ThrowIfIsNullOrEmpty("device");
            _token.ThrowIfIsNullOrEmpty("token");
        }
        private void Do()
        {
            _tokenRepository.Close(_device, _token);
        }
    }
}
