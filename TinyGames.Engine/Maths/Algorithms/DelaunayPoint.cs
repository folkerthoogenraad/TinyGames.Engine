using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Maths.Algorithms
{
    public struct DelaunayPoint
    {
        public double X { get; set; }
        public double Y { get; set; }

        public DelaunayPoint(double x, double y)
        {
            X = x;
            Y = y;
        }
        public override string ToString() => $"{X},{Y}";
    }

}
