using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentBikeGame
{
    public class TextureSampler
    {
        public Color[] Colors { get; }
        public int Width { get; }
        public int Height { get; }

        public TextureSampler(Color[] colors, int width, int height)
        {
            Colors = colors;
            Width = width;
            Height = height;
        }

        public Color GetColor(Vector2 v)
        {
            return GetColor(v.X, v.Y);
        }

        public Color GetColor(float x, float y)
        {
            return GetColor((int)x, (int)y);
        }

        public Color GetColor(int x, int y)
        {
            x = x % Width;
            y = y % Height;

            if (x < 0) x += Width;
            if (y < 0) y += Width;

            return Colors[x + y * Width];
        }

        public static TextureSampler FromTexture(Texture2D texture)
        {
            Color[] colors = new Color[texture.Width * texture.Height];
            texture.GetData(colors);

            return new TextureSampler(colors, texture.Width, texture.Height);
        }
    }
}
