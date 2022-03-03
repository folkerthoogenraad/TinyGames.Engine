using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Gameplay
{
    public class Tilemap
    {
        public int Width { get; }
        public int Height { get; }
        public int TileWidth { get; }
        public int TileHeight { get; }

        public int PixelWidth => Width * TileWidth;
        public int PixelHeight => Height * TileHeight;

        public Vector2 PixelSize => new Vector2(PixelWidth, PixelHeight);

        public TilemapLayer[] Layers { get; }

        public Tilemap(TilemapLayer[] layers, int width, int height, int tileWidth, int tileHeight)
        {
            Layers = layers;
            Width = width;
            Height = height;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
        }

        public Vector2 GetTilePosition(Point p)
        {
            return GetTilePosition(p.X, p.Y);
        }
        public Vector2 GetTilePosition(int x, int y)
        {
            return new Vector2(x * TileWidth, y * TileHeight);
        }
        public Vector2 GetTileCenter(int x, int y)
        {
            return GetTilePosition(x, y) + PixelSize / 2;
        }

        public Point GetTilemapPosition(Vector2 v)
        {
            return new Point((int)(v.X / TileWidth), (int)(v.Y / TileHeight));
        }
    }
}
