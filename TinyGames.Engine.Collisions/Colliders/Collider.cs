using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Maths;

namespace TinyGames.Engine.Collisions
{
    // TODO several shapes and whatnot.
    public abstract class Collider
    {
        public abstract AABB Bounds { get; }
    }
}
