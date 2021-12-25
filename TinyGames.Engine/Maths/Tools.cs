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
    }
}
