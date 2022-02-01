using Raique.Library;
using Raique.Microservices.Authenticate.Exceptions;
using Raique.Microservices.Authenticate.Protocols;
using System;
using System.Threading.Tasks;

namespace Raique.Microservices.Authenticate.UseCases
{
    public class CreatePasswordRecovery
    {
        public static async Task<string> Execute(IPasswordRecoveryRepository passwordRecoveryRepository, IUserRepository userRepository, ICodeCreator codeCreator, string userName, string appKey) =>
            await new CreatePasswordRecovery(passwordRecoveryRepository, userRepository, codeCreator, userName, appKey).Do();

        private readonly IPasswordRecoveryRepository _passwordRecoveryRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICodeCreator _codeCreator;
        private readonly string _userName;
        private readonly string _appKey;
        private readonly Func<Exception> UserNotExistsExceptionCreator;
        
        private CreatePasswordRecovery(IPasswordRecoveryRepository passwordRecoveryRepository, IUserRepository userRepository, ICodeCreator codeCreator, string userName, string appKey)
        {
            _passwordRecoveryRepository = passwordRecoveryRepository;
            _userRepository = userRepository;
            _codeCreator = codeCreator;
            _userName = userName;
            _appKey = appKey;
            ValidateInstance();
            UserNotExistsExceptionCreator = () => UserNotExistsException.Create();
        }

        private void ValidateInstance()
        {
            _passwordRecoveryRepository.ThrowIfNull("passwordRecoveryRepository");
            _userRepository.ThrowIfNull("userRepository");
            _codeCreator.ThrowIfNull("codeCreator");
            _userName.ThrowIfIsNullOrEmpty("userName");
            _appKey.ThrowIfIsNullOrEmpty("appKey");
        }

        private async Task<string> Do()
        {
            var user = await _userRepository.GetByKeyToApp(_userName, _appKey);
            UserNotExistsExceptionCreator.ThrowIfFalse(user.IsValid());
            string code = _codeCreator.Create(7, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");
            await _passwordRecoveryRepository.Create(user.UserId, code);
            return code;
        }
    }
}
