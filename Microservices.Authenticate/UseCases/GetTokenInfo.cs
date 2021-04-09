using Raique.Library;
using Raique.Microservices.Authenticate.Domain;
using Raique.Microservices.Authenticate.Exceptions;
using Raique.Microservices.Authenticate.Protocols;
using System;
using System.Threading.Tasks;

namespace Raique.Microservices.Authenticate.UseCases
{
    public class GetTokenInfo
    {
        private readonly string _device;
        private readonly string _token;
        private readonly string _appKey;
        private readonly ITokenRepository _tokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly Func<Exception> InvalidTokenExceptionCreator;

        public static async Task<User> Execute(ITokenRepository tokenRepository, IUserRepository userRepository,
            string token, string appKey, string device) =>
            await new GetTokenInfo(tokenRepository, userRepository, token, appKey, device).Do();
        private GetTokenInfo(ITokenRepository tokenRepository, IUserRepository userRepository,
            string token, string appKey, string device)
        {
            _device = device;
            _token = token;
            _appKey = appKey;
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
            ValidateInstance();
            InvalidTokenExceptionCreator = () => InvalidTokenException.Create(_token, _appKey, _device);
        }
        private void ValidateInstance()
        {
            _device.ThrowIfIsNullOrEmpty("device");
            _token.ThrowIfIsNullOrEmpty("token");
            _appKey.ThrowIfIsNullOrEmpty("appKey");
            _tokenRepository.ThrowIfNull("tokenRepository");
            _userRepository.ThrowIfNull("userRepository");
        }

        private async Task<User> Do()
        {
            int userId = await _tokenRepository.GetUserIdByToken(_device, _token);
            InvalidTokenExceptionCreator.ThrowIfTrue(userId <= 0);
            var user = await _userRepository.GetById(userId);
            InvalidTokenExceptionCreator.ThrowIfFalse(user.IsValid());
            InvalidTokenExceptionCreator.ThrowIfFalse(user.AppKey.Equals(_appKey));
            return user;
        }
    }
}
