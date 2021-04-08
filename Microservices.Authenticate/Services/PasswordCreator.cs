using Raique.Library;

namespace Raique.Microservices.Authenticate.Services
{
    public class PasswordCreator
    {
        public static string Create(string password, string checkKey)
        {
            return $"{password}_{checkKey}".Hash();
        }

    }
}
