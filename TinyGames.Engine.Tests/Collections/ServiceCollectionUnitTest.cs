using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
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
            var collection = new GameServiceContainer();

            collection.AddService<TestServiceA>();
            collection.AddService<TestServiceB>();

            Assert.IsNotNull(collection.GetService<TestServiceA>());
            Assert.IsNotNull(collection.GetService<TestServiceB>());
        }
        
        [TestMethod]
        public void TestGetShouldBeNull()
        {
            var collection = new GameServiceContainer();

            Assert.IsNull(collection.GetService<TestServiceA>());
        }

        [TestMethod]
        public void TestCreateInstance()
        {
            var collection = new GameServiceContainer();

            collection.AddService<TestServiceA>();
            collection.AddService<TestServiceB>();

            Assert.IsNotNull(collection.CreateInstance<TestServiceC>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreateInstanceAndThrow()
        {
            var collection = new GameServiceContainer();

            collection.AddService<TestServiceA>();

            collection.CreateInstance<TestServiceC>(); // should throw
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreateInstanceAndThrowUncreateable()
        {
            var collection = new GameServiceContainer();

            collection.CreateInstance<TestServiceC>();
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
