using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Maths
{
    public struct Line
    {
        public Vector2 From;
        public Vector2 To;

        public Line(Vector2 from, Vector2 to)
        {
            From = from;
            To = to;
        }

        public Vector2 Reflect(Vector2 input)
        {
            Vector2 relative = input - From;
            Vector2 normal = Normal;

            float distance = Vector2.Dot(normal, relative);

            return input - distance * 2 * normal;
        }

        public float SignedDistanceDeterminant(Vector2 point)
        {
            return ((To.X - From.X) * (point.Y - From.Y) - (To.Y - From.Y) * (point.X - From.X));
        }
        public float SignedDistance(Vector2 point)
        {
            return SignedDistanceDeterminant(point) / Length;
        }

        public float GetProjection(Vector2 point)
        {
            Vector2 dir = Direction;
            float sqrLength = dir.LengthSquared();

            // If length of line is zero
            if (sqrLength == 0) return Vector2.Distance(From, point);

            Vector2 offset = point - From;

            return Vector2.Dot(Direction, offset) / sqrLength;
        }

        public float GetProjectionClamped(Vector2 point)
        {
            return Math.Clamp(GetProjection(point), 0, 1);
        }

        public Vector2 GetProjectedPoint(Vector2 point)
        {
            return From + GetProjection(point) * Direction;
        }

        public Vector2 GetProjectedPointClamped(Vector2 point)
        {
            return From + GetProjectionClamped(point) * Direction;
        }

        public float Distance(Vector2 point)
        {
            return Vector2.Distance(point, GetProjectedPoint(point));
        }

        public float DistanceClamped(Vector2 point)
        {
            return Vector2.Distance(point, GetProjectedPointClamped(point));
        }

        public bool IsOnLeft(Vector2 point)
        {
            return SignedDistanceDeterminant(point) < 0;
        }
        public bool IsOnRight(Vector2 point)
        {
            return SignedDistanceDeterminant(point) > 0;
        }

        public float LengthSquared => Vector2.DistanceSquared(From, To);
        public float Length => Vector2.Distance(From, To);

        public Vector2 Origin => From;
        public Vector2 Direction => (To - From);
        public Vector2 Tangent => Direction.Normalized();
        public Vector2 Normal => Tangent.Perpendicular();
    }
}
