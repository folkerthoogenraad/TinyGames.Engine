using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.IO
{
    public class Sprite
    {
        public string Texture { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        
        public int Width { get; set; }
        public int Height { get; set; }

        public int OriginX { get; set; } = 0;
        public int OriginY { get; set; } = 0;
    }
}
