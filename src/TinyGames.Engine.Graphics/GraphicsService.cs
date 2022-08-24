using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Graphics
{
    public class GraphicsService : IGraphicsService
    {
        public float Width => ScreenWidth / ScalingFactor;

        public float Height => ScreenHeight / ScalingFactor;

        public float AspectRatio => Width / Height;

        public float ScreenWidth => Device.PresentationParameters.BackBufferWidth;

        public float ScreenHeight => Device.PresentationParameters.BackBufferHeight;

        public float ScalingFactor { get; set; } = 1.0f;

        public GraphicsDevice Device { get; set; }

        public GraphicsService(GraphicsDevice device, float scaling = 1.0f)
        {
            Device = device;
            ScalingFactor = scaling;
        }
    }
}
