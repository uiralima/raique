using Raique.Database.SqlServer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raique.Microservices.Authenticate.Infra.SqlServer.Setup.Contracts
{
    public abstract class TableCreator : Base, ITableCreator
    {
        protected TableCreator(IDatabaseConfig config) : base(config)
        {
        }
        public abstract string CreateTableQuery { get; }

        public abstract string TableName { get; }

        public async Task Create()
        {
            bool createTransaction = true;
            var commands = SepareCommand(CreateTableQuery);
            using(var session = CreateSession())
            {
                foreach (var command in commands)
                {
                    session.SetQuery(PutCommandInsideIf(command));
                    await session.ExecuteNonQueryAsync(createTransaction);
                    createTransaction = false;
                }
            }
        }

        private IEnumerable<string> SepareCommand(string command) => command.Split("GO").Where(f => !string.IsNullOrWhiteSpace(f));

        private string PutCommandInsideIf(string command) =>
           String.Format(@"IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') AND type in (N'U'))
                    BEGIN 
                        {1}
                    END", TableName, command);
    }
}
