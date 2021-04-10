using Raique.Database.SqlServer.Contracts;

namespace Raique.Microservices.Authenticate.Infra.SqlServer.Setup.Implementations
{
    public class TokenTableCreator : Contracts.TableCreator
    {
        public TokenTableCreator(IDatabaseConfig config) : base(config)
        {
        }
        public override string CreateTableQuery =>
            @"
                CREATE TABLE [dbo].[Token](
	                [TokenId] [int] IDENTITY(1,1) NOT NULL,
	                [TokenStr] [varchar](1024) NOT NULL,
	                [UserId] [int] NOT NULL,
	                [Device] [varchar](128) NOT NULL,
	                [CreatedDate] [datetime] NOT NULL,
	                [LastPing] [datetime] NOT NULL,
                 CONSTRAINT [PK_Token] PRIMARY KEY CLUSTERED 
                (
	                [TokenId] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                ) ON [PRIMARY]
                GO

                ALTER TABLE [dbo].[Token]  WITH CHECK ADD  CONSTRAINT [FK_Token_User] FOREIGN KEY([UserId])
                REFERENCES [dbo].[User] ([UserId])
                GO

                ALTER TABLE [dbo].[Token] CHECK CONSTRAINT [FK_Token_User]
             ";

        public override string TableName => "Token";
    }
}
