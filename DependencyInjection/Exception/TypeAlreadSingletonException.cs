using Raique.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raique.DependencyInjection.Exception
{
    public class TypeAlreadSingletonException : System.TypeLoadException, IBusinessException, IDetailedException
    {
        public static System.Exception CreateToType(string typeName) => new TypeAlreadSingletonException(typeName);
        private readonly string _typeName;
        private TypeAlreadSingletonException(string typeName) : base(string.Format(Properties.Resources.ExceptionTypeAlreadSingleton, typeName))
        {
            _typeName = typeName;
        }
        public string GetDetail()
        {
            return $"O tipo {_typeName} já está marcado como singleton";
        }
    }
}
