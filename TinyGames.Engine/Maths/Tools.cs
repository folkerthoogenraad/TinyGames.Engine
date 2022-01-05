using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Maths
{
    public static class Tools
    {
        public static float DegToRad = 0.0174532925f;
        public static float RadToDeg = 57.2957795f;

        public static float Lerp(float a, float b, float f)
        {
            return a + (b - a) * f;
        }

        public static Vector2 AngleVector(float angle)
        {
            return new Vector2(MathF.Cos(angle), MathF.Sin(angle));
        }
    }
}
