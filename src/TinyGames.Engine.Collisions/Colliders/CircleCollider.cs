using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Maths;

namespace TinyGames.Engine.Collisions
{
    // TODO several shapes and whatnot.
    public class CircleCollider : Collider
    {
        public override AABB Bounds { get; }
        public Circle Circle { get; }

        public CircleCollider(Circle circle)
        {
            Circle = circle;
            Bounds = circle.Bounds;
        }
    }
}
