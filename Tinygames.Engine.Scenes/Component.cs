using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Scenes
{
    public class Component
    {
        public GameObject GameObject { get; set; }
        public Scene Scene => GameObject.Scene;
        public bool Initialized { get; private set; }

        public virtual void Init()
        {
            Initialized = true;
        }

        public virtual void Update(float delta)
        {

        }

        public virtual void Destroy()
        {
            Initialized = false;
        }

        public T GetComponent<T>() where T : Component
        {
            return GameObject.GetComponent<T>();
        }

        public object GetComponent(Type t)
        {
            return GameObject.GetComponent(t);
        }

        public IEnumerable<T> GetComponents<T>() where T : Component
        {
            return GameObject.GetComponents<T>();
        }
    }
}
