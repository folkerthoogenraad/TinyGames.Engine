using System;
using System.Linq;
using System.Collections.Generic;

namespace Tinygames.Engine.Scenes
{
    public class Scene
    {
        public bool Initialized { get; private set; }

        private List<GameObject> _gameObjects;
        public IEnumerable<GameObject> GameObjects => _gameObjects.Where(x => !x.Destroyed && (x.Initialized || !Initialized));

        public Scene()
        {
            _gameObjects = new List<GameObject>();
        }

        public virtual void Init()
        {
            if (Initialized) throw new InvalidOperationException("Scene is already initialized.");

            Initialized = true;
            InitGameObjects();
        }

        public virtual void Update(float delta)
        {
            InitGameObjects();
            DestroyGameObjects();

            // This "ToArray" is pretty inefficient (full copy), but we keep our current state throughout
            foreach (var obj in _gameObjects.ToArray().Where(x => !x.Destroyed && x.Initialized)) 
            {
                obj.Update(delta);
            }

            InitGameObjects();
            DestroyGameObjects();
        }

        public virtual void Destroy()
        {
            if (!Initialized) throw new InvalidOperationException("Scene is not yet initialized");

            foreach (var obj in _gameObjects) RemoveGameObject(obj);

            DestroyGameObjects();

            Initialized = false;
        }

        public void AddGameObject(GameObject obj)
        {
            obj.Scene = this;
            _gameObjects.Add(obj);
        }

        public void RemoveGameObject(GameObject obj)
        {
            obj.Destroyed = true;
        }

        private void InitGameObjects()
        {
            GameObject[] toInit;
            do
            {
                toInit = _gameObjects.Where(x => !x.Destroyed && !x.Initialized).ToArray();
                foreach (var obj in _gameObjects.Where(x => !x.Destroyed && !x.Initialized).ToArray())
                {
                    obj.Init();
                }
            } while (toInit.Length > 0);
        }

        private void DestroyGameObjects()
        {
            // NOTE: destroy shouln't ever add game objects to the scene
            foreach (var obj in _gameObjects.Where(x => !x.Destroyed && !x.Initialized))
            {
                obj.Destroy();
            }

            _gameObjects.RemoveAll(x => x.Destroyed);
        }
    }
}
