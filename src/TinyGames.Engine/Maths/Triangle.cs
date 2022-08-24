using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Maths
{
    public struct Triangle
    {
        public Vector2 A { get; set; }
        public Vector2 B { get; set; }
        public Vector2 C { get; set; }

        public Vector2 Origin => A;
        public Vector2 LegA => B - A;
        public Vector2 LegB => C - A;
        public Vector2 LegHind => C - B;

        public IEnumerable<Line> Lines => GetLines();

        public Triangle(Vector2 a, Vector2 b, Vector2 c)
        {
            A = a;
            B = b;
            C = c;
        }

        public float SurfaceArea()
        {
            Vector2 v1 = LegA;
            Vector2 v2 = LegB;

            // Cross product
            // Half times parallelogram
            return 0.5f * (v1.X * v2.Y) - (v1.Y * v2.X);
        }

        public bool PointInside(Vector2 point)
        {
            Line initial = new Line(A, B);

            bool sign = initial.IsOnRight(point);

            if (sign != new Line(B, C).IsOnRight(point)) return false;
            if (sign != new Line(C, A).IsOnRight(point)) return false;

            return true;
        }

        private IEnumerable<Line> GetLines()
        {
            yield return new Line(A, B);
            yield return new Line(B, C);
            yield return new Line(C, A);
        }
    }
}
