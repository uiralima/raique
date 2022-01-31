using Raique.Library;
using Raique.Microservices.Authenticate.Exceptions;
using Raique.Microservices.Authenticate.Protocols;
using Raique.Microservices.Authenticate.Services;
using System;
using System.Threading.Tasks;

namespace Raique.Microservices.Authenticate.UseCases
{
    public class Login
    {
        public static async Task<string> Execute(ITokenRepository tokenRepository,
            IUserRepository userRepository, ITokenCreator tokenCreator, string appKey, string userName, string password, string device) =>
               await new Login(tokenRepository, userRepository, tokenCreator, appKey, userName, password, device).Do();

        private readonly ITokenRepository _tokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITokenCreator _tokenCreator;
        private readonly string _appKey;
        private readonly string _userName;
        private readonly string _password;
        private readonly string _device;
        private readonly Func<Exception> InvalidUsernameOrPasswordExceptionCreator;

        private Login(ITokenRepository tokenRepository, IUserRepository userRepository, ITokenCreator tokenCreator, string appKey, string userName, string password, string device)
        {
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
            _appKey = appKey;
            _userName = userName;
            _password = password;
            _tokenCreator = tokenCreator;
            _device = device;
            ValidateInstance();
            InvalidUsernameOrPasswordExceptionCreator = () => InvalidUsernameOrPasswordException.Create(_userName, _appKey);
        }
        private void ValidateInstance()
        {
            _tokenRepository.ThrowIfNull("tokenRepository");
            _userRepository.ThrowIfNull("userRepository");
            _tokenCreator.ThrowIfNull("tokenCreator");
            _appKey.ThrowIfIsNullOrEmpty("appKey");
            _userName.ThrowIfIsNullOrEmpty("userName");
            _password.ThrowIfIsNullOrEmpty("password");
            _device.ThrowIfIsNullOrEmpty("device");
        }

        private async Task<string> Do()
        {
            var user = await _userRepository.GetByKeyToApp(_userName, _appKey);
            InvalidUsernameOrPasswordExceptionCreator.ThrowIfFalse(user.IsValid());
            string typedPassword = PasswordCreator.Create(_password, user.CheckKey);
            InvalidUsernameOrPasswordExceptionCreator.ThrowIfFalse(user.Password.Equals(typedPassword));
            user.Password = String.Empty;
            user.CheckKey = String.Empty;
            string token = _tokenCreator.Create(user, _device, _appKey);
            await _tokenRepository.Create(user.UserId, _device, token);
            return token;
        }
    }
}
