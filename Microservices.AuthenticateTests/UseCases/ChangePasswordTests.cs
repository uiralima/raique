using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raique.Microservices.Authenticate.Exceptions;
using Raique.Microservices.Authenticate.UseCases;
using Raique.Microservices.AuthenticateTests.Implementations;
using System;

namespace Raique.Microservices.AuthenticateTests.UseCases
{
    [TestClass()]
    public class ChangePasswordTests
    {
        [DataRow(true, 1, "NOVO", "CHAVE", DisplayName = "User Repository Null")]
        [DataRow(false, 0, "NOVO", "CHAVE", DisplayName = "UserId invalido")]
        [DataRow(false, 1, "", "CHAVE", DisplayName = "Nova senha invalida")]
        [DataRow(false, 1, "NOVO", "", DisplayName = "Chave invalida")]
        [TestMethod()]
        public void ChangePasswordInvalidParametersTest(bool isUserRepNull, int userId, string newPassword, string appKey)
        {
            var userRep = isUserRepNull ? null : UserRepositoryMock.CreateRepository();

            try
            {
                ChangePassword.Execute(userRep, userId, newPassword, appKey);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                if (isUserRepNull)
                    Assert.AreEqual(ex.GetType(), typeof(ArgumentNullException));
                else
                    Assert.AreEqual(ex.GetType(), typeof(ArgumentException));
            }
        }

        [DataRow(1, "NOVO", "CHAVE", true, true)]
        [DataRow(0, "NOVO", "CHAVE", false, false)]
        [DataRow(1, "NOVO", "CHAVE_INVALIDA", true, false)]
        [TestMethod()]
        public void ChangePasswordFlowTest(int userId, string newPassword, string appKey, bool callGetById, bool callChangePassword)
        {
            var userRep = UserRepositoryMock.CreateRepository();
            try
            {
                ChangePassword.Execute(userRep, userId, newPassword, appKey);
                Assert.AreEqual(userRep.GetByIdCount, callGetById ? 1 : 0);
                Assert.AreEqual(userRep.ChangePasswordCount, callChangePassword ? 1 : 0);
            }
            catch
            {
                Assert.AreEqual(userRep.GetByIdCount, callGetById ? 1 : 0);
                Assert.AreEqual(userRep.ChangePasswordCount, callChangePassword ? 1 : 0);
            }
        }

        [DataRow(1, "NOVO", "CHAVE", false)]
        [DataRow(15, "NOVO", "CHAVE", true, typeof(InvalidUserException))]
        [DataRow(1, "NOVO", "CHAVE_INVALIDA", true, typeof(InvalidUserException))]
        [TestMethod()]
        public void ChangePasswordCodeTest(int userId, string newPassword, string appKey, bool exception, Type exceptionType = null)
        {
            var userRep = UserRepositoryMock.CreateRepository();
            try
            {
                ChangePassword.Execute(userRep, userId, newPassword, appKey);
                Assert.IsFalse(exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue((exception) && (exceptionType.FullName == ex.GetType().FullName));
            }
        }
    }
}
