using System;
using System.Collections.Generic;
using System.Text;

namespace Raique.DependencyInjection
{
    internal class ExclusiveRepository : ListRepository
    {
        private Action<Type> _throwIfTypeAlreadRegistred;
        internal ExclusiveRepository(Action<Type> throwIfTypeAlreadRegistred)
        {
            _throwIfTypeAlreadRegistred = throwIfTypeAlreadRegistred;
        }
        internal override void SetType<T, W>()
        {
            _throwIfTypeAlreadRegistred(typeof(T));
            base.SetType<T, W>();
        }
    }
}
