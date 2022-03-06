using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.IO
{
    public class IOSprite
    {
        public string Texture { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        
        public int Width { get; set; }
        public int Height { get; set; }

        public int OriginX { get; set; } = 0;
        public int OriginY { get; set; } = 0;

        public IOSprite()
        {

        }

        public IOSprite(string texture, int x, int y, int width, int height, int originX = 0, int originY = 0)
        {
            Texture = texture;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            OriginX = originX;
            OriginY = originY;
        }

        public IOSprite(string texture, Rectangle rect, int originX = 0, int originY = 0) : this(texture, rect.X, rect.Y, rect.Width, rect.Height, originX, originY) { }

        public IOSprite SetOrigin(int x, int y)
        {
            OriginX = x;
            OriginY = y;

            return this;
        }
    }
}
