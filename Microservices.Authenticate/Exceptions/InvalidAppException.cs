using Raique.Core.Exceptions;
using Raique.Microservices.Authenticate.Properties;
using System;

namespace Raique.Microservices.Authenticate.Exceptions
{
    public class InvalidAppException : Exception, IBusinessException
    {
        public static Func<InvalidAppException> Create = () => new InvalidAppException(Resources.ExceptionInvalidApp);

        private InvalidAppException(string message) : base(message)
        { }
    }
}
