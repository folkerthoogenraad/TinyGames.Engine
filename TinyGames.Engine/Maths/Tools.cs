using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TinyGames.Engine.Maths.Algorithms.SquarePacking;

namespace TinyGames.Engine.Maths
{
    public static class Tools
    {
        public static float DegToRad = 0.0174532925f;
        public static float RadToDeg = 57.2957795f;

        public static float Lerp(float a, float b, float f)
        {
            return a + (b - a) * f;
        }

        public static Vector2 AngleVector(float angle)
        {
            return new Vector2(MathF.Cos(angle), MathF.Sin(angle));
        }

        public static bool IsPowerOfTwo(int x)
        {
            return (x & (x - 1)) == 0;
        }

        public static Rectangle[] PackRectangles(int width, int height, Point[] rectangleSizes)
        {
            QuadTreeSquarePackingAlgorithm algorithm = new QuadTreeSquarePackingAlgorithm();

            return algorithm.Pack(width, height, rectangleSizes);
        }

        public static IEnumerable<S> PackRectangles<T, S>(this IEnumerable<T> list, int width, int height, Func<T, Point> sizeSelector, Func<T, Rectangle, S> resultFunc)
        {
            T[] array = list.ToArray();
            Point[] rectangleSizes = array.Select(sizeSelector).ToArray();
            Rectangle[] rectangles = PackRectangles(width, height, rectangleSizes);

            return list.Zip(rectangles, resultFunc);
        }
    }
}
