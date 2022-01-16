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

        public Triangle(Vector2 a, Vector2 b, Vector2 c)
        {
            A = a;
            B = b;
            C = c;
        }
    }
}
