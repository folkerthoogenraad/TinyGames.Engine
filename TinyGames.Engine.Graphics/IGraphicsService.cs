using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Graphics
{
    public interface IGraphicsService
    {
        public float Width { get; }
        public float Height { get; }
        public float AspectRatio { get; }

        public float ScreenWidth { get; }
        public float ScreenHeight { get; }

        public GraphicsDevice Device { get; }
    }
}
