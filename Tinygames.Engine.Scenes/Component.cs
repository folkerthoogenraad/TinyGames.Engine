using System;
using System.Collections.Generic;
using System.Text;

namespace Tinygames.Engine.Scenes
{
    public class Component
    {
        public bool Initialized { get; set; }
        public bool Destroyed { get; set; }
        public GameObject GameObject { get; set; }

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
    }
}
