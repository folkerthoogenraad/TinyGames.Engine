using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Tinygames.Engine.Scenes
{
    public class GameObject
    {
        public bool Initialized { get; set; }
        public bool Destroyed { get; set; }
        public Scene Scene { get; set; }

        public IEnumerable<Component> Components => _components;

        private List<Component> _components;

        public GameObject()
        {
            _components = new List<Component>();
        }

        public virtual void Init()
        {
            if (Initialized) throw new InvalidOperationException("GameObject is already initialized.");

            Initialized = true;
            foreach (var component in Components) component.Init();
        }

        public virtual void Update(float delta)
        {
            foreach(var component in Components) component.Update(delta);
        }

        public virtual void Destroy()
        {
            if (!Initialized) throw new InvalidOperationException("GameObject is not yet initialized");

            Initialized = false;
            foreach (var component in Components) component.Destroy();
        }

        public T AddComponent<T>() where T : Component, new()
        {
            return AddComponent(new T());
        }

        public T AddComponent<T>(T component) where T : Component
        {
            if (Initialized) throw new ArgumentException("Cannot add components at runtime.");

            component.GameObject = this;
            _components.Add(component);

            return component;
        }

        public T GetComponent<T>() where T : Component
        {
            return GetComponents<T>().FirstOrDefault();
        }

        public IEnumerable<T> GetComponents<T>() where T : Component
        {
            return Components.OfType<T>();
        }
    }
}
