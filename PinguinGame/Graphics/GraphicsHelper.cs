using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Animations;

namespace PinguinGame.Graphics
{
    public class GraphicsHelper
    {
        public static float YToDepth(float y)
        {
            return y / 1024;
        }
        public static float DepthToY(float depth)
        {
            return depth * 1024;
        }

        public static float YToDepth(Vector2 input)
        {
            return YToDepth(input.Y);
        }

        public static float DebugSinTime(float offset = 0, float frequency = 1)
        {
            return Wave.Sine(PenguinGame.DebugRunTime * frequency + offset);
        }
    }
}
