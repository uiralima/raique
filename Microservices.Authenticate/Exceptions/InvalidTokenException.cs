using Raique.Core.Exceptions;
using Raique.Microservices.Authenticate.Properties;
using System;

namespace Raique.Microservices.Authenticate.Exceptions
{
    public class InvalidTokenException : Exception, IBusinessException, IDetailedException
    {
        public static InvalidTokenException Create(string token, string appKey, string device) =>
            new InvalidTokenException(Resources.ExceptionInvalidToken, token, appKey, device);

        private readonly string _token;
        private readonly string _appKey;
        private readonly string _device;

        private InvalidTokenException(string message, string token, string appKey, string device) : base(message)
        {
            _token = token;
            _appKey = appKey;
            _device = device;
        }

        public string GetDetail() => $"Token: {_token} Aplicativo: {_appKey} Device: {_device}";
    }
}
