namespace Raique.Microservices.Authenticate.Protocols
{
    public interface ITokenInfoExtractor
    {
        (Domain.User, string, string) Extract(string token);
    }
}
