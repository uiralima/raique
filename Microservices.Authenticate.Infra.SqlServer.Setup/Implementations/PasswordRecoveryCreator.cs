using Raique.Database.SqlServer.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raique.Microservices.Authenticate.Infra.SqlServer.Setup.Implementations
{
    class PasswordRecoveryCreator : Contracts.TableCreator
    {
        public PasswordRecoveryCreator(IDatabaseConfig config) : base(config)
        {
        }
        public override string CreateTableQuery =>
           @"
                CREATE TABLE dbo.PasswordRecovery
	                (
	                Code varchar(12) NOT NULL,
	                UserId int NOT NULL,
	                CreatedDate datetime NOT NULL
	                )  ON [PRIMARY]
                GO
                ALTER TABLE dbo.PasswordRecovery ADD CONSTRAINT
	                PK_PasswordRecovery PRIMARY KEY CLUSTERED 
	                (
	                Code
	                ) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

                GO
                ALTER TABLE dbo.PasswordRecovery ADD CONSTRAINT
	                FK_PasswordRecovery_User FOREIGN KEY
	                (
	                UserId
	                ) REFERENCES dbo.[User]
	                (
	                UserId
	                ) ON UPDATE  NO ACTION 
	                 ON DELETE  NO ACTION 
	
                GO
                ALTER TABLE dbo.PasswordRecovery SET (LOCK_ESCALATION = TABLE)
                GO
            ";
        public override string TableName => "PasswordRecovery";
    }
}
