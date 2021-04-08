using Raique.Core.Exceptions;
using Raique.Microservices.Authenticate.Properties;
using System;

namespace Raique.Microservices.Authenticate.Exceptions
{
    public class InvalidUsernameOrPasswordException : Exception, IBusinessException, IDetailedException
    {
        public static Func<string, string, InvalidUsernameOrPasswordException> Create = (userName, appKey) =>
            new InvalidUsernameOrPasswordException(Resources.ExceptionInvalidUsernameOrPassword, userName, appKey);

        private readonly string _userName;
        private readonly string _appKey;

        private InvalidUsernameOrPasswordException(string message, string userName, string appKey) : base(message)
        {
            _userName = userName;
            _appKey = appKey;
        }

        public string GetDetail() => $"Usuário: {_userName} Aplicativo: {_appKey}";
    }
}
