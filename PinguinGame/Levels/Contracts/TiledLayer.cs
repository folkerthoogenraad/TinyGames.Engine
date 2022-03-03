using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Levels.Contracts
{
    public class TiledLayer
    {
        public const string LayerTypeObjectGroup = "objectgroup";
        public const string LayerTypeTileLayer = "tilelayer";

        public string Name { get; set; }
        public string Type { get; set; }

        public bool Visible { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int[] Data { get; set; }

        public TiledObject[] Objects { get; set; }
        public TiledProperty[] Properties { get; set; }

        public bool IsObjectLayer => Type == LayerTypeObjectGroup;
        public bool IsTileLayer => Type == LayerTypeTileLayer;

        public int GetTileAt(int x, int y)
        {
            return Data[x + y * Width];
        }
    }
}
