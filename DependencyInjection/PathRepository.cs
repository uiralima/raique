using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Raique.DependencyInjection
{
    public class PathRepository
    {
        public static void AddToResource(string path, string pattern, Dictionary<string, List<Type>> resource) =>
            new PathRepository(resource, path, pattern).AddPath();

        private readonly Dictionary<string, List<Type>> _resource;
        private readonly string _path;
        private readonly string _pattern;
        

        private PathRepository(Dictionary<string, List<Type>> resource, string path, string pattern)
        {
            _resource = resource;
            _path = path;
            _pattern = pattern;
        }
        
        private void AddPath()
        {
            GetAllLibrariesInPath(_path, _pattern)
                .Where(IsNotTestLibrary)
                .Select(LoadAssemblies)
                .Select(LoadAssymblyTypes)
                .Aggregate(Pushtypes)
                .ToList()
                .ForEach(f => RegsiterAllImplementations(f, _resource));
        }

        private string[] GetAllLibrariesInPath(string path, string pattern) =>
            String.IsNullOrWhiteSpace(pattern) ?
                System.IO.Directory.GetFiles(path) :
                System.IO.Directory.GetFiles(path, pattern);

        private Func<string, bool> IsNotTestLibrary = libraryName =>
            ((libraryName.IndexOf("Test.dll") < 0) || (libraryName.IndexOf("Tests.dll") < 0));

        private readonly Func<string, Assembly> LoadAssemblies = libraryName => Assembly.LoadFrom(libraryName);

        private readonly Func<Assembly, Type[]> LoadAssymblyTypes = assembly => assembly.GetTypes();

        private readonly Func<IEnumerable<Type>, IEnumerable<Type>, Type[]> Pushtypes = (start, next) =>
        {
            if (null == start)
            {
                start = new List<Type>();
            }
            return start.Union(next).ToArray();
        };

        private readonly Action<Type, Dictionary<string, List<Type>>> RegsiterAllImplementations = (type, resource) =>
        {
            RegisterType(type, resource);
            RegisterInterfaces(type, resource);
            RegisterBase(type, resource);
        };

        private static readonly Action<Type, Dictionary<string, List<Type>>> RegisterBase = (type, resource) =>
        {
            if ((null != type.BaseType) && (type.BaseType.IsAbstract))
            {
                PutTypeInResource(type.BaseType.FullName, type, resource);
            }
        };

        private static readonly Action<Type, Dictionary<string, List<Type>>> RegisterType = (type, resource) =>
        {
            if ((type.IsClass) && (!type.IsAbstract))
            {
                PutTypeInResource(type.FullName, type, resource);
            }
        };
        private static readonly Action<Type, Dictionary<string, List<Type>>> RegisterInterfaces = (type, resource) =>
        {
            foreach (var interf in type.GetInterfaces())
            {
                PutTypeInResource(interf.FullName, type, resource);
            }
        };

        private static readonly Action<string, Type, Dictionary<string, List<Type>>> PutTypeInResource = (key, value, resource) =>
        {
            if (!resource.ContainsKey(key))
            {
                resource.Add(key, new List<Type>());
            }
            resource[key].Add(value);
        };
    }
}
