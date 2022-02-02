using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TinyGames.Engine.Util;

namespace TinyGames.Engine.Maths
{
    /// <summary>
    /// Only really works for convex polygons
    /// </summary>
    public class Polygon
    {
        public Vector2[] Points;

        public Polygon(Vector2[] points)
        {
            Points = points;
        }

        public Vector2 ClosestPoint(Vector2 v)
        {
            // TODO line should have a function to calculate the projected point _and_ distance in one go.
            return Lines.OrderBy(x => x.DistanceClamped(v)).Select(x => x.GetProjectedPointClamped(v)).First();
        }

        public float DistanceTo(Vector2 v)
        {
            var closests = ClosestPoint(v);

            return (v - closests).Length();
        }

        public bool Inside(Vector2 v)
        {
            return Lines.All(x => x.IsOnRight(v));
        }

        public IEnumerable<Line> Lines => Points.Loop().Select((points) => new Line(points.Item1, points.Item2));
        public IEnumerable<Triangle> Triangles => GetTriangles();

        public Vector2 Center => Points.Aggregate((a, b) => a + b) / Points.Length;

        public IEnumerable<Triangle> GetTriangles()
        {
            var center = Center;

            return Lines.Select(x => new Triangle(x.From, x.To, center));
        }

        public Polygon Translated(Vector2 v)
        {
            return Clone().Translate(v);
        }

        public Polygon Translate(Vector2 v)
        {
            for(int i = 0; i < Points.Length; i++)
            {
                Points[i] = Points[i] + v;
            }
            return this;
        }

        public Polygon Clone()
        {
            return new Polygon(Points.ToArray());
        }

        public void Reverse()
        {
            Points = Points.Reverse().ToArray();
        }

        public bool IsClockwise()
        {
            return GetWindingNumber() < 0;
        }
        public bool IsCounterClockwise()
        {
            return !IsClockwise();
        }

        public float GetWindingNumber()
        {
            // (x2 - x1) * (y2 + y1)
            return Points.Loop().Select(t => (t.Item2.X - t.Item1.X) * (t.Item2.Y + t.Item1.Y)).Sum();
        }
    }
}
