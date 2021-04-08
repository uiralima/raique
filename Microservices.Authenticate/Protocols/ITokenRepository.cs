namespace Raique.Microservices.Authenticate.Protocols
{
    public interface ITokenRepository
    {
        void Create(int userId, string device, string token);
        int GetUserIdByToken(string device, string token);
        void Close(string device, string token);
    }
}
