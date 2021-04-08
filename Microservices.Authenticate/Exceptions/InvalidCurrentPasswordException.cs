using Raique.Core.Exceptions;
using Raique.Microservices.Authenticate.Properties;
using System;

namespace Raique.Microservices.Authenticate.Exceptions
{
    public class InvalidCurrentPasswordException : Exception, IBusinessException
    {
        public static InvalidCurrentPasswordException Create() => new InvalidCurrentPasswordException(Resources.ExceptionInvalidCurrentPassword);
        private InvalidCurrentPasswordException(string message) : base(message)
        { }
    }
}
