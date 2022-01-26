using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tinygames.Engine.Scenes;
using System.Linq;

namespace TinyGames.Engine.Scenes.Tests
{
    [TestClass]
    public class SceneUnitTest
    {
        [TestMethod]
        public void TestSceneIsInitialized()
        {
            Scene scene = new Scene();

            Assert.IsFalse(scene.Initialized);

            scene.Init();

            Assert.IsTrue(scene.Initialized);

            scene.Destroy();

            Assert.IsFalse(scene.Initialized);
        }

        [TestMethod]
        public void TestSceneAddGameObjects()
        {
            Scene scene = new Scene();

            scene.AddGameObject(new GameObject());
            scene.AddGameObject(new GameObject());
            scene.AddGameObject(new GameObject());

            Assert.AreEqual(3, scene.GameObjects.Count());

            scene.Init();

            Assert.AreEqual(3, scene.GameObjects.Count());
        }

        [TestMethod]
        public void TestSceneRemoveGameObjects()
        {
            Scene scene = new Scene();

            var obj = new GameObject();

            scene.AddGameObject(obj);
            scene.AddGameObject(new GameObject());
            scene.AddGameObject(new GameObject());

            Assert.AreEqual(3, scene.GameObjects.Count());

            scene.Init();

            Assert.AreEqual(3, scene.GameObjects.Count());

            scene.RemoveGameObject(obj);

            Assert.AreEqual(2, scene.GameObjects.Count());
        }

        [TestMethod]
        public void TestSceneAddGameObjectsAtRunning()
        {
            Scene scene = new Scene();

            var obj = new GameObject();

            scene.AddGameObject(new GameObject());
            scene.AddGameObject(new GameObject());

            Assert.AreEqual(2, scene.GameObjects.Count());

            scene.Init();

            Assert.AreEqual(2, scene.GameObjects.Count());

            scene.AddGameObject(obj);

            Assert.AreEqual(2, scene.GameObjects.Count());
            Assert.IsFalse(obj.Initialized);

            scene.Update(1);

            Assert.AreEqual(3, scene.GameObjects.Count());
            Assert.IsTrue(obj.Initialized);
        }

        [TestMethod]
        public void TestUpdateCounts()
        {
            Scene scene = new Scene();

            var obj1 = new UpdateCountTestGameObject();
            var obj2 = new UpdateCountTestGameObject();
            var obj3 = new UpdateCountTestGameObject();

            scene.AddGameObject(obj1);
            scene.AddGameObject(obj2);
            scene.AddGameObject(obj3);

            scene.Init();

            for (int i = 0; i < 10; i++) scene.Update(1);

            Assert.AreEqual(obj1.Count, 10);
            Assert.AreEqual(obj2.Count, 10);
            Assert.AreEqual(obj3.Count, 10);
        }

        [TestMethod]
        public void TestAddGameObjectDuringUpdate()
        {
            Scene scene = new Scene();

            var obj1 = new AddGameObjectDuringUpdateGameObject<GameObject>();

            scene.AddGameObject(obj1);

            scene.Init();

            Assert.IsTrue(scene.GameObjects.Count() == 1);

            scene.Update(1);

            Assert.IsTrue(scene.GameObjects.Count() == 2);
        }

        [TestMethod]
        public void TestAddGameObjectDuringInit()
        {
            Scene scene = new Scene();

            var obj1 = new AddGameObjectDuringInitGameObject<GameObject>();

            scene.AddGameObject(obj1);

            Assert.IsTrue(scene.GameObjects.Count() == 1);

            scene.Init();

            Assert.IsTrue(scene.GameObjects.Count() == 2);
        }
    }
}
