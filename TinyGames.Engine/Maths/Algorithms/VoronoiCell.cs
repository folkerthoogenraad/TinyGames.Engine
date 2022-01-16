using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Maths.Algorithms
{
    public struct VoronoiCell
    {
        public DelaunayPoint[] Points { get; set; }
        public int Index { get; set; }
        public VoronoiCell(int triangleIndex, DelaunayPoint[] points)
        {
            Points = points;
            Index = triangleIndex;
        }
    }

}
