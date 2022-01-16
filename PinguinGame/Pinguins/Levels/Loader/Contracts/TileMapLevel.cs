using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Pinguins.Levels.Loader.Contracts
{
    public class TileMapLevel
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public TileMapLayer[] Layers { get; set; }
    }
}
