using Raique.Library;
using Raique.Microservices.Authenticate.Exceptions;
using Raique.Microservices.Authenticate.Protocols;
using System;
using System.Threading.Tasks;

namespace Raique.Microservices.Authenticate.UseCases
{
    public class CreateUser
    {
        public static async Task<int> Execute(IAppRepository appRepository, IUserRepository userRepository,
            string appKey, string userName, string password) =>
            await new CreateUser(appRepository, userRepository, appKey, userName, password).Do();

        private readonly IAppRepository _appRepository;
        private readonly IUserRepository _userRepository;
        private readonly string _appKey;
        private readonly string _userName;
        private readonly string _password;
        private readonly Func<Exception> UserAlreadExistsExceptionCreator;
        private readonly Func<Exception> InvalidAppExceptionCreator;

        private CreateUser(IAppRepository appRepository, IUserRepository userRepository, string appKey, string userName, string password)
        {
            _appRepository = appRepository;
            _userRepository = userRepository;
            _appKey = appKey;
            _userName = userName;
            _password = password;
            ValidateInstance();
            UserAlreadExistsExceptionCreator = () => UserAlreadExistsException.Create(_userName);
            InvalidAppExceptionCreator = () => InvalidAppException.Create();
        }
        private void ValidateInstance()
        {
            _appRepository.ThrowIfNull("appRepository");
            _userRepository.ThrowIfNull("userRepository");
            _appKey.ThrowIfIsNullOrEmpty("appKey");
            _userName.ThrowIfIsNullOrEmpty("userName");
            _password.ThrowIfIsNullOrEmpty("password");
        }

        private async Task<int> Do()
        {
            UserAlreadExistsExceptionCreator.ThrowIfTrue(await UserNameAlreadExists());
            InvalidAppExceptionCreator.ThrowIfFalse(await AppAlreadExists());
            return await _userRepository.Create(CreateUserWithData(CreateRandomKey()));
        }

        private Domain.User CreateUserWithData(string checkKey) => new Domain.User
        {
            AppKey = _appKey,
            Key = _userName,
            CheckKey = checkKey,
            Password = Services.PasswordCreator.Create(_password, checkKey)
        };

        private async Task<bool> UserNameAlreadExists()
        {
            return (await _userRepository.GetByKeyToApp(_userName, _appKey)).IsValid();
        }

        private async Task<bool> AppAlreadExists()
        {
            return (await _appRepository.GetByKey(_appKey)).IsValid();
        }

        private string CreateRandomKey()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
