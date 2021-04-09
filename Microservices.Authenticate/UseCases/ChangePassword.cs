using Raique.Library;
using Raique.Microservices.Authenticate.Exceptions;
using Raique.Microservices.Authenticate.Protocols;
using Raique.Microservices.Authenticate.Services;
using System;
using System.Threading.Tasks;

namespace Raique.Microservices.Authenticate.UseCases
{
    public class ChangePassword
    {
        public static async Task Execute(IUserRepository userRepository, int userId, string newPassword, string appKey) =>
            await new ChangePassword(userRepository, userId, newPassword, appKey).Do();

        private readonly IUserRepository _userRepository;
        private readonly string _newPassword;
        private readonly string _appKey;
        private readonly int _userId;
        private readonly Func<Exception> InvalidUserExceptionCreator;

        public IUserRepository UserRepository => _userRepository;

        private ChangePassword(IUserRepository userRepository, int userId, string newPassword, string appKey)
        {
            _userRepository = userRepository;
            _newPassword = newPassword;
            _appKey = appKey;
            _userId = userId;
            ValidateInstance();
            InvalidUserExceptionCreator = () => InvalidUserException.Create(_userId);
        }

        private void ValidateInstance()
        {
            _userRepository.ThrowIfNull("userRepository");
            _userId.ThrowIfZero("userId");
            _newPassword.ThrowIfIsNullOrEmpty("newPassword");
            _appKey.ThrowIfIsNullOrEmpty("appKey");
        }

        private async Task Do()
        {

            var user = await _userRepository.GetById(_userId);
            InvalidUserExceptionCreator.ThrowIfFalse(user.IsValid() && user.AppKey.Equals(_appKey));
            var newPassword = PasswordCreator.Create(_newPassword, user.CheckKey);
            await _userRepository.ChangePassword(_userId, newPassword);
        }

    }
}
