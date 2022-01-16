using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Maths;

namespace TinyGames.Engine.Extensions
{
    public static class RandomExtension
    {
        public static float NextFloat(this Random random)
        {
            return (float)random.NextDouble();
        }
        public static float NextFloatNormalized(this Random random)
        {
            return (float)(random.NextDouble() * 2 - 1);
        }

        public static float NextFloatRange(this Random random, float min, float max)
        {
            return Tools.Lerp(min, max, random.NextFloat());
        }

        public static Vector2 RandomPointInBox(this Random random, float minX, float maxX, float minY, float maxY)
        {
            return new Vector2(random.NextFloatRange(minX, maxX), random.NextFloatRange(minY, maxY));
        }
        public static Vector2 RandomPointInCircle(this Random random, Vector2 center, float radius)
        {
            float r = radius * MathF.Sqrt(random.NextFloat());
            float angle = random.NextFloat() * MathF.PI * 2;

            return Tools.AngleVector(angle) * r;
        }

        public static Vector2 RandomPointInCircle(this Random random)
        {
            float r = MathF.Sqrt(random.NextFloat());
            float angle = random.NextFloat() * MathF.PI * 2;

            return Tools.AngleVector(angle) * r;
        }
        public static Vector2 RandomPointInDonut(this Random random, float minRadius, float maxRadius)
        {
            float percentage = minRadius / maxRadius;
            float r = MathF.Sqrt(percentage + (1 - percentage) * random.NextFloat()) * maxRadius;
            float angle = random.NextFloat() * MathF.PI * 2;

            return Tools.AngleVector(angle) * r;
        }
    }
}
