using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Collections;

namespace TinyGames.Engine.Tests.Collections
{
    [TestClass]
    public class ServiceCollectionUnitTest
    {
        [TestMethod]
        public void TestRegisterAndGet()
        {
            var collection = new ServiceCollection();

            collection.RegisterService<TestServiceA>();
            collection.RegisterService<TestServiceB>();

            Assert.IsNotNull(collection.GetService<TestServiceA>());
            Assert.IsNotNull(collection.GetService<TestServiceB>());
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGetAndThrowUnregistered()
        {
            var collection = new ServiceCollection();

            collection.GetService<TestServiceA>();
        }

        [TestMethod]
        public void TestCreateInstance()
        {
            var collection = new ServiceCollection();

            collection.RegisterService<TestServiceA>();
            collection.RegisterService<TestServiceB>();

            Assert.IsNotNull(collection.CreateInstance<TestServiceC>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreateInstanceAndThrow()
        {
            var collection = new ServiceCollection();

            collection.RegisterService<TestServiceA>();

            Assert.IsNotNull(collection.CreateInstance<TestServiceC>());
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestRegisterServiceDouble()
        {
            var collection = new ServiceCollection();

            collection.RegisterService<TestServiceA>();
            collection.RegisterService<TestServiceA>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreateInstanceAndThrowUncreateable()
        {
            var collection = new ServiceCollection();

            collection.CreateInstance<TestServiceC>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestRegisterAndThrowUncreateable()
        {
            var collection = new ServiceCollection();

            collection.RegisterService<TestServiceC>();
        }

        public class TestServiceA
        {

        }
        public class TestServiceB
        {

        }
        public class TestServiceC
        {
            public TestServiceC(TestServiceA a, TestServiceB b)
            {

            }
        }
    }
}
