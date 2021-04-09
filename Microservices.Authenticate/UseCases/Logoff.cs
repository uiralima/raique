using Raique.Library;
using Raique.Microservices.Authenticate.Protocols;
using System;
using System.Threading.Tasks;

namespace Raique.Microservices.Authenticate.UseCases
{
    public class Logoff
    {
        private readonly string _device;
        private readonly string _token;
        private readonly ITokenRepository _tokenRepository;

        public static async Task Execute(ITokenRepository tokenRepository,
            string token, string device) =>
            await new Logoff(tokenRepository, token, device).Do();
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
        private async Task Do()
        {
            await _tokenRepository.Close(_device, _token);
        }
    }
}
