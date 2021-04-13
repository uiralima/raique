using System;
using System.Collections.Generic;
using System.Text;

namespace Raique.DependencyInjection
{
    internal class ContextListRepository
    {
        private readonly Dictionary<string, Dictionary<string, Type>> _internalTypes = new Dictionary<string, Dictionary<string, Type>>();

        internal void Clear()
        {
            _internalTypes.Clear();
        }

        internal virtual void SetType<T, W>(string context)
        {
            string myContext = context ?? "*";
            string typeName = typeof(T).FullName;
            if (_internalTypes.ContainsKey(typeName))
            {
                if (_internalTypes[typeName].ContainsKey(myContext))
                {
                    _internalTypes[typeName][myContext] = typeof(W);
                }
                else
                {
                    _internalTypes[typeName].Add(myContext, typeof(W));
                }
            }
            else
            {
                _internalTypes.Add(typeName, new Dictionary<string, Type>());
                _internalTypes[typeName].Add(myContext, typeof(W));
            }
        }

        internal bool IsTypeRegistred(string typeFullname, string context)
        {
            string myContext = context ?? "*";
            return _internalTypes.ContainsKey(typeFullname) && _internalTypes[typeFullname].ContainsKey(myContext);
        }

        internal bool IsTypeRegistred(Type type, string context)
        {
            return IsTypeRegistred(type.FullName, context);
        }

        internal Type GetType(string typeName, string context)
        {
            return _internalTypes[typeName][context];
        }
    }
}
