using Raique.Core.Exceptions;
using System;

namespace Raique.Common.HTTP.Hooks.Exceptions
{
    public class UninformedAppException : Exception, IBusinessException
    {
        public static Func<Exception> Creator = () => new UninformedAppException();
        private UninformedAppException() : base(Properties.Resources.UninformedAppException)
        { }
    }
}
