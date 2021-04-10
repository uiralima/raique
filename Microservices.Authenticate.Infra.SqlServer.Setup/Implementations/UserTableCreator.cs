using Raique.Database.SqlServer.Contracts;

namespace Raique.Microservices.Authenticate.Infra.SqlServer.Setup.Implementations
{
    public class UserTableCreator : Contracts.TableCreator
    {
        public UserTableCreator(IDatabaseConfig config) : base(config)
        {
        }
        public override string CreateTableQuery =>
            @"
                CREATE TABLE [dbo].[User](
	                [UserId] [int] NOT NULL,
	                [Key] [varchar](128) NOT NULL,
	                [Password] [varchar](128) NOT NULL,
	                [CheckKey] [varchar](128) NOT NULL,
	                [AppKey] [varchar](64) NOT NULL,
	                [IsInvalid] [bit] NOT NULL,
                 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
                (
	                [UserId] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                ) ON [PRIMARY]
                GO

                ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_App] FOREIGN KEY([AppKey])
                REFERENCES [dbo].[App] ([AppKey])
                GO

                ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_App]
             ";

        public override string TableName => "User";
    }
}
