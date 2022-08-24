using System;

namespace TinyGames.Engine.Animations
{
    public class Ease
    {
        public static float Linear(float f) => f;
        public static float EaseIn(float f) => MathF.Pow(f, 2);
        public static float EaseOut(float f) => 1 - MathF.Pow(f - 1, 2);
        public static float EaseInOut(float f) => 1 - (MathF.Cos(f * MathF.PI) * 0.5f + 0.5f);
        public static float Overshoot(float f) => (1 - 2 * (f - 1)) * f;
    }
}
