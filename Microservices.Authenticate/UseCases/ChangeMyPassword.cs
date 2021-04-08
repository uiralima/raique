using Raique.Library;
using Raique.Microservices.Authenticate.Exceptions;
using Raique.Microservices.Authenticate.Protocols;
using Raique.Microservices.Authenticate.Services;
using System;

namespace Raique.Microservices.Authenticate.UseCases
{
    public class ChangeMyPassword
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly string _token;
        private readonly string _currentPassword;
        private readonly string _newPassword;
        private readonly string _appKey;
        private readonly string _device;

        public static void Execute(ITokenRepository tokenRepository, IUserRepository userRepository,
            string token, string currentPassword, string newPassword, string appKey, string device) =>
            new ChangeMyPassword(tokenRepository, userRepository, token, currentPassword, newPassword, appKey, device).Do();
        private ChangeMyPassword(ITokenRepository tokenRepository, IUserRepository userRepository,
            string token, string currentPassword, string newPassword, string appKey, string device)
        {
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
            _token = token;
            _currentPassword = currentPassword;
            _newPassword = newPassword;
            _appKey = appKey;
            _device = device;
            ValidateInstance();
        }
        private void ValidateInstance()
        {
            _tokenRepository.ThrowIfNull("tokenRepository");
            _userRepository.ThrowIfNull("userRepository");
            _appKey.ThrowIfIsNullOrEmpty("appKey");
            _token.ThrowIfIsNullOrEmpty("token");
            _currentPassword.ThrowIfIsNullOrEmpty("currentPassword");
            _newPassword.ThrowIfIsNullOrEmpty("newPassword");
            _appKey.ThrowIfIsNullOrEmpty("appKey");
            _device.ThrowIfIsNullOrEmpty("device");
        }
        private void Do()
        {
            var userId = _tokenRepository.GetUserIdByToken(_device, _token);
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
            string currentPassword = PasswordCreator.Create(_currentPassword, user.CheckKey);
            if (!currentPassword.Equals(user.Password))
            {
                throw InvalidCurrentPasswordException.Create();
            }
            string newPassword = PasswordCreator.Create(_newPassword, user.CheckKey);
            _userRepository.ChangePassword(userId, newPassword);
        }
    }
}
