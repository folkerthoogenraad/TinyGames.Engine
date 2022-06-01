using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Maths
{
    public static class Vector2Extensions
    {
        public static Vector2 Rotated(this Vector2 i, float angle)
        {
            float s = MathF.Sin(angle);
            float c = MathF.Cos(angle);

            return new Vector2(
                c * i.X - s * i.Y,
                s * i.X + c * i.Y);
        }

        public static Vector2 Perpendicular(this Vector2 i)
        {
            return new Vector2(i.Y, -i.X);
        }

        public static Vector2 Normalized(this Vector2 i)
        {
            i.Normalize();
            return i;
        }
        public static Vector2 NormalizedOrDefault(this Vector2 i, Vector2 def)
        {
            if(i.LengthSquared() <= 0)
            {
                return def;
            }

            i.Normalize();
            return i;
        }

        public static float GetAngle(this Vector2 i)
        {
            return MathF.Atan2(i.Y, i.X);
        }
        public static float GetAngleInDegrees(this Vector2 i)
        {
            return MathF.Atan2(i.Y, i.X) * Tools.RadToDeg;
        }
        public static bool IsNan(this Vector2 i)
        {
            return float.IsNaN(i.X) || float.IsNaN(i.Y);
        }
    }
}
