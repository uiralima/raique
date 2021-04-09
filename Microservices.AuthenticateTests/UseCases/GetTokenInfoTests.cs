using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raique.Microservices.Authenticate.Exceptions;
using Raique.Microservices.Authenticate.UseCases;
using Raique.Microservices.AuthenticateTests.Implementations;
using System;
using System.Threading.Tasks;

namespace Raique.Microservices.AuthenticateTests.UseCases
{
    [TestClass()]
    public class GetTokenInfoTests
    {
        [DataRow(true, false, "TOKEN", "KEY", "DEVICE", DisplayName = "Token Repository Null")]
        [DataRow(false, true, "TOKEN", "KEY", "DEVICE", DisplayName = "User Repository Null")]
        [DataRow(false, false, "", "KEY", "DEVICE", DisplayName = "Token Empty")]
        [DataRow(false, false, "TOKEN", "", "DEVICE", DisplayName = "appKey Empty")]
        [DataRow(false, false, "TOKEN", "KEY", "", DisplayName = "device Empty")]
        [TestMethod()]
        public async Task GetTokenInfoInvalidParametersTest(bool isTokenRepNull, bool isUserRepNull, string token, string appKey, string device)
        {
            var tokenRep = isTokenRepNull ? null : TokenRepositoryMock.CreateRepository();
            var userRep = isUserRepNull ? null : UserRepositoryMock.CreateRepository();
            try
            {
                await GetTokenInfo .Execute(tokenRep, userRep, token, appKey, device);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                if (isTokenRepNull || isUserRepNull)
                    Assert.AreEqual(ex.GetType(), typeof(ArgumentNullException));
                else
                    Assert.AreEqual(ex.GetType(), typeof(ArgumentException));
            }
        }

        [DataRow("TOKEN", "CHAVE", "DEVICE", true, true)]
        [DataRow("TOKEN_INVALIDO", "CHAVE", "DEVICE", true, false)]
        [DataRow("TOKEN", "CHAVE_INVALIDA", "DEVICE", true, true)]
        [DataRow("TOKEN", "CHAVE", "DEVICE_INVALIDO", true, false)]
        [TestMethod()]
        public async Task CreateUserFlowTest(string token, string appKey, string device, bool callGetUserIdByTokenCount, bool callGetByIdCount)
        {
            var tokenRep = TokenRepositoryMock.CreateRepository();
            var userRep = UserRepositoryMock.CreateRepository();
            try
            {
                await GetTokenInfo .Execute(tokenRep, userRep, token, appKey, device);
                Assert.AreEqual(userRep.GetByIdCount, callGetByIdCount ? 1 : 0);
                Assert.AreEqual(tokenRep.GetUserIdByTokenCount, callGetUserIdByTokenCount ? 1 : 0);
            }
            catch
            {
                Assert.AreEqual(userRep.GetByIdCount, callGetByIdCount ? 1 : 0);
                Assert.AreEqual(tokenRep.GetUserIdByTokenCount, callGetUserIdByTokenCount ? 1 : 0);
            }
        }

        [DataRow("TOKEN", "CHAVE", "DEVICE", false, DisplayName = "Token válido")]
        [DataRow("TOKEN_INVALIDO", "CHAVE", "DEVICE", true, typeof(InvalidTokenException), DisplayName = "Token inválido")]
        [DataRow("TOKEN", "CHAVE_INVALIDA", "DEVICE", true, typeof(InvalidTokenException), DisplayName = "Chave do app inválida")]
        [DataRow("TOKEN", "CHAVE", "DEVICE_INVALIDO", true, typeof(InvalidTokenException), DisplayName = "Device inválido")]
        [TestMethod()]
        public async Task LoginCodeTest(string token, string appKey, string device, bool exception, Type exceptionType = null)
        {
            var tokenRep = TokenRepositoryMock.CreateRepository();
            var userRep = UserRepositoryMock.CreateRepository();
            try
            {
                await GetTokenInfo .Execute(tokenRep, userRep, token, appKey, device);
                Assert.IsFalse(exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue((exception) && (exceptionType.FullName == ex.GetType().FullName));
            }
        }
    }
}
