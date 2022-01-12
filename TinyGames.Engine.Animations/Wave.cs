using System;

namespace TinyGames.Engine.Animations
{
    public class Wave
    {
        public static float Sine(float input) => 1 - (MathF.Cos(input * MathF.PI * 2) * 0.5f + 0.5f);
        public static float Triangle(float input) => 1 - MathF.Abs(1 - 2 * (input % 1));
        public static float Parabola(float input) => 1 - MathF.Pow(2 * (input % 1) - 1, 2);
        public static float Sawtooth(float input) => input % 1;
    }
}
