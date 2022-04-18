using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Collections
{
    public class Grid<T>
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public T[] Data { get; private set; }

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;

            Data = new T[Width * Height];
        }
        
        public void Set(int x, int y, T tile)
        {
            Data[IndexOf(x, y)] = tile;
        }

        public T Get(int x, int y)
        {
            return Data[IndexOf(x, y)];
        }

        private int IndexOf(int x, int y)
        {
            if (x < 0) x = 0;
            if (x >= Width - 1) x = Width - 1;

            if (y < 0) y = 0;
            if (y >= Height - 1) y = Height - 1;

            return x + y * Width;
        }
    }
}
