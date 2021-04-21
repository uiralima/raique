using Raique.Core.Exceptions;
using System;

namespace Raique.Common.HTTP.Hooks.Exceptions
{
    public class InvalidUserException : Exception, IBusinessException
    {
        public static Func<Exception> Creator = () => new InvalidUserException();
        private InvalidUserException() : base(Properties.Resources.InvalidUserException)
        { }
    }
}
