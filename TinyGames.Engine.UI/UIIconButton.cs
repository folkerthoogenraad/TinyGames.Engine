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
        public Color BackgroundColor { get; set; } = Color.White;

        public override void DrawSelf(Graphics2D graphics, AABB bounds)
        {
            if(Background != null)
            {
                graphics.DrawNineSideSprite(Background, bounds.Position, bounds.Size, BackgroundColor);
            }

            if(Icon != null)
            {
                var innerBounds = bounds.Shrink(8);

                var iconPosition = innerBounds.Center;

                if (Text.Length > 0)
                {
                    iconPosition = innerBounds.LeftCenter + new Vector2(Icon.Width / 2, 0);
                    graphics.DrawString(Font, Text, innerBounds.RightCenter, Color.White, FontHAlign.Right, FontVAlign.Center);
                }

                graphics.DrawSprite(Icon, iconPosition);

                if (IconOverlay != null)
                {
                    graphics.DrawSprite(IconOverlay, iconPosition, 0, 0, IconOverlayColor);
                }
            }
            else
            {
                if (Text.Length > 0)
                {
                    graphics.DrawString(Font, Text, bounds.Center, Color.White, FontHAlign.Center, FontVAlign.Center);
                }
            }
        }
    }

}
