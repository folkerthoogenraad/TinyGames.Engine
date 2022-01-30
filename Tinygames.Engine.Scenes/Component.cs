using System;
using System.Collections.Generic;
using System.Text;

namespace Tinygames.Engine.Scenes
{
    public class Component
    {
        public GameObject GameObject { get; set; }

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
