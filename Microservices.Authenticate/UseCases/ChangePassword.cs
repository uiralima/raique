using Raique.Library;
using Raique.Microservices.Authenticate.Exceptions;
using Raique.Microservices.Authenticate.Protocols;
using Raique.Microservices.Authenticate.Services;

namespace Raique.Microservices.Authenticate.UseCases
{
    public class ChangePassword
    {
        public static void Execute(IUserRepository userRepository, int userId, string newPassword, string appKey) =>
            new ChangePassword(userRepository, userId, newPassword, appKey).Do();

        private readonly IUserRepository _userRepository;
        private readonly string _newPassword;
        private readonly string _appKey;
        private readonly int _userId;

        private ChangePassword(IUserRepository userRepository, int userId, string newPassword, string appKey)
        {
            _userRepository = userRepository;
            _newPassword = newPassword;
            _appKey = appKey;
            _userId = userId;
            ValidateInstance();
        }

        private void ValidateInstance()
        {
            _userRepository.ThrowIfNull("userRepository");
            _userId.ThrowIfZero("userId");
            _newPassword.ThrowIfIsNullOrEmpty("newPassword");
            _appKey.ThrowIfIsNullOrEmpty("appKey");
        }

        private void Do()
        {
            var user = _userRepository.GetById(_userId);
            if (!user.IsValid())
            {
                throw InvalidUserException.Create(_userId);
            }
            if (!user.AppKey.Equals(_appKey))
            {
                throw InvalidUserException.Create(_userId);
            }
            var newPassword = PasswordCreator.Create(_newPassword, user.CheckKey);
            _userRepository.ChangePassword(_userId, newPassword);
        }
    }
}
