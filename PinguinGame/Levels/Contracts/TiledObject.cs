using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PinguinGame.Levels.Contracts
{
    public class TiledObject
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public TiledObjectPolygonCoordinate[] Polygon { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public TiledProperty[] Properties { get; set; }

        public Vector2 Position => new Vector2(X, Y);
        public Vector2 Size => new Vector2(Width, Height);
        public Vector2 Center => Position + Size / 2;
    }
}
