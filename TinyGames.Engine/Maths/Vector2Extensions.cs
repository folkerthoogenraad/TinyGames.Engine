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
    }
}
