using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Maths.Algorithms
{
    public struct DelaunayEdge
    {
        public DelaunayPoint P { get; set; }
        public DelaunayPoint Q { get; set; }
        public int Index { get; set; }

        public DelaunayEdge(int e, DelaunayPoint p, DelaunayPoint q)
        {
            Index = e;
            P = p;
            Q = q;
        }
    }

}
