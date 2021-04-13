using System;
using System.Collections.Generic;

namespace Raique.DependencyInjection
{
    public static class Repository
    {
        private static object _lockResource = new object();
        private static readonly Dictionary<string, List<Type>> _resource = new Dictionary<string, List<Type>>();

        private static object _lockPreference = new object();
        private static ListRepository _preferenceRepository = new ListRepository();

        private static object _lockTransientTypes = new object();
        private static ExclusiveRepository _transientRepository = new ExclusiveRepository(Repository.ThrowIfTypeAlreadRegistred);

        private static object _lockSingletonTypes = new object();
        private static ExclusiveRepository _singletonRepository = new ExclusiveRepository(Repository.ThrowIfTypeAlreadRegistred);

        private static object _lockContextPreferences = new object();
        private static ContextListRepository _contextPreferenceRepository = new ContextListRepository();

        private static object _lockInstances = new object();
        private static readonly Dictionary<string, Dictionary<string, object>> _instances = new Dictionary<string, Dictionary<string, object>>();

        public static void Clear()
        {
            _resource.Clear();
            lock(_preferenceRepository)
            {
                _preferenceRepository.Clear();
            }
            lock (_lockTransientTypes)
            {
                _transientRepository.Clear();
            }
            lock(_lockContextPreferences)
            {
                _contextPreferenceRepository.Clear();
            }
            lock (_lockSingletonTypes)
            {
                _singletonRepository.Clear();
            }
            _instances.Clear();
        }

        public static void AddPath(string path, string pattern = null)
        {
            lock (_lockResource)
            {
                PathRepository.AddToResource(path, pattern, _resource);
            }
        }

        public static void SetPreferenceInContext<T, W>(string context)
        {
            lock (_lockContextPreferences)
            {
                _contextPreferenceRepository.SetType<T, W>(context);
            }
        }

        public static void SetPreference<T, W>()
        {
            lock (_lockPreference)
            {
                _preferenceRepository.SetType<T, W>();
            }
        }

        public static void SetTransiente<T, W>()
        {
            lock (_lockTransientTypes)
            {
                _transientRepository.SetType<T, W>();
            }
        }
        public static void SetSingleton<T, W>()
        {
            lock (_lockSingletonTypes)
            {
                _singletonRepository.SetType<T, W>();
            }
        }

        private static void ThrowIfTypeAlreadRegistred(Type type)
        {
            ThrowIfTypeAlreadSingletonException(type);
            ThrowIfTypeAlreadTransientException(type);
        }

        private static void ThrowIfTypeAlreadSingletonException(Type type)
        {
            if (_singletonRepository.IsTypeRegistred(type))
            {
                throw Exception.TypeAlreadSingletonException.CreateToType(type.FullName);
            }
        }

        private static void ThrowIfTypeAlreadTransientException(Type type)
        {
            if (_transientRepository.IsTypeRegistred(type))
            {
                throw Exception.TypeAlreadTransientException.CreateToType(type.FullName);
            }
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
            if (_singletonRepository.IsTypeRegistred(typename))
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
                if (_contextPreferenceRepository.IsTypeRegistred(typeName, context))
                {
                    return _contextPreferenceRepository.GetType(typeName, context);
                }
            }
            if (_preferenceRepository.IsTypeRegistred(typeName))
            {
                return _preferenceRepository.GetType(typeName);
            }
            if (_transientRepository.IsTypeRegistred(typeName))
            {
                return _transientRepository.GetType(typeName);
            }
            if (_singletonRepository.IsTypeRegistred(typeName))
            {
                return _singletonRepository.GetType(typeName);
            }
            if (_resource.ContainsKey(typeName))
            {
                return _resource[typeName][0];
            }
            throw Exception.TypeLoadException.CreateToType(typeName);
        }
    }
}
