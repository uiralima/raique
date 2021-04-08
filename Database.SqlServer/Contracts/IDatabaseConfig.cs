namespace Raique.Database.SqlServer.Contracts
{
    public interface IDatabaseConfig
    {
        public string Server { get; }
        public string Port { get; }
        public string Database { get; }
        public string UserName { get; }
        public string Password { get; }

    }
}
