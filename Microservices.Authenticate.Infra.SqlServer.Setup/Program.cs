using Raique.Database.SqlServer.Contracts;
using Raique.Microservices.Authenticate.Infra.SqlServer.Setup.Contracts;
using Raique.Microservices.Authenticate.Infra.SqlServer.Setup.Implementations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Raique.Microservices.Authenticate.Infra.SqlServer.Setup
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IDatabaseConfig config = new SqlGoDaddyConfig();
            List<ITableCreator> creators = new List<ITableCreator>();
            creators.Add(new AppTableCreator(config));
            creators.Add(new UserTableCreator(config));
            creators.Add(new TokenTableCreator(config));

            foreach(var creator in creators)
            {
                Console.WriteLine($"Criando tabela {creator.TableName}...");
                await creator.Create();
                Console.WriteLine($"Tabela {creator.TableName} criada!");
            }
        }


    }
}
