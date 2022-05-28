using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Graphics
{
    public interface IDrawable2D
    {
        public int LayerIndex { get; }
        public void Draw(Graphics2D graphics);
    }
}
