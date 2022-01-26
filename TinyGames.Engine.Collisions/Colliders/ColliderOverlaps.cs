using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Maths;

namespace TinyGames.Engine.Collisions
{
    public static class ColliderOverlaps
    {
        public static bool Overlaps(this Collider self, Collider other, Vector2 relativePosition)
        {
            if(self is BoxCollider)
            {
                return OverlapsBox(self as BoxCollider, other, relativePosition);
            }
            if(self is CircleCollider)
            {
                return OverlapsCircle(self as CircleCollider, other, relativePosition);
            }

            throw new NotImplementedException();
        }

        public static bool OverlapsBox(BoxCollider self, Collider other, Vector2 relativePosition)
        {
            if(other is BoxCollider)
            {
                return self.Bounds.Overlaps(other.Bounds.Translated(relativePosition));
            }
            if(other is CircleCollider)
            {
                Circle circle = ((CircleCollider)other).Circle;

                return self.Bounds.DistanceTo(relativePosition + circle.Position) < circle.Radius;
            }

            throw new NotImplementedException();
        }
        public static bool OverlapsCircle(CircleCollider self, Collider other, Vector2 relativePosition)
        {
            if (other is BoxCollider)
            {
                return other.Bounds.DistanceTo(self.Circle.Position - relativePosition) < self.Circle.Radius;
            }
            if (other is CircleCollider)
            {
                Circle circle = ((CircleCollider)other).Circle.Translated(relativePosition);

                return circle.Overlaps(self.Circle);
            }

            throw new NotImplementedException();
        }
    }
}
