using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Maths.Algorithms
{
    public interface ISquarePackingAlgorithm
    {
        public Rectangle[] Pack(int width, int height, Point[] rectangleSizes);
    }
}
