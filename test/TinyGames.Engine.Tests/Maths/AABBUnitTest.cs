using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Maths;

namespace TinyGames.Engine.Tests.Maths
{
    [TestClass]
    public class AABBUnitTest
    {
        [TestMethod]
        public void TestCreate()
        {
            var a = AABB.Create(4, 5, 8, 8);
            var b = AABB.Create(-2, -4, 9, 18);

            Assert.AreEqual(a.Left, 4);
            Assert.AreEqual(a.Right, 4 + 8);
            Assert.AreEqual(a.Top, 5);
            Assert.AreEqual(a.Bottom, 5 + 8);

            Assert.AreEqual(b.Left, -2);
            Assert.AreEqual(b.Right, -2 + 9);
            Assert.AreEqual(b.Top, -4);
            Assert.AreEqual(b.Bottom, -4 + 18);
        }
        [TestMethod]
        public void TestCreateCentered()
        {
            var a = AABB.CreateCentered(4, 5, 8, 8);
            var b = AABB.CreateCentered(-2, -4, 10, 18);

            Assert.AreEqual(a.Left, 4 - 4);
            Assert.AreEqual(a.Right, 4 + 4);
            Assert.AreEqual(a.Top, 5 - 4);
            Assert.AreEqual(a.Bottom, 5 + 4);

            Assert.AreEqual(b.Left, -2 - 5);
            Assert.AreEqual(b.Right, -2 + 5);
            Assert.AreEqual(b.Top, -4 - 9);
            Assert.AreEqual(b.Bottom, -4 + 9);
        }


        [TestMethod]
        public void TestOverlap()
        {
            var a = AABB.Create(2, 2, 4, 3);
            var b = AABB.Create(5, 0, 4, 3);
            var c = AABB.Create(8, 2, 5, 3);
            var d = AABB.Create(4, 1, 4, 6);

            Assert.IsTrue(AABB.Overlaps(a, a));
            Assert.IsTrue(AABB.Overlaps(a, b));
            Assert.IsFalse(AABB.Overlaps(a, c));
            Assert.IsTrue(AABB.Overlaps(a, d));

            Assert.IsTrue(AABB.Overlaps(b, a));
            Assert.IsTrue(AABB.Overlaps(b, b));
            Assert.IsTrue(AABB.Overlaps(b, c));
            Assert.IsTrue(AABB.Overlaps(b, d));

            Assert.IsFalse(AABB.Overlaps(c, a));
            Assert.IsTrue(AABB.Overlaps(c, b));
            Assert.IsTrue(AABB.Overlaps(c, c));
            Assert.IsFalse(AABB.Overlaps(c, d));

            Assert.IsTrue(AABB.Overlaps(d, a));
            Assert.IsTrue(AABB.Overlaps(d, b));
            Assert.IsFalse(AABB.Overlaps(d, c));
            Assert.IsTrue(AABB.Overlaps(d, d));
        }
    }
}
