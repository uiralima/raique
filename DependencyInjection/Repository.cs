using System;
using System.Collections.Generic;

namespace Raique.DependencyInjection
{
    public static class Repository
    {
        private static object _lockResource = new object();
        private static readonly Dictionary<string, List<Type>> _resource = new Dictionary<string, List<Type>>();

        private static object _lockPreference = new object();
        private static readonly Dictionary<string, Type> _preference = new Dictionary<string, Type>();

        private static object _lockTransientTypes = new object();
        private static readonly Dictionary<string, Type> _transientTypes = new Dictionary<string, Type>();

        private static object _lockContextPreferences = new object();
        private static readonly Dictionary<string, Dictionary<string, Type>> _contextPreference = new Dictionary<string, Dictionary<string, Type>>();

        private static object _lockSingletonTypes = new object();
        private static readonly Dictionary<string, Type> _singletonTypes = new Dictionary<string, Type>();

        private static object _lockInstances = new object();
        private static readonly Dictionary<string, Dictionary<string, object>> _instances = new Dictionary<string, Dictionary<string, object>>();

        public static void Clear()
        {
            _resource.Clear();
            _preference.Clear();
            _transientTypes.Clear();
            _contextPreference.Clear();
            _singletonTypes.Clear();
            _instances.Clear();
        }
        public static void AddPath(string path, string pattern = null)
        {
            lock (_lockResource)
            {
                PathRepository.AddToResource(path, pattern, _resource);
            }
        }

        public static void SetPreference<T>(Type preferedType)
        {
            string typeName = typeof(T).FullName;
            lock (_lockPreference)
            {
                if (_preference.ContainsKey(typeName))
                {
                    _preference[typeName] = preferedType;
                }
                else
                {
                    _preference.Add(typeName, preferedType);
                }
            }
        }

        public static void SetTransiente<T, W>()
        {
            var transientType = typeof(T);
            var implementation = typeof(W);
            lock (_lockTransientTypes)
            {
                ThrowIfTypeAlreadRegistred(transientType);
                _transientTypes.Add(transientType.FullName, implementation);
            }
        }
        public static void SetSingleton<T, W>()
        {
            var transientType = typeof(T);
            var implementation = typeof(W);
            lock (_lockTransientTypes)
            {
                ThrowIfTypeAlreadRegistred(transientType);
                _singletonTypes.Add(transientType.FullName, implementation);
            }
        }

        private static void ThrowIfTypeAlreadRegistred(Type type)
        {
            ThrowIfTypeAlreadSingletonException(type);
            ThrowIfTypeAlreadTransientException(type);
        }

        private static void ThrowIfTypeAlreadSingletonException(Type type)
        {
            if (IsTypeSingleton(type))
            {
                throw Exception.TypeAlreadSingletonException.CreateToType(type.FullName);
            }
        }
        private static void ThrowIfTypeAlreadTransientException(Type type)
        {
            if (IsTypeTransiente(type))
            {
                throw Exception.TypeAlreadTransientException.CreateToType(type.FullName);
            }
        }

        private static bool IsTypeTransiente(Type type)
        {
            return IsTypeTransiente(type.FullName);
        }
        private static bool IsTypeTransiente(string typeFullname)
        {
            return _transientTypes.ContainsKey(typeFullname);
        }

        private static bool IsTypeSingleton(Type type)
        {
            return IsTypeSingleton(type.FullName);
        }

        private static bool IsTypeSingleton(string typeFullname)
        {
            return _singletonTypes.ContainsKey(typeFullname);
        }

        public static void SetPreferenceInContext<T>(Type preferedType, string context)
        {
            string typeName = typeof(T).FullName;
            lock (_lockContextPreferences)
            {
                if (_contextPreference.ContainsKey(typeName))
                {
                    if (_contextPreference[typeName].ContainsKey(context))
                    {
                        _contextPreference[typeName][context] = preferedType;
                    }
                    else
                    {
                        _contextPreference[typeName].Add(context, preferedType);
                    }
                }
                else
                {
                    _contextPreference.Add(typeName, new Dictionary<string, Type>());
                    _contextPreference[typeName].Add(context, preferedType);
                }
            }
        }

        private static object CreateType(Type type, string context)
        {
            var constructor = type.GetConstructors()[0];
            var parameters = constructor.GetParameters();
            List<object> p = new List<object>();
            foreach (var parameter in parameters)
            {
                p.Add(CreateInstanceInContext(parameter.ParameterType.FullName, context));
            }
            return constructor.Invoke(p.ToArray());
        }
        public static object CreateInstance(string typename)
        {
            return CreateInstanceInContext(typename, null);
        }
        public static T CreateInstance<T>() where T : class
        {
            string typename = typeof(T).FullName;
            return (T)CreateInstanceInContext(typename, null);
        }
        public static T CreateInstanceInContext<T>(string context) where T : class
        {
            string typename = typeof(T).FullName;
            return (T)CreateInstanceInContext(typename, context);
        }
        public static object CreateInstanceInContext(string typename, string context)
        {
            Type type = GetType(typename, context);
            if (IsTypeSingleton(typename))
            {
                if (!IsTypeInCache(type, context))
                {
                    var instance = CreateType(type, context);
                    AddTypeInCanche(type, context, instance);
                }
                return GetTypeInCache(type, context);
            }
            else
            {
                return CreateType(type, context);
            }
        }

        private static bool IsTypeInCache(Type type, string context)
        {
            string myContext = context ?? "*";
            string typename = type.FullName;
            return ((_instances.ContainsKey(typename)) && (_instances[typename].ContainsKey(myContext)));
        }

        private static void AddTypeInCanche(Type type, string context, object instance)
        {
            string myContext = context ?? "*";
            string typename = type.FullName;
            lock (_lockInstances)
            {
                if (_instances.ContainsKey(typename))
                {
                    if (_instances[typename].ContainsKey(myContext))
                    {
                        _instances[typename][myContext] = instance;
                    }
                    else
                    {
                        _instances[typename].Add(myContext, instance);
                    }
                }
                else
                {
                    _instances.Add(typename, new Dictionary<string, object>());
                    _instances[typename].Add(myContext, instance);
                }
            }
        }

        private static object GetTypeInCache(Type type, string context)
        {
            string myContext = context ?? "*";
            string typename = type.FullName;
            return _instances[typename][myContext];
        }

        private static Type GetType(string typeName, string context = null)
        {
            if (!string.IsNullOrWhiteSpace(context))
            {
                if ((_contextPreference.ContainsKey(typeName)) && (_contextPreference[typeName].ContainsKey(context)))
                {
                    return _contextPreference[typeName][context];
                }
            }
            if (_preference.ContainsKey(typeName))
            {
                return _preference[typeName];
            }
            if (IsTypeTransiente(typeName))
            {
                return _transientTypes[typeName];
            }
            if (IsTypeSingleton(typeName))
            {
                return _singletonTypes[typeName];
            }
            if (_resource.ContainsKey(typeName))
            {
                return _resource[typeName][0];
            }
            throw Exception.TypeLoadException.CreateToType(typeName);
        }
    }
}
