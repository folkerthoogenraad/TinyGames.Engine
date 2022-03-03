using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Levels.Contracts
{
    public class TiledLevel
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public string BackgroundColor { get; set; }
        public TiledLayer[] Layers { get; set; }
        public TiledTileset[] Tilesets { get; set; }
        public TiledProperty[] Properties { get; set; }
    }
}
