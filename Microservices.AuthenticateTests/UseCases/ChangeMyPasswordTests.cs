using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raique.Microservices.Authenticate.Exceptions;
using Raique.Microservices.Authenticate.UseCases;
using Raique.Microservices.AuthenticateTests.Implementations;
using System;
using System.Threading.Tasks;

namespace Raique.Microservices.AuthenticateTests.UseCases
{
    [TestClass()]
    public class ChangeMyPasswordTests
    {
        [DataRow(true, false, "TOKEN", "SENHA", "NOVO", "CHAVE", "DEVICE")]
        [DataRow(false, true, "TOKEN", "SENHA", "NOVO", "CHAVE", "DEVICE")]
        [DataRow(false, false, "", "SENHA", "NOVO", "CHAVE", "DEVICE")]
        [DataRow(false, false, "TOKEN", "", "NOVO", "CHAVE", "DEVICE")]
        [DataRow(false, false, "TOKEN", "SENHA", "", "CHAVE", "DEVICE")]
        [DataRow(false, false, "TOKEN", "SENHA", "NOVO", "", "DEVICE")]
        [DataRow(false, false, "TOKEN", "SENHA", "NOVO", "CHAVE", "")]
        [TestMethod()]
        public async Task ChangeMyPasswordInvalidParametersTest(bool isUserRepNull, bool isTokenRepNull, string token,
            string currentPassword, string newPassword, string appKey, string device)
        {
            var userRep = isUserRepNull ? null : UserRepositoryMock.CreateRepository();
            var tokenRepository = isTokenRepNull ? null : TokenRepositoryMock.CreateRepository();
            try
            {
                await ChangeMyPassword.Execute(tokenRepository, userRep, token, currentPassword, newPassword, appKey, device);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                if (isUserRepNull || isTokenRepNull)
                    Assert.AreEqual(ex.GetType(), typeof(ArgumentNullException));
                else
                    Assert.AreEqual(ex.GetType(), typeof(ArgumentException));
            }
        }

        [DataRow("TOKEN", "SENHA", "NOVO", "CHAVE", "DEVICE", true, true, true)]
        [DataRow("TOKEN_INVALIDO", "SENHA", "NOVO", "CHAVE", "DEVICE", true, false, false)]
        [DataRow("TOKEN", "SENHA_INVALIDA", "NOVO", "CHAVE", "DEVICE", true, true, false)]
        [DataRow("TOKEN", "SENHA", "NOVO", "CHAVE_INVALIDA", "DEVICE", true, true, false)]
        [DataRow("TOKEN", "SENHA", "NOVO", "CHAVE", "DEVICE_INVALIDO", true, false, false)]
        [TestMethod()]
        public async Task ChangeMyPasswordFlowTest(string token, string currentPassword, string newPassword, string appKey,
            string device, bool callGetUserIdByToken, bool callGetById, bool callChangePassword)
        {
            var userRep = UserRepositoryMock.CreateRepository();
            var tokenRepository = TokenRepositoryMock.CreateRepository();
            try
            {
                await ChangeMyPassword.Execute(tokenRepository, userRep, token, currentPassword, newPassword, appKey, device);
                Assert.AreEqual(userRep.GetByIdCount, callGetById ? 1 : 0);
                Assert.AreEqual(userRep.ChangePasswordCount, callChangePassword ? 1 : 0);
                Assert.AreEqual(tokenRepository.GetUserIdByTokenCount, callGetUserIdByToken ? 1 : 0);
            }
            catch
            {
                Assert.AreEqual(userRep.GetByIdCount, callGetById ? 1 : 0);
                Assert.AreEqual(userRep.ChangePasswordCount, callChangePassword ? 1 : 0);
                Assert.AreEqual(tokenRepository.GetUserIdByTokenCount, callGetUserIdByToken ? 1 : 0);
            }
        }

        [DataRow("TOKEN", "SENHA", "NOVO", "CHAVE", "DEVICE", false)]
        [DataRow("TOKEN_INVALIDO", "SENHA", "NOVO", "CHAVE", "DEVICE", true, typeof(InvalidTokenException))]
        [DataRow("TOKEN", "SENHA_INVALIDA", "NOVO", "CHAVE", "DEVICE", true, typeof(InvalidCurrentPasswordException))]
        [DataRow("TOKEN", "SENHA", "NOVO", "CHAVE_INVALIDA", "DEVICE", true, typeof(InvalidTokenException))]
        [DataRow("TOKEN", "SENHA", "NOVO", "CHAVE", "DEVICE_INVALIDO", true, typeof(InvalidTokenException))]
        [TestMethod()]
        public async Task ChangePasswordCodeTest(string token, string currentPassword, string newPassword, string appKey,
            string device, bool exception, Type exceptionType = null)
        {
            var userRep = UserRepositoryMock.CreateRepository();
            var tokenRepository = TokenRepositoryMock.CreateRepository();
            try
            {
                await ChangeMyPassword.Execute(tokenRepository, userRep, token, currentPassword, newPassword, appKey, device);
                Assert.IsFalse(exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue((exception) && (exceptionType.FullName == ex.GetType().FullName));
            }
        }
    }
}
