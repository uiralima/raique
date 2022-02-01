using Raique.Library;
using Raique.Microservices.Authenticate.Protocols;
using Raique.Microservices.Authenticate.Services;
using System.Threading.Tasks;

namespace Raique.Microservices.Authenticate.UseCases
{
    public class ChangePasswordWithCode
    {
        public static async Task<bool> Execute(IUserRepository userRepository,
            string userName, string code, string password, string appKey) =>
            await new ChangePasswordWithCode(userRepository, userName, code, password, appKey).Do();

        private readonly IUserRepository _userRepository;
        private readonly string _userName;
        private readonly string _password;
        private readonly string _appKey;
        private readonly string _code;

        private ChangePasswordWithCode(IUserRepository userRepository, string userName, string code, string password, string appKey)
        {
            _userRepository = userRepository;
            _userName = userName;
            _password = password;
            _appKey = appKey;
            _code = code;
            ValidateInstance();
        }

        private void ValidateInstance()
        {
            _userRepository.ThrowIfNull("userRepository");
            _userName.ThrowIfIsNullOrEmpty("userName");
            _password.ThrowIfIsNullOrEmpty("password");
            _code.ThrowIfIsNullOrEmpty("code");
        }

        private async Task<bool> Do()
        {
            var user = await _userRepository.GetByKeyToApp(_userName, _appKey);
            if (!user.IsValid())
            {
                return false;
            }
            var newPassword = PasswordCreator.Create(_password, user.CheckKey);
            return await _userRepository.ChangePasswordByCode(_userName, _code, newPassword, _appKey);
        }
    }
}
