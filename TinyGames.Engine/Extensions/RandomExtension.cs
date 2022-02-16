using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TinyGames.Engine.Maths;
using TinyGames.Engine.Util;

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
        public static float NextAngle(this Random random)
        {
            return (float)random.NextDouble() * 360;
        }

        public static T Choose<T>(this Random random, params T[] input)
        {
            return input.Random(random);
        }

        public static Vector2 NextPointInBox(this Random random, float minX, float maxX, float minY, float maxY)
        {
            return new Vector2(random.NextFloatRange(minX, maxX), random.NextFloatRange(minY, maxY));
        }
        public static Vector2 RandomPointInCircle(this Random random, Vector2 center, float radius)
        {
            float r = radius * MathF.Sqrt(random.NextFloat());
            float angle = random.NextFloat() * MathF.PI * 2;

            return Tools.AngleVector(angle) * r;
        }

        public static Vector2 NextPointInCircle(this Random random)
        {
            float r = MathF.Sqrt(random.NextFloat());
            float angle = random.NextFloat() * MathF.PI * 2;

            return Tools.AngleVector(angle) * r;
        }
        public static Vector2 NextPointInDonut(this Random random, float minRadius, float maxRadius)
        {
            float percentage = minRadius / maxRadius;
            float r = MathF.Sqrt(percentage + (1 - percentage) * random.NextFloat()) * maxRadius;
            float angle = random.NextFloat() * MathF.PI * 2;

            return Tools.AngleVector(angle) * r;
        }

        public static Vector2 NextPointInTriangle(this Random random, Triangle triangle)
        {
            Vector2 offset = triangle.LegA * random.NextFloat() + triangle.LegB * random.NextFloat();
            Vector2 position = offset + triangle.Origin;

            if (!triangle.PointInside(position))
            {
                position = new Line(triangle.B, triangle.C).Reflect(position);
            }

            return position;
        }

        public static IEnumerable<Vector2> NextPointsInPolygon(this Random random, Polygon polygon, int count)
        {
            (Triangle Triangle, float SurfaceArea)[] triangles = polygon.Triangles.Select(x => (x, x.SurfaceArea())).ToArray();
            float totalSurfaceArea = triangles.Sum(x => x.SurfaceArea);

            for(int i = 0; i < count; i++)
            {
                float surface = random.NextFloat() * totalSurfaceArea;
                int triangleIndex = 0;

                while(surface > 0)
                {
                    surface -= triangles[triangleIndex].SurfaceArea;

                    if(surface < 0)
                    {
                        yield return random.NextPointInTriangle(triangles[triangleIndex].Triangle);
                        break;
                    }

                    triangleIndex++;
                }
            }
        }
    }
}
