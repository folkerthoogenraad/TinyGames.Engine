using System;
using System.Linq;
using System.Collections.Generic;

namespace TinyGames.Engine.Scenes
{
    public class Scene
    {
        public bool Initialized { get; private set; }

        private List<ISceneComponent> _components;
        private List<GameObject> _gameObjects;
        private int _lastIndex = 0;

        public IEnumerable<GameObject> GameObjects => _gameObjects.Where(x => !x.Destroyed && (x.Initialized || !Initialized));
        public IEnumerable<ISceneComponent> Components => _components;

        public Scene()
        {
            _gameObjects = new List<GameObject>();
            _components = new List<ISceneComponent>();
            _lastIndex = 0;
        }

        public virtual void Init()
        {
            if (Initialized) throw new InvalidOperationException("Scene is already initialized.");

            foreach(var component in _components) component.Init(this);
            
            Initialized = true;
            InitGameObjects();
        }

        public virtual void Update(float delta)
        {
            Sync();

            foreach (var component in _components) component.BeforeUpdate(delta);

            // This "ToArray" is pretty inefficient (full copy), but we keep our current state throughout
            foreach (var obj in _gameObjects.ToArray().Where(x => !x.Destroyed && x.Initialized)) 
            {
                obj.Update(delta);
            }

            foreach (var component in _components) component.AfterUpdate(delta);

            Sync();
        }

        public virtual void Destroy()
        {
            if (!Initialized) throw new InvalidOperationException("Scene is not yet initialized");

            foreach (var obj in _gameObjects) RemoveGameObject(obj);

            DestroyGameObjects();

            foreach (var component in _components) component.Destroy();

            Initialized = false;
        }

        public void Sync()
        {
            InitGameObjects();
            DestroyGameObjects();
        }

        public void AddGameObject(GameObject obj)
        {
            var requiredAttributes = obj.GetType().GetCustomAttributes(typeof(RequireSceneComponent), true).OfType<RequireSceneComponent>();
            
            foreach(var attribute in requiredAttributes)
            {
                if (GetComponent(attribute.Type) == null)
                {
                    throw new ArgumentException("Scene does not contain the required scene components for this object to function.");
                }
            }

            obj.Scene = this;
            obj.Id = _lastIndex++;
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
            foreach (var obj in _gameObjects.Where(x => x.Destroyed && x.Initialized))
            {
                obj.Destroy();
            }

            _gameObjects.RemoveAll(x => x.Destroyed);
        }

        public GameObject FindGameObjectById(int id)
        {
            return _gameObjects.Where(x => x.Id == id && !x.Destroyed).FirstOrDefault();
        }

        public IEnumerable<T> FindGameObjectsOfType<T>()
        {
            return GameObjects.OfType<T>();
        }

        public T AddComponent<T>(T t) where T : ISceneComponent
        {
            if (Initialized) throw new ArgumentException("Cannot add components after being initialized");

            _components.Add(t);

            return t;
        }

        public T GetComponent<T>()
        {
            return _components.OfType<T>().FirstOrDefault();
        }
        public object GetComponent(Type type)
        {
            return _components.Where(x => x.GetType() == type).FirstOrDefault();
        }
    }
}
