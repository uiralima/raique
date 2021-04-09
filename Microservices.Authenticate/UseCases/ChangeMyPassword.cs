using Raique.Library;
using Raique.Microservices.Authenticate.Domain;
using Raique.Microservices.Authenticate.Exceptions;
using Raique.Microservices.Authenticate.Protocols;
using Raique.Microservices.Authenticate.Services;
using System;
using System.Threading.Tasks;

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
        private readonly Func<Exception> InvalidTokenExceptionCreator;
        private readonly Func<Exception> InvalidCurrentPasswordExceptionCreator;

        public static async Task Execute(ITokenRepository tokenRepository, IUserRepository userRepository,
            string token, string currentPassword, string newPassword, string appKey, string device) =>
            await new ChangeMyPassword(tokenRepository, userRepository, token, currentPassword, newPassword, appKey, device).Do();
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
            InvalidTokenExceptionCreator = () => InvalidTokenException.Create(_token, _appKey, _device);
            InvalidCurrentPasswordExceptionCreator = () => InvalidCurrentPasswordException.Create();
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
        private async Task Do()
        {
            int userId = await GetUserIdByToken();
            InvalidTokenExceptionCreator.ThrowIfTrue(userId.IsLessThanOrEqualZero());
            User user = await GetUserById(userId);
            InvalidTokenExceptionCreator.ThrowIfFalse(user.IsValid() && user.AppKey.Equals(_appKey));
            string currentPassword = CreatePasswordHash(_currentPassword, user.CheckKey);
            InvalidCurrentPasswordExceptionCreator.ThrowIfFalse(currentPassword.Equals(user.Password));
            string newPassword = CreatePasswordHash(_newPassword, user.CheckKey);
            await ChangePassword(userId, newPassword);
        }
        private async Task<int> GetUserIdByToken() => await _tokenRepository.GetUserIdByToken(_device, _token);
        private async Task<User> GetUserById(int userId) => await _userRepository.GetById(userId);
        private string CreatePasswordHash(string passord, string key) => PasswordCreator.Create(passord, key);
        private async Task ChangePassword(int userId, string password) => await _userRepository.ChangePassword(userId, password);
    }
}
