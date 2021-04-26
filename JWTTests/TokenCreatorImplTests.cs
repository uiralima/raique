using JWTTests.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raique.JWT;
using Raique.Microservices.Authenticate.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raique.JWT.Tests
{
    [TestClass]
    public class TokenCreatorImplTests
    {
        [TestMethod()]
        public void CreateTest()
        {
            try
            {
                TokenCreatorImpl impl = new TokenCreatorImpl(new JWTConfigMock());
                User user = new User
                {
                    UserId = 1,
                    Key = "User",
                    AppKey = "App"
                };
                string token = impl.Create(user, "device", "appKey");
            }
            catch
            {
                Assert.Fail();
            }
        }
    }
}