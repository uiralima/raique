using Raique.Database.SqlServer.Contracts;
using System;

namespace Raique.Microservices.Authenticate.Infra.SqlServer.Setup
{
    public class SqlGoDaddyConfig : IDatabaseConfig
    {
        public string Server => "198.71.225.113";

        public string Port => "1433";

        public string Database => "rqe_authenticate";

        public string UserName => "rqe_authenticate";

        public string Password => "^Mv85y8j";
    }
}
