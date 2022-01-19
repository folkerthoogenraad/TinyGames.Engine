using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Levels.Contracts
{
    public class TiledTileset
    {
        // Stats
        public string Name { get; set; }

        // Images
        public string Image { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }

        // Tiles
        public int TileCount { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public int FirstGID { get; set; }
        public int Columns { get; set; }
    }
}
