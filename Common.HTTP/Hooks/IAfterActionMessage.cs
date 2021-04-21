using System;

namespace Raique.Common.HTTP.Hooks
{
    public interface IAfterActionMessage : IMessage
    {
        Exception Exception { get; }
        void ClearException();
    }
}
