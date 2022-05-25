using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Collections
{
    public enum GridSamplingBehaviour
    {
        None,
        Clamp,
        Wrap,
    }

    public class Grid<T>
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public T[] Data { get; private set; }

        public GridSamplingBehaviour SamplingBehaviour { get; set; } = GridSamplingBehaviour.Clamp;

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
            if(SamplingBehaviour == GridSamplingBehaviour.Wrap)
            {
                x %= Width;
                y %= Height;

                // Negative numbers stay negative in modulo
                if (x < 0) x += Width;
                if (y < 0) y += Height;
            }
            else if(SamplingBehaviour == GridSamplingBehaviour.Clamp)
            {
                if (x < 0) x = 0;
                if (x >= Width - 1) x = Width - 1;

                if (y < 0) y = 0;
                if (y >= Height - 1) y = Height - 1;
            }

            return x + y * Width;
        }

        public IEnumerable<T> GetInBox(int x, int y, int width, int height)
        {
            // Maybe check the box for bounds? But this is kinda defined in the clamping behaviour right?
            // Maybe a different box type check sometime? Idk.

            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    yield return Get(x + i, y + j);
                }
            }
        }
    }
}
