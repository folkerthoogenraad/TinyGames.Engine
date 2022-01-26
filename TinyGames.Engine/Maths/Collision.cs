using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Maths
{
    public static class Collision
    {
        public static Vector2 Unstuck(Circle a, Circle b)
        {
            // TODO MinkowskiDifference instead
            Vector2 direction = b.Position - a.Position;

            float length = direction.Length();

            if(length == 0) return Vector2.UnitX * (a.Radius + b.Radius);

            direction /= length;

            return direction * (length - (a.Radius + b.Radius));
        }

        public static Vector2 Unstuck(AABB a, AABB b)
        {
            var minkow = AABB.MinkowskiDifference(b, a);

            return minkow.Unstuck(Vector2.Zero);
        }

        public static Vector2 Unstuck(AABB a, Circle b)
        {
            Vector2 closest = a.ClampPoint(b.Position);

            return Unstuck(new Circle(closest, 0), b);
        }

        public static Vector2 Unstuck(Circle a, AABB b)
        {
            return -Unstuck(b, a);
        }
    }
}
