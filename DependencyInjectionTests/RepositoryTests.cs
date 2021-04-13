using DependencyInjectionTests.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raique.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raique.DependencyInjection.Tests
{
    [TestClass()]
    public class RepositoryTests
    {
        #region SetTransiente
        [TestMethod()]
        public void SetTransienteAlreadTransienteTest()
        {
            Repository.Clear();
            try
            {
                Repository.SetTransiente<ITestInterface, TestType1>();
                Repository.SetTransiente<ITestInterface, TestType1>();
                Assert.Fail();
            }
            catch(System.Exception ex)
            {
                Assert.AreEqual(ex.GetType().FullName, typeof(Exception.TypeAlreadTransientException).FullName);
            }
        }
        [TestMethod()]
        public void SetTransientealreadSiingletonTest()
        {
            Repository.Clear();
            try
            {
                Repository.SetSingleton<ITestInterface, TestType1>();
                Repository.SetTransiente<ITestInterface, TestType1>();
                Assert.Fail();
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.GetType().FullName, typeof(Exception.TypeAlreadSingletonException).FullName);
            }
        }
        [TestMethod()]
        public void SetTransienteSuccess()
        {
            Repository.Clear();
            try
            {
                Repository.SetTransiente<ITestInterface, TestType1>();
                Assert.AreEqual(Repository.CreateInstance<ITestInterface>().GetType().FullName, typeof(TestType1).FullName);
            }
            catch
            {
                Assert.Fail();
            }
        }
        #endregion

        #region SetSingleton
        [TestMethod()]
        public void SetSingletonAlreadTransienteTest()
        {
            Repository.Clear();
            try
            {
                Repository.SetTransiente<ITestInterface, TestType1>();
                Repository.SetSingleton<ITestInterface, TestType1>();
                Assert.Fail();
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.GetType().FullName, typeof(Exception.TypeAlreadTransientException).FullName);
            }
        }
        [TestMethod()]
        public void SetSingletonalreadSiingletonTest()
        {
            Repository.Clear();
            try
            {
                Repository.SetSingleton<ITestInterface, TestType1>();
                Repository.SetSingleton<ITestInterface, TestType1>();
                Assert.Fail();
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.GetType().FullName, typeof(Exception.TypeAlreadSingletonException).FullName);
            }
        }
        [TestMethod()]
        public void SetSingletonSuccess()
        {
            Repository.Clear();
            try
            {
                Repository.SetSingleton<ITestInterface, TestType1>();
                Assert.AreEqual(Repository.CreateInstance<ITestInterface>().GetType().FullName, typeof(TestType1).FullName);
            }
            catch
            {
                Assert.Fail();
            }
        }
        #endregion

        #region Test Transiente
        [TestMethod()]
        public void TestIfTransiente()
        {
            Repository.Clear();
            Repository.SetTransiente<ITestInterface, TestType1>();
            var aux = Repository.CreateInstance<ITestInterface>();
            aux.IncCount();
            aux = Repository.CreateInstance<ITestInterface>();
            Assert.AreEqual(aux.Count, 0);
        }
        #endregion

        #region Test Singleton
        [TestMethod()]
        public void TestIfSingleton()
        {
            Repository.Clear();
            Repository.SetSingleton<ITestInterface, TestType1>();
            var aux = Repository.CreateInstance<ITestInterface>();
            aux.IncCount();
            aux = Repository.CreateInstance<ITestInterface>();
            Assert.AreEqual(1, aux.Count);
        }
        #endregion

        #region Test TypeLoad Exception

        [TestMethod()]
        public void MainTypeUnregistred()
        {
            Repository.Clear();
            try
            {
                var aux = Repository.CreateInstance<ITestInterface>();
                Assert.Fail();
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(typeof(Exception.TypeLoadException).FullName, ex.GetType().FullName);
            }
        }

        [TestMethod()]
        public void InjectedTypeUnregistred()
        {
            Repository.Clear();
            try
            {
                Repository.SetTransiente<ITestInterface, TestType2>();
                var aux = Repository.CreateInstance<ITestInterface>();
                Assert.Fail();
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(typeof(Exception.TypeLoadException).FullName, ex.GetType().FullName);
            }
        }

        [TestMethod()]
        public void InjectedTypeSuccess()
        {
            Repository.Clear();
            try
            {
                Repository.SetTransiente<ITestInterface, TestType2>();
                Repository.SetSingleton<IInjectedType, InjectedType>();
                var aux = Repository.CreateInstance<ITestInterface>();
            }
            catch
            {
                Assert.Fail();
            }
        }

        #endregion

        #region Set Preference
        //TODO: Testar SetPreference e SetPreferenceInContext 

        #endregion
    }
}