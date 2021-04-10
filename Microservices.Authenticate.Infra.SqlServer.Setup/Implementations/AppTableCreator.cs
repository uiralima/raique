using Raique.Database.SqlServer.Contracts;
using System;
using System.Threading.Tasks;

namespace Raique.Microservices.Authenticate.Infra.SqlServer.Setup.Implementations
{
    public class AppTableCreator : Contracts.TableCreator
    {
        public AppTableCreator(IDatabaseConfig config) : base(config)
        {
        }
        public override string CreateTableQuery =>
            @"
                CREATE TABLE [dbo].[App](
	                [AppKey] [varchar](64) NOT NULL,
	                [Fullname] [varchar](64) NOT NULL,
	                [IsInvalid] [bit] NOT NULL,
                 CONSTRAINT [PK_App_1] PRIMARY KEY CLUSTERED 
                (
	                [AppKey] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                ) ON [PRIMARY]
             ";

        public override string TableName => "App";
    }
}
