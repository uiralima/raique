using Raique.Microservices.Authenticate.Protocols;

namespace Raique.Microservices.Authenticate.Services
{
    public class CodeCreator : ICodeCreator
    {
        public string Create(int length, string validChars)
        {
            return Raique.Library.StringUtilities.GenerateRandom(length, validChars);
        }

        public string Create(int length)
        {
            return Raique.Library.StringUtilities.GenerateRandom(length);
        }
    }
}
