using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raique.Microservices.Authenticate.UseCases;
using Raique.Microservices.AuthenticateTests.Implementations;
using System;
using System.Threading.Tasks;

namespace Raique.Microservices.AuthenticateTests.UseCases
{
    [TestClass()]
    public class LogoffTests
    {
        [DataRow(true, "TOKEN", "DEVICE", DisplayName = "Token Repository Null")]
        [DataRow(false, "", "DEVICE", DisplayName = "Token Empty")]
        [DataRow(false, "TOKEN", "", DisplayName = "Devcice Empty")]
        [TestMethod()]
        public async Task LogoffInvalidParametersTest(bool isTokenRepNull, string token, string device)
        {
            var tokenRep = isTokenRepNull ? null : TokenRepositoryMock.CreateRepository();
            try
            {
                await Logoff.Execute(tokenRep, token, device);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                if (isTokenRepNull)
                    Assert.AreEqual(ex.GetType(), typeof(ArgumentNullException));
                else
                    Assert.AreEqual(ex.GetType(), typeof(ArgumentException));
            }
        }

        [DataRow("TOKEN", "DEVICE", true)]
        [DataRow("TOKEN_INVALIDO", "DEVICE", true)]
        [DataRow("TOKEN", "DEVICE_INVALIDO", true)]
        [TestMethod()]
        public async Task LogoffFlowTest(string token, string device, bool callCloseCount)
        {
            var tokenRep = TokenRepositoryMock.CreateRepository();
            try
            {
                await Logoff.Execute(tokenRep, token, device);
                Assert.AreEqual(tokenRep.CloseCount, callCloseCount ? 1 : 0);
            }
            catch
            {
                Assert.AreEqual(tokenRep.CloseCount, callCloseCount ? 1 : 0);
            }
        }

        [DataRow("TOKEN", "DEVICE", false, DisplayName = "Token válido")]
        [DataRow("TOKEN_INVALIDO", "DEVICE", false, DisplayName = "Token inválido")]
        [DataRow("TOKEN", "DEVICE_INVALIDO", false, DisplayName = "Device inválido")]
        [TestMethod()]
        public async Task LogoffCodeTest(string token, string device, bool exception, Type exceptionType = null)
        {
            var tokenRep = TokenRepositoryMock.CreateRepository();
            try
            {
                await Logoff.Execute(tokenRep, token, device);
                Assert.IsFalse(exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue((exception) && (exceptionType.FullName == ex.GetType().FullName));
            }
        }
    }
}
