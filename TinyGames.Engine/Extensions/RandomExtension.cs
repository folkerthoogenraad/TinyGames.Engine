using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
