namespace Raique.Database.SqlServer.Contracts
{
    public interface IDatabaseConfig
    {
        string Server { get; }
        string Port { get; }
        string Database { get; }
        string UserName { get; }
        string Password { get; }
    }
}
