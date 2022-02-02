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

        public bool GetBoolProperty(string name, bool def = false)
        {
            if (Properties != null)
            {
                return Properties.Where(x => x.Name == name).Any(x => x.GetValueAsBool());
            }
            return def;
        }
        public int GetIntProperty(string name, int def = 0)
        {
            if (Properties != null)
            {
                return Properties.Where(x => x.Name == name).Select(x => x.GetValueAsInt()).FirstOrDefault();
            }
            return def;
        }
        public string GetStringProperty(string name, string def = null)
        {
            if (Properties != null)
            {
                return Properties.Where(x => x.Name == name).Select(x => x.GetValueAsString()).FirstOrDefault();
            }

            return def;
        }
        public float GetFloatProperty(string name, float def = 0)
        {
            if (Properties != null)
            {
                return Properties.Where(x => x.Name == name).Select(x => x.GetValueAsFloat()).FirstOrDefault();
            }

            return def;
        }
    }
}
