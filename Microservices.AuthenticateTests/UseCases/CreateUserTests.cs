using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raique.Microservices.Authenticate.Exceptions;
using Raique.Microservices.Authenticate.UseCases;
using Raique.Microservices.AuthenticateTests.Implementations;
using System;

namespace Raique.Microservices.AuthenticateTests.UseCases
{
    [TestClass]
    public class CreateUserTests
    {
        [DataRow(true, false, DisplayName = "App Repository Null")]
        [DataRow(false, true, DisplayName = "User Repository Null")]
        [DataRow(false, false, DisplayName = "AppKey Empty")]
        [DataRow(false, false, "Key", DisplayName = "UserName Empty")]
        [DataRow(false, false, "Key", "UserName", DisplayName = "Password Empty")]
        [TestMethod()]
        public void CreateUserInvalidParametersTest(bool isAppRepNull, bool isUserRepNull, string key = "", string userName = "", string password = "")
        {
            var appRep = isAppRepNull ? null : AppRepositoryMock.CreateRepository();
            var userRep = isUserRepNull ? null : UserRepositoryMock.CreateRepository();
            try
            {
                CreateUser.Execute(appRep, userRep, key, userName, password);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                if (isAppRepNull || isUserRepNull)
                    Assert.AreEqual(ex.GetType(), typeof(ArgumentNullException));
                else
                    Assert.AreEqual(ex.GetType(), typeof(ArgumentException));
            }
        }

        [DataRow("Chave", "Usuario", "Senha", true, true, true, DisplayName = "Passou pelos 3 métodos")]
        [DataRow("Chave", "Usuario", "Senha", false, true, false, DisplayName = "Passou apenas pelo GetByName")]
        [DataRow("Chave2", "Usuario2", "Senha", false, true, true, DisplayName = "Não achou o app")]
        [DataRow(null, "Usuario2", "Senha", false, false, false, DisplayName = "Parametro inválido")]
        [TestMethod()]
        public void CreateUserFlowTest(string appKey = "", string userName = "", string password = "", bool incCreate = false, bool incGetByKeyToAppCount = false, bool incAppGetByKey = false)
        {
            var userRep = UserRepositoryMock.CreateRepository();
            var appRep = AppRepositoryMock.CreateRepository();
            try
            {
                CreateUser.Execute(appRep, userRep, appKey, userName, password);
                Assert.AreEqual(userRep.CreateCount, incCreate ? 1 : 0);
                Assert.AreEqual(userRep.GetByKeyToAppCount, incGetByKeyToAppCount ? 1 : 0);
                Assert.AreEqual(appRep.GetByKeyCount, incAppGetByKey ? 1 : 0, "Não deveria ter chamado a verificação de app");
            }
            catch
            {
                Assert.AreEqual(userRep.CreateCount, incCreate ? 1 : 0);
                Assert.AreEqual(userRep.GetByKeyToAppCount, incGetByKeyToAppCount ? 1 : 0);
                Assert.AreEqual(appRep.GetByKeyCount, incAppGetByKey ? 1 : 0, "Não deveria ter chamado a verificação de app");
            }
        }

        [DataRow("Chave", "Usuario2", "Senha", false, DisplayName = "Iserindo Usuário 1")]
        [DataRow("Chave", "Usuario2", "Senha", true, typeof(UserAlreadExistsException), DisplayName = "Inserindo Usuário com o mesmo nome")]
        [DataRow("Chave2", "Usuario3", "Senha", true, typeof(InvalidAppException), DisplayName = "Tentando reiserir App 2")]
        [TestMethod()]
        public void CreateUserTest(string appKey, string userName, string password, bool exception, Type exceptionType = null)
        {
            var userRep = UserRepositoryMock.CreateRepository();
            var appRep = AppRepositoryMock.CreateRepository();
            try
            {
                CreateUser.Execute(appRep, userRep, appKey, userName, password);
                Assert.IsFalse(exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue((exception) && (exceptionType.FullName == ex.GetType().FullName));
            }
        }
    }
}
