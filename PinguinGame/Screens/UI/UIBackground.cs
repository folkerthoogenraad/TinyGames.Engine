using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Animations;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;
using TinyGames.Engine.UI;

namespace PinguinGame.Screens.UI
{
    public class UIBackgroundColor : UIComponent
    {
        public Color BackgroundColor { get; set; }

        public override void DrawSelf(Graphics2D graphics, AABB bounds)
        {
            graphics.DrawRectangle(bounds, BackgroundColor); 
        }
    }

    public class UIBackground : UIComponent
    {
        public Color BackgroundColor { get; set; }
        public Color BackgroundOverlayColor { get; set; }
        public Texture2D BackgroundOverlay { get; set; }

        private float Timer = 0;

        public override void Update(float delta)
        {
            base.Update(delta);

            Timer += delta * 0.3f;
        }

        public override void DrawSelf(Graphics2D graphics, AABB bounds)
        {
            graphics.DrawRectangle(bounds, BackgroundColor);

            if(BackgroundOverlay != null)
            {
                graphics.DrawTextureRegion(BackgroundOverlay, TextureBounds, bounds.Position, bounds.Size, BackgroundOverlayColor);
            }
        }

        private AABB TextureBounds => new AABB()
        {
            Left = Bounds.Left + Timer * 16,
            Right = Bounds.Right + Timer * 16,
            Top = Bounds.Top + Wave.Sine(Timer) * 16,
            Bottom = Bounds.Bottom + Wave.Sine(Timer) * 16,
        };
    }
}
