using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TinyGames.Engine.Scenes
{
    public class GameObject
    {
        private readonly List<Component> _components;

        public int Id { get; set; } = -1;
        public bool Initialized { get; set; }
        public bool Destroyed { get; set; }
        public Scene Scene { get; set; }
        public IEnumerable<Component> Components => _components;

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

            var requiredAttributes = component.GetType().GetCustomAttributes(typeof(RequireComponent), true).OfType<RequireComponent>();

            foreach (var attribute in requiredAttributes)
            {
                if (GetComponent(attribute.Type) == null)
                {
                    throw new ArgumentException("GameObject does not contain the required components for this object to function.");
                }
            }

            component.GameObject = this;
            _components.Add(component);

            return component;
        }

        public T GetComponent<T>() where T : Component
        {
            return GetComponents<T>().FirstOrDefault();
        }
        public object GetComponent(Type t)
        {
            return Components.Where(x => x.GetType() == t).FirstOrDefault();
        }

        public IEnumerable<T> GetComponents<T>() where T : Component
        {
            return Components.OfType<T>();
        }
    }
}
