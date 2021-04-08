using Raique.Library;
using Raique.Microservices.Authenticate.Domain;
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
            int userId = GetUserIdByToken();
            ThrowInvalidTokenExceptionIfTrue(userId.IsLessThanOrEqualZero());
            User user = GetUserById(userId);
            ThrowInvalidTokenExceptionIfFalse(user.IsValid());
            ThrowInvalidTokenExceptionIfFalse(user.AppKey.Equals(_appKey));
            string currentPassword = CreatePasswordHash(_currentPassword, user.CheckKey);
            ThrowInvalidCurrentPasswordExceptionIfDifferent(currentPassword, user.Password);
            string newPassword = CreatePasswordHash(_newPassword, user.CheckKey);
            ChangePassword(userId, newPassword);
        }
        private int GetUserIdByToken() => _tokenRepository.GetUserIdByToken(_device, _token);
        private User GetUserById(int userId) => _userRepository.GetById(userId);
        private void ThrowInvalidTokenExceptionIfFalse(bool compare) => ThrowInvalidTokenExceptionIfTrue(!compare);
        private void ThrowInvalidTokenExceptionIfTrue(bool compare)
        {
            if (compare)
            {
                ThrowInvalidTokenException();
            }
        }
        private void ThrowInvalidTokenException() => throw InvalidTokenException.Create(_token, _appKey, _device);
        private void ThrowInvalidCurrentPasswordExceptionIfDifferent(string password1, string password2)
        {
            if (!password1.Equals(password2))
            {
                ThrowInvalidCurrentPasswordException();
            }
        }
        private void ThrowInvalidCurrentPasswordException() => throw InvalidCurrentPasswordException.Create();
        private string CreatePasswordHash(string passord, string key) => PasswordCreator.Create(passord, key);
        private void ChangePassword(int userId, string password) => _userRepository.ChangePassword(userId, password);
    }
}
