using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raique.Microservices.Authenticate.Exceptions;
using Raique.Microservices.Authenticate.UseCases;
using Raique.Microservices.AuthenticateTests.Implementations;
using System;

namespace Raique.Microservices.AuthenticateTests.UseCases
{
    [TestClass]
    public class LoginTest
    {
        [DataRow(true, false, false, DisplayName = "Token Repository Null")]
        [DataRow(false, true, false, DisplayName = "User Repository Null")]
        [DataRow(false, false, true, DisplayName = "Token Creator Null")]
        [DataRow(false, false, false, DisplayName = "AppKey Empty")]
        [DataRow(false, false, false, "Key", DisplayName = "UserName Empty")]
        [DataRow(false, false, false, "Key", "UserName", DisplayName = "Password Empty")]
        [DataRow(false, false, false, "Key", "UserName", "Password", DisplayName = "Device Empty")]
        [TestMethod()]
        public void LoginInvalidParametersTest(bool isTokenRepNull, bool isUserRepNull, bool isTokenCreatorNull, string key = "", string userName = "", string password = "", string device = "")
        {
            var tokenRep = isTokenRepNull ? null : TokenRepositoryMock.CreateRepository();
            var userRep = isUserRepNull ? null : UserRepositoryMock.CreateRepository();
            var tokenCreator = isTokenCreatorNull ? null : TokenCreatorMock.Create();
            try
            {
                Login.Execute(tokenRep, userRep, tokenCreator, key, userName, password, device);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                if (isTokenRepNull || isUserRepNull || isTokenCreatorNull)
                    Assert.AreEqual(ex.GetType(), typeof(ArgumentNullException));
                else
                    Assert.AreEqual(ex.GetType(), typeof(ArgumentException));
            }
        }

        [DataRow("CHAVE", "USUARIO", "SENHA", "DEVICE", true, true, true, DisplayName = "Login Válido")]
        [DataRow("CHAVE2", "USUARIO", "SENHA", "DEVICE", true, false, false, DisplayName = "Login Inválido")]
        [TestMethod()]
        public void LoginFlowTest(string appKey = "", string userName = "", string password = "", string device = "", bool callGetByKeyToAppCount = false, bool callCreateToken = false, bool callTokenCreator = false)
        {
            var userRep = UserRepositoryMock.CreateRepository();
            var tokenRep = TokenRepositoryMock.CreateRepository();
            var tokenCreator = TokenCreatorMock.Create();
            try
            {
                Login.Execute(tokenRep, userRep, tokenCreator, appKey, userName, password, device);
                Assert.AreEqual(userRep.GetByKeyToAppCount, callGetByKeyToAppCount ? 1 : 0);
                Assert.AreEqual(tokenRep.CreateCount, callCreateToken ? 1 : 0);
                Assert.AreEqual(tokenCreator.CreateCount, callTokenCreator ? 1 : 0);
            }
            catch
            {
                Assert.AreEqual(userRep.GetByKeyToAppCount, callGetByKeyToAppCount ? 1 : 0);
                Assert.AreEqual(tokenRep.CreateCount, callCreateToken ? 1 : 0);
                Assert.AreEqual(tokenCreator.CreateCount, callTokenCreator ? 1 : 0);
            }
        }

        [DataRow("CHAVE", "USUARIO", "SENHA", "DEVICE", false, DisplayName = "Usuário autenticado")]
        [DataRow("CHAVE", "USUARIOINVALIDO", "SENHA", "DEVICE", true, typeof(InvalidUsernameOrPasswordException), DisplayName = "Usuário inválido")]
        [DataRow("CHAVE", "USUARIO", "SENHAINVALIDA", "DEVICE", true, typeof(InvalidUsernameOrPasswordException), DisplayName = "Senha Inválida")]
        [DataRow("CHAVEINVALIDA", "USUARIO", "SENHA", "DEVICE", true, typeof(InvalidUsernameOrPasswordException), DisplayName = "Chave Inválida")]
        [TestMethod()]
        public void LoginCodeTest(string appKey, string userName, string password, string device, bool exception, Type exceptionType = null)
        {
            var userRep = UserRepositoryMock.CreateRepository();
            var tokenRep = TokenRepositoryMock.CreateRepository();
            var tokenCreator = TokenCreatorMock.Create();
            try
            {
                Login.Execute(tokenRep, userRep, tokenCreator, appKey, userName, password, device);
                Assert.IsFalse(exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue((exception) && (exceptionType.FullName == ex.GetType().FullName));
            }
        }
    }
}
