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

        public async Task<bool> Create()
        {
            if (!await TableExists())
            {
                bool createTransaction = true;
                var commands = SepareCommand(CreateTableQuery);
                using (var session = CreateSession())
                {
                    foreach (var command in commands)
                    {
                        session.SetQuery(command);
                        await session.ExecuteNonQueryAsync(createTransaction);
                        createTransaction = false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private IEnumerable<string> SepareCommand(string command) => command.Split("GO").Where(f => !string.IsNullOrWhiteSpace(f));

        private async Task<bool> TableExists()
        {
            string query = String.Format(@"IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') AND type in (N'U'))
                    BEGIN 
                        SELECT CAST(0 AS BIT)
                    END
                    ELSE
                    BEGIN
                        SELECT CAST(1 AS BIT)
                    END", TableName);
            using(var session = CreateSession())
            {
                session.SetQuery(query);
                using(var reader = await session.ExecuteReaderAsync())
                {
                    return ((reader.Read()) && (reader.GetBoolean(0)));
                }
            }
        }
    }
}
