using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;

namespace PinguinGame.Screens.Fades
{
    public class ColorFade : Fade
    {
        public Color Color { get; set; }

        public ColorFade(Color color)
        {
            Color = color;
        }

        public override void Draw(Graphics2D graphics, float amount, AABB bounds)
        {
            graphics.DrawRectangle(bounds, new Color(Color, amount));
        }
    }
}
