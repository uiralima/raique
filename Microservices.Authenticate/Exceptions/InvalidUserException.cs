using Raique.Core.Exceptions;
using Raique.Microservices.Authenticate.Properties;
using System;

namespace Raique.Microservices.Authenticate.Exceptions
{
    public class InvalidUserException : Exception, IBusinessException, IDetailedException
    {
        public static InvalidUserException Create(int userId) => new InvalidUserException(userId, Resources.ExceptionInvalidUser);
        private readonly int _userId;

        private InvalidUserException(int userId, string message) : base(message)
        {
            _userId = userId;
        }
        public string GetDetail()
        {
            return $"UserId: {_userId}";
        }
    }
}
