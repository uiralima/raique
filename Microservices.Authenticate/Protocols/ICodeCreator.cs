namespace Raique.Microservices.Authenticate.Protocols
{
    public interface ICodeCreator
    {
        string Create(int length, string validChars);
        string Create(int length);
    }
}
