using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Levels.Contracts
{
    public class TiledObjectPolygonCoordinate
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }
    }
}
