using Raique.Microservices.Authenticate.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raique.Microservices.Authenticate.Exceptions
{
    public class UserNotExistsException : Exception
    {
        public static Func<UserNotExistsException> Create = () => new UserNotExistsException(Resources.ExceptionUserNotExists);

        private UserNotExistsException(string message) : base(message)
        { }
    }
}
