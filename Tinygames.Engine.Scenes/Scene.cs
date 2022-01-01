using System;
using System.Linq;
using System.Collections.Generic;

namespace Tinygames.Engine.Scenes
{
    public class Scene
    {
        private List<GameObject> _gameObjects;

        public Scene()
        {
            _gameObjects = new List<GameObject>();
        }

        public virtual void Init()
        {
            foreach (var obj in _gameObjects) obj.Init();
        }

        public virtual void Update(float delta)
        {
            InitGameObjects();

            foreach (var obj in _gameObjects.Where(x => !x.Destroyed && x.Initialized))
            {
                obj.Update(delta);
            }

            DestroyGameObjects();
        }

        public virtual void Destroy()
        {
            foreach (var obj in _gameObjects) obj.Destroy();
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
            foreach (var obj in _gameObjects.Where(x => !x.Destroyed && !x.Initialized))
            {
                obj.Init();
            }
        }

        private void DestroyGameObjects()
        {
            _gameObjects = _gameObjects.Where(x => !x.Destroyed).ToList();
        }
    }
}
