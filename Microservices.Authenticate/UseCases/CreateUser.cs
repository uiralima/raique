using Raique.Library;
using Raique.Microservices.Authenticate.Exceptions;
using Raique.Microservices.Authenticate.Protocols;
using System;

namespace Raique.Microservices.Authenticate.UseCases
{
    public class CreateUser
    {
        public static int Execute(IAppRepository appRepository, IUserRepository userRepository,
            string appKey, string userName, string password) =>
            new CreateUser(appRepository, userRepository, appKey, userName, password).Do();

        private readonly IAppRepository _appRepository;
        private readonly IUserRepository _userRepository;
        private readonly string _appKey;
        private readonly string _userName;
        private readonly string _password;

        private CreateUser(IAppRepository appRepository, IUserRepository userRepository, string appKey, string userName, string password)
        {
            _appRepository = appRepository;
            _userRepository = userRepository;
            _appKey = appKey;
            _userName = userName;
            _password = password;
            ValidateInstance();
        }
        private void ValidateInstance()
        {
            _appRepository.ThrowIfNull("appRepository");
            _userRepository.ThrowIfNull("userRepository");
            _appKey.ThrowIfIsNullOrEmpty("appKey");
            _userName.ThrowIfIsNullOrEmpty("userName");
            _password.ThrowIfIsNullOrEmpty("password");
        }

        private int Do()
        {
            if (UserNameAlreadExists())
            {
                throw UserAlreadExistsException.Create(_userName);
            }
            if (!AppAlreadExists())
            {
                throw InvalidAppException.Create();
            }
            string checkKey = CreateRandomKey();
            return _userRepository.Create(new Domain.User
            {
                AppKey = _appKey,
                Key = _userName,
                CheckKey = checkKey,
                Password = Services.PasswordCreator.Create(_password, checkKey)
            });
        }

        private bool UserNameAlreadExists()
        {
            return _userRepository.GetByKeyToApp(_userName, _appKey).IsValid();
        }

        private bool AppAlreadExists()
        {
            return _appRepository.GetByKey(_appKey).IsValid();
        }

        private string CreateRandomKey()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
