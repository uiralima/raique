using Raique.Core.Exceptions;
using System;

namespace Raique.Common.HTTP.Hooks.Exceptions
{
    public class UninformedDeviceException : Exception, IBusinessException
    {
        public static Func<Exception> Creator = () => new UninformedDeviceException();
        private UninformedDeviceException() : base(Properties.Resources.UninformedDeviceException)
        { }
    }
}
