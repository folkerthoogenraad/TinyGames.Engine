using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.UI.Animations
{
    public abstract class UIAnimation
    {
        public virtual bool Done => false;
        public virtual bool Visible => true;

        public virtual Vector2? Scale => null;
        public virtual Vector2? Position => null;
        public virtual float? Rotation => null;
        public virtual float Alpha => 1;

        public abstract void Update(float delta);
    }

}
