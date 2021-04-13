using Raique.Core.Exceptions;

namespace Raique.DependencyInjection.Exception
{
    public class TypeLoadException : System.TypeLoadException, IBusinessException, IDetailedException
    {
        public static System.Exception CreateToType(string typeName) => new TypeLoadException(typeName);
        private readonly string _typeName;
        private TypeLoadException(string typeName) : base(string.Format(Properties.Resources.ExceptionTypeLoad, typeName))
        {
            _typeName = typeName;
        }
        public string GetDetail()
        {
            return $"Implementação não encontrada para {_typeName}";
        }
    }
}
