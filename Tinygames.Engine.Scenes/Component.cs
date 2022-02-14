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
    }
}
