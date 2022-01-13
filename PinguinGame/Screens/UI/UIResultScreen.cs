using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Player;
using PinguinGame.Screens.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Animations;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Graphics.Fonts;
using TinyGames.Engine.Maths;
using TinyGames.Engine.UI;
using TinyGames.Engine.UI.Animations;
using TinyGames.Engine.Util;

namespace PinguinGame.Screens.UI
{
    public class UIBanner : UIComponent
    {
        public Font Font { get; set; }
        public string Text { get; set; }
        public Color Color { get; set; }
        public Sprite Banner { get; set; }
        public Sprite BannerOverlay { get; set; }

        public override void DrawSelf(Graphics2D graphics, AABB bounds)
        {
            base.DrawSelf(graphics, bounds);

            graphics.DrawSprite(Banner, bounds.Center, 0, 0, Color);
            graphics.DrawSprite(BannerOverlay, bounds.Center, 0, 0, Color.White);
            graphics.DrawString(Font, Text, bounds.Center, Color.White, FontHAlign.Center, FontVAlign.Center);
        }
    }

    public class UIResultScreen : UIComponent
    {
        public UIBackground Background { get; set; }
        public UISprite Title { get; set; }
        public UISprite Goblet { get; set; }
        public UIBanner Banner { get; set; }

        private ResultsResources _resources;

        public UIResultScreen(ResultsResources resources, string name, Color color)
        {
            _resources = resources;

            Background = new UIBackground() { 
                BackgroundColor = resources.BackgroundColor,
                BackgroundOverlayColor = resources.BackgroundOverlayColor,
                BackgroundOverlay= resources.BackgroundOverlay,
            };

            Title = new UISprite()
            {
                Sprite = resources.TitleWin
            };
            Goblet = new UISprite()
            {
                Sprite = resources.Goblet
            };
            Banner = new UIBanner()
            {
                Banner = resources.Banner,
                BannerOverlay = resources.BannerOverlay,
                Font = resources.Font,
                Color = color,
                Text = name,
            };

            AddComponent(Background);
            AddComponent(Goblet);
            AddComponent(Banner);
            AddComponent(Title);

            FadeIn();
        }
        public override void UpdateLayout(AABB bounds)
        {
            base.UpdateLayout(bounds);

            // Manual layouting
            Background.UpdateLayout(bounds);
            Title.UpdateLayout(AABB.Create(bounds.TopCenter + new Vector2(0, 32), Vector2.Zero));

            Goblet.UpdateLayout(AABB.Create(bounds.Center + new Vector2(0, 16), Vector2.Zero));
            Banner.UpdateLayout(AABB.Create(bounds.Center + new Vector2(0, 24), Vector2.Zero));
        }

        public void FadeIn()
        {
            Title.SetAnimation(new UITransformAnimation(new Vector2(0, -16), new Vector2(1.2f, 1.2f)));
            Goblet.SetAnimation(new UITransformAnimation(new Vector2(0, 16), new Vector2(1.4f, 1.4f)));
            Banner.SetAnimation(new UITransformAnimation(new Vector2(0, 32), new Vector2(1.8f, 1.8f)));
        }

        public void FadeOut()
        {

        }

    }
}
