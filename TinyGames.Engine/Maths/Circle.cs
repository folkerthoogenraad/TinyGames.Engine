using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Maths
{
    public struct Circle
    {
        public Vector2 Position { get; set; }
        public float Radius { get; set; }

        public float Diameter => Radius * 2;
        public float Circumference => 2 * MathF.PI * Radius;
        public float SurfaceArea => MathF.PI * Radius * Radius;

        public Circle(Vector2 position, float radius)
        {
            Position = position;
            Radius = radius;
        }
        public Circle(float radius)
        {
            Radius = radius;
            Position = Vector2.Zero;
        }

        public AABB Bounds => AABB.CreateCentered(Position, new Vector2(Diameter, Diameter));

        public Circle Clone()
        {
            return new Circle(Position, Radius);
        }

        public Circle Translate(Vector2 position)
        {
            Position += position;
            return this;
        }

        public Circle Translated(Vector2 position)
        {
            return Clone().Translate(position);
        }

        public bool Overlaps(Circle other)
        {
            var distance = Vector2.Distance(Position, other.Position);

            return distance < other.Radius + Radius;
        }
    }
}
