using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Graphics.Fonts;
using TinyGames.Engine.Maths;

namespace TinyGames.Engine.UI
{
    public class UIIconButton : UIComponent
    {
        public string Text { get; set; } = "";

        public Sprite Icon { get; set; }

        public Sprite IconOverlay { get; set; }
        public Color IconOverlayColor { get; set; }

        public Font Font { get; set; }
        public NineSideSprite Background { get; set; }

        public override void DrawSelf(Graphics2D graphics, AABB bounds)
        {
            graphics.DrawNineSideSprite(Background, bounds.Position, bounds.Size);

            var innerBounds = bounds.Shrink(8);

            graphics.DrawString(Font, Text, innerBounds.RightCenter, Color.White, FontHAlign.Right, FontVAlign.Center);
            graphics.DrawSprite(Icon, innerBounds.LeftCenter + new Vector2(Icon.Width / 2, 0));

            if (IconOverlay != null)
            {
                graphics.DrawSprite(IconOverlay, innerBounds.LeftCenter + new Vector2(Icon.Width / 2, 0), 0, 0, IconOverlayColor);
            }
        }
    }

}
