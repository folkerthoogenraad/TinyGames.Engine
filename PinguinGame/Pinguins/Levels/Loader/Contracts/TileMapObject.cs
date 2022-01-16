using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PinguinGame.Pinguins.Levels.Loader.Contracts
{
    public class TileMapObject
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public TileMapObjectPolygonCoordinate[] Polygon { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public TileMapObjectProperty[] Properties { get; set; }

        public bool GetBoolProperty(string name, bool def = false)
        {
            if (Properties != null)
            {
                return Properties.Where(x => x.Name == name).Any(x => x.GetValueAsBool());
            }
            return def;
        }
    }
}
