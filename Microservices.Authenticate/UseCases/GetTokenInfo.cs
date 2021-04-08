using Raique.Library;
using Raique.Microservices.Authenticate.Domain;
using Raique.Microservices.Authenticate.Exceptions;
using Raique.Microservices.Authenticate.Protocols;

namespace Raique.Microservices.Authenticate.UseCases
{
    public class GetTokenInfo
    {
        private readonly string _device;
        private readonly string _token;
        private readonly string _appKey;
        private readonly ITokenRepository _tokenRepository;
        private readonly IUserRepository _userRepository;

        public static User Execute(ITokenRepository tokenRepository, IUserRepository userRepository,
            string token, string appKey, string device) =>
            new GetTokenInfo(tokenRepository, userRepository, token, appKey, device).Do();
        private GetTokenInfo(ITokenRepository tokenRepository, IUserRepository userRepository,
            string token, string appKey, string device)
        {
            _device = device;
            _token = token;
            _appKey = appKey;
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
            ValidateInstance();
        }
        private void ValidateInstance()
        {
            _device.ThrowIfIsNullOrEmpty("device");
            _token.ThrowIfIsNullOrEmpty("token");
            _appKey.ThrowIfIsNullOrEmpty("appKey");
            _tokenRepository.ThrowIfNull("tokenRepository");
            _userRepository.ThrowIfNull("userRepository");
        }

        private User Do()
        {
            int userId = _tokenRepository.GetUserIdByToken(_device, _token);
            if (userId <= 0)
            {
                throw InvalidTokenException.Create(_token, _appKey, _device);
            }
            var user = _userRepository.GetById(userId);
            if (!user.IsValid())
            {
                throw InvalidTokenException.Create(_token, _appKey, _device);
            }
            if (!user.AppKey.Equals(_appKey))
            {
                throw InvalidTokenException.Create(_token, _appKey, _device);
            }
            return user;
        }
    }
}
