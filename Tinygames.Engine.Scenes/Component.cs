using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Scenes
{
    public class Component
    {
        public GameObject GameObject { get; set; }
        public Scene Scene => GameObject.Scene;

        public virtual void Init()
        {

        }

        public virtual void Update(float delta)
        {

        }

        public virtual void Destroy()
        {

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
