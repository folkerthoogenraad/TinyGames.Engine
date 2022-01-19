using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace PinguinGame.MiniGames.Minecarts
{
    public class TilemapLayer
    {
        public string Name { get; set; }
        public Sprite[] Sprites { get; set; }
        public int Width { get; }
        public int Height { get; }

        public TilemapLayer(int width, int height)
        {
            Width = width;
            Height = height;

            Sprites = new Sprite[width * height];
        }
        public TilemapLayer(Sprite[] sprites, int width, int height)
        {
            Width = width;
            Height = height;

            if (sprites.Length != Width * Height) throw new Exception("Cannot assign sprite array with wrong length");

            Sprites = sprites;
        }

        public void SetSprite(int x, int y, Sprite sprite)
        {
            int index = GetIndex(x, y);

            if (index < 0) return;

            Sprites[index] = sprite;
        }

        public Sprite GetSprite(int x, int y)
        {
            int index = GetIndex(x, y);

            if (index < 0) return null;

            return Sprites[index];
        }

        private int GetIndex(int x, int y)
        {
            if(x < 0) return -1;
            if(x >= Width) return -1;
            if(y < 0) return -1;
            if(y >= Height) return -1;

            return x + y * Width;
        }
    }
}
