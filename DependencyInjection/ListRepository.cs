using System;
using System.Collections.Generic;
using System.Text;

namespace Raique.DependencyInjection
{
    internal class ListRepository
    {
        private readonly Dictionary<string, Type> _internalTypes = new Dictionary<string, Type>();

        internal void Clear()
        {
            _internalTypes.Clear();
        }

        internal virtual void SetType<T, W>()
        {
            _internalTypes.Add(typeof(T).FullName, typeof(W));
        }

        internal bool IsTypeRegistred(string typeFullname)
        {
            return _internalTypes.ContainsKey(typeFullname);
        }

        internal bool IsTypeRegistred(Type type)
        {
            return IsTypeRegistred(type.FullName);
        }

        internal Type GetType(string typeName)
        {
            return _internalTypes[typeName];
        }
    }
}
