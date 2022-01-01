using System;
using System.Collections.Generic;
using System.Text;

namespace Tinygames.Engine.Scenes
{
    public class GameObject
    {
        public bool Initialized { get; set; }
        public bool Destroyed { get; set; }

        public Scene Scene { get; set; }

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
