using System;
using System.Linq;
using System.Collections.Generic;

namespace TinyGames.Engine.Scenes
{
    public class Scene
    {
        public delegate void SceneGameObjectDelegate(Scene scene, GameObject obj);

        public bool Initialized { get; private set; }

        private List<ISceneBehaviour> _behaviours;
        private List<GameObject> _gameObjects;
        private int _lastIndex = 0;

        public IEnumerable<GameObject> GameObjects => _gameObjects.Where(x => !x.Destroyed && (x.Initialized || !Initialized));
        public IEnumerable<ISceneBehaviour> Behaviours => _behaviours;
        public IServiceProvider Services { get; }

        public event SceneGameObjectDelegate OnGameObjectCreated;
        public event SceneGameObjectDelegate OnGameObjectDestroyed;

        public Scene(IServiceProvider services)
        {
            _gameObjects = new List<GameObject>();
            _behaviours = new List<ISceneBehaviour>();
            _lastIndex = 0;

            Services = services;
        }

        public virtual void Init()
        {
            if (Initialized) throw new InvalidOperationException("Scene is already initialized.");

            foreach(var component in _behaviours) component.Init(this);
            
            Initialized = true;
            InitGameObjects();
        }

        public virtual void Update(float delta)
        {
            Sync();

            foreach (var component in _behaviours) component.BeforeUpdate(delta);

            // This "ToArray" is pretty inefficient (full copy), but we keep our current state throughout
            foreach (var obj in _gameObjects.ToArray().Where(x => !x.Destroyed && x.Initialized)) 
            {
                obj.Update(delta);
            }

            foreach (var component in _behaviours) component.AfterUpdate(delta);

            Sync();
        }

        public virtual void Destroy()
        {
            if (!Initialized) throw new InvalidOperationException("Scene is not yet initialized");

            foreach (var obj in _gameObjects) RemoveGameObject(obj);

            DestroyGameObjects();

            foreach (var component in _behaviours) component.Destroy();

            Initialized = false;
        }

        public void Sync()
        {
            InitGameObjects();
            DestroyGameObjects();
        }

        public void AddGameObject(GameObject obj)
        {
            var requiredAttributes = obj.GetType().GetCustomAttributes(typeof(RequireSceneBehaviour), true).OfType<RequireSceneBehaviour>();
            
            foreach(var attribute in requiredAttributes)
            {
                if (GetBehaviour(attribute.Type) == null)
                {
                    throw new ArgumentException("Scene does not contain the required scene components for this object to function.");
                }
            }

            obj.Scene = this;

            if (obj.Id < 0)
            {
                obj.Id = _lastIndex++;
            }
            else if (FindGameObjectById(obj.Id) != null) throw new ArgumentException("Id is already in use");       
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
                    OnGameObjectCreated?.Invoke(this, obj);
                }
            } while (toInit.Length > 0);
        }

        private void DestroyGameObjects()
        {
            // NOTE: destroy shouln't ever add game objects to the scene
            foreach (var obj in _gameObjects.Where(x => x.Destroyed && x.Initialized))
            {
                obj.Destroy();
                OnGameObjectDestroyed?.Invoke(this, obj);
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

        public T AddBehaviour<T>(T t) where T : ISceneBehaviour
        {
            if (Initialized) throw new ArgumentException("Cannot add behaviours after being initialized");

            _behaviours.Add(t);

            return t;
        }

        public T GetBehaviour<T>()
        {
            return _behaviours.OfType<T>().FirstOrDefault();
        }
        public object GetBehaviour(Type type)
        {
            return _behaviours.Where(x => x.GetType() == type).FirstOrDefault();
        }
    }
}
