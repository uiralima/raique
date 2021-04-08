using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raique.Microservices.Authenticate.Exceptions;
using Raique.Microservices.Authenticate.UseCases;
using Raique.Microservices.AuthenticateTests.Implementations;
using System;

namespace Raique.Microservices.AuthenticateTests.UseCases
{
    [TestClass()]
    public class CreateAppTests
    {
        [DataRow("App01", false, DisplayName = "Iserindo App 1")]
        [DataRow("App02", false, DisplayName = "Iserindo App 2")]
        [DataRow("App02", true, typeof(AppAlreadExistsException), DisplayName = "Tentando reiserir App 2")]
        [TestMethod()]
        public void CreateAppTest(string appName, bool exception, Type exceptionType = null)
        {
            var rep = AppRepositoryMock.CreateRepository();
            try
            {
                CreateApp.Execute(rep, appName);
                Assert.IsFalse(exception);
            }
            catch (Exception ex)
            {
                Assert.IsTrue((exception) && (exceptionType.FullName == ex.GetType().FullName));
            }
        }

        [DataRow("App03", true, DisplayName = "Repository Null")]
        [DataRow(null, false, DisplayName = "App Null")]
        [TestMethod()]
        public void CreateAppInvalidParametersTest(string appName, bool isRepNull)
        {
            var rep = isRepNull ? null : AppRepositoryMock.CreateRepository();
            try
            {
                CreateApp.Execute(rep, appName);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.GetType(), isRepNull ? typeof(ArgumentNullException) : typeof(ArgumentException));
            }
        }

        [DataRow("App 04", true, true, DisplayName = "Passou pelos 2 métodos")]
        [DataRow("App 04", false, true, DisplayName = "Passou apenas pelo GetByName")]
        [DataRow(null, false, false, DisplayName = "Não passou em nenhum")]
        [TestMethod()]
        public void CreateAppFlowTest(string appName, bool incCreate, bool incGetByName)
        {
            var rep = AppRepositoryMock.CreateRepository();
            try
            {
                CreateApp.Execute(rep, appName);
                Assert.AreEqual(rep.CreateCount, incCreate ? 1 : 0);
                Assert.AreEqual(rep.GetByNameCount, incGetByName ? 1 : 0);
            }
            catch
            {
                Assert.AreEqual(rep.CreateCount, incCreate ? 1 : 0);
                Assert.AreEqual(rep.GetByNameCount, incGetByName ? 1 : 0);
            }
        }
    }
}
