using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Maths;

namespace TinyGames.Engine.Collisions
{
    public static class ColliderPenetration
    {
        public static Vector2 Penetration(this Collider self, Collider other, Vector2 relativePosition)
        {
            if(self is BoxCollider)
            {
                return PenetrationBox(self as BoxCollider, other, relativePosition);
            }
            if(self is CircleCollider)
            {
                return PenetrationCircle(self as CircleCollider, other, relativePosition);
            }

            throw new NotImplementedException();
        }

        public static Vector2 PenetrationBox(BoxCollider self, Collider other, Vector2 relativePosition)
        {
            if(other is BoxCollider)
            {
                return Collision.Unstuck(self.Bounds, other.Bounds.Translated(relativePosition));
            }
            if(other is CircleCollider)
            {
                Circle circle = ((CircleCollider)other).Circle;

                return Collision.Unstuck(self.Bounds, circle.Translated(relativePosition));
            }

            throw new NotImplementedException();
        }
        public static Vector2 PenetrationCircle(CircleCollider self, Collider other, Vector2 relativePosition)
        {
            if (other is BoxCollider)
            {
                return Collision.Unstuck(self.Circle, other.Bounds.Translated(relativePosition));
            }
            if (other is CircleCollider)
            {
                Circle circle = ((CircleCollider)other).Circle.Translated(relativePosition);

                return Collision.Unstuck(self.Circle, circle);
            }

            throw new NotImplementedException();
        }
    }
}
