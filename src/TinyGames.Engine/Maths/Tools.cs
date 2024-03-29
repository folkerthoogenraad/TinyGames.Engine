﻿using Microsoft.Xna.Framework;
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
        public static bool Lerp(bool a, bool b, float f)
        {
            return Lerp(a, b, f, 0.01f);
        }
        public static bool Lerp(bool a, bool b, float f, float threshold)
        {
            if (f > threshold) return b;
            return a;
        }

        public static float Clamp(float min, float max, float value)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
        public static float Clamp01(float value)
        {
            if (value < 0) return 0;
            if (value > 1) return 1;
            return value;
        }

        public static Vector2 AngleVector(float angle)
        {
            return new Vector2(MathF.Cos(angle), MathF.Sin(angle));
        }

        public static Vector2 RotateVector(Vector2 v, float angle)
        {
            float sin = MathF.Sin(angle);
            float cos = MathF.Cos(angle);

            return new Vector2(
                v.X * cos + v.Y * -sin,
                v.X * sin + v.Y * cos
                );
        }

        public static bool IsPowerOfTwo(int x)
        {
            return (x & (x - 1)) == 0;
        }

        public static float AngleLerp(float from, float to, float f)
        {
            float add = ShortestAngle(from, to);

            return from + add * f;
        }

        public static float ShortestAngle(float from, float to)
        {
            float angle = to - from;

            if (angle > MathF.PI) return angle - MathF.PI * 2;
            if (angle < -MathF.PI) return angle + MathF.PI * 2;

            return angle;
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
