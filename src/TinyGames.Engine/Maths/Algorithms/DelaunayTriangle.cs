using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Maths.Algorithms
{
    public struct DelaunayTriangle
    {
        public int Index { get; set; }
        public IEnumerable<DelaunayPoint> Points { get; set; }

        public DelaunayTriangle(int t, IEnumerable<DelaunayPoint> points)
        {
            Points = points;
            Index = t;
        }
    }

}
