using Raique.Core.Exceptions;
using Raique.Microservices.Authenticate.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raique.Microservices.Authenticate.Exceptions
{
    public class UserAlreadExistsException : Exception, IBusinessException
    {
        public static Func<string, UserAlreadExistsException> Create = userName => new UserAlreadExistsException(String.Format(Resources.ExceptionUserAlreadExists, userName));

        private UserAlreadExistsException(string message) : base(message)
        { }
    }
}
