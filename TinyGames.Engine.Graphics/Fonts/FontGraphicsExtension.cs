using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Graphics.Fonts
{
    public static class FontGraphicsExtension
    {
        public static void DrawString(this Graphics2D graphics, Font font, string text, Vector2 position, Color color, FontHAlign halign = FontHAlign.Left, FontVAlign valign = FontVAlign.Top)
        {
            font.DrawString(graphics, text, position, Vector2.One, color, halign, valign);
        }
        public static void DrawString(this Graphics2D graphics, Font font, string text, Vector2 position, Vector2 scale, Color color, FontHAlign halign = FontHAlign.Left, FontVAlign valign = FontVAlign.Top)
        {
            font.DrawString(graphics, text, position, scale, color, halign, valign);
        }
    }
}
