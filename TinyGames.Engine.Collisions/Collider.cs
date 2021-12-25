using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Maths;

namespace TinyGames.Engine.Collisions
{
    // TODO several shapes and whatnot.
    public class Collider
    {
        public AABB Bounds { get; }

        public Collider(AABB bounds)
        {
            Bounds = bounds;
        }
    }
}
