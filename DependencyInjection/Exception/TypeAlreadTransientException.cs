using Raique.Core.Exceptions;

namespace Raique.DependencyInjection.Exception
{
    public class TypeAlreadTransientException : System.TypeLoadException, IBusinessException, IDetailedException
    {
        public static System.Exception CreateToType(string typeName) => new TypeAlreadTransientException(typeName);
        private readonly string _typeName;
        private TypeAlreadTransientException(string typeName) : base(string.Format(Properties.Resources.ExceptionTypeAlreadTransient, typeName))
        {
            _typeName = typeName;
        }
        public string GetDetail()
        {
            return $"O tipo {_typeName} já está marcado como transiente";
        }
    }
}
