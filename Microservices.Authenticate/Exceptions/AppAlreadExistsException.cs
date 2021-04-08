using Raique.Core.Exceptions;
using Raique.Microservices.Authenticate.Properties;
using System;

namespace Raique.Microservices.Authenticate.Exceptions
{
    public class AppAlreadExistsException : Exception, IBusinessException
    {
        public static Func<string, AppAlreadExistsException> Create = appName => new AppAlreadExistsException(String.Format(Resources.ExceptionAppAlreadExists, appName));

        private AppAlreadExistsException(string message) : base(message)
        { }
    }
}
