using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Graphics.Fonts;
using TinyGames.Engine.Maths;

namespace TinyGames.Engine.UI
{
    public class UIButton : UIComponent
    {
        public string Text { get; set; } = "";
        public Font Font { get; set; }
        public NineSideSprite Background { get; set; }

        public override void DrawSelf(Graphics2D graphics, AABB bounds)
        {
            graphics.DrawNineSideSprite(Background, bounds.Position, bounds.Size);
            graphics.DrawString(Font, Text, bounds.Center, Color.White, FontHAlign.Center, FontVAlign.Center);
        }
    }

}
