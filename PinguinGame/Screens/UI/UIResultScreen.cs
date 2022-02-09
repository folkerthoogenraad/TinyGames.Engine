using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Player;
using PinguinGame.Screens.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using TinyGames.Engine.Animations;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Graphics.Fonts;
using TinyGames.Engine.Maths;
using TinyGames.Engine.UI;
using TinyGames.Engine.UI.Animations;
using TinyGames.Engine.Util;

namespace PinguinGame.Screens.UI
{
    public class UIResultsScreenModel
    {
        public string BannerText { get; set; }
        public Color BannerColor  { get; set; }
        public string BackLabel { get; set; }
        public UIResultLineModel[] Lines;
    }

    public class UIGoblet : UIComponent
    {
        public UISprite Goblet { get; set; }
        public UIBanner Banner { get; set; }

        public UIGoblet(Font font, Sprite goblet, Sprite banner, Sprite bannerOverlay, string name, Color color)
        {
            Goblet = new UISprite()
            {
                Sprite = goblet
            };
            Banner = new UIBanner()
            {
                Banner = banner,
                BannerOverlay = bannerOverlay,
                Font = font,
                Color = color,
                Text = name,
            };


            AddComponent(Goblet);
            AddComponent(Banner);
        }

        public override void UpdateLayout(AABB bounds)
        {
            // TODO update layout isn't quite correct
            base.UpdateLayout(bounds);

            Goblet.UpdateLayout(AABB.Create(Vector2.Zero, Vector2.Zero));
            Banner.UpdateLayout(AABB.Create(new Vector2(0, 16), Vector2.Zero));
        }

        public override void DrawSelf(Graphics2D graphics, AABB bounds)
        {
            base.DrawSelf(graphics, bounds);

            //graphics.DrawCircle(bounds.Center, 20, Color.Blue);
        }
    }

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
        public UIGoblet Goblet { get; set; }
        public UIButton OK { get; set; }

        public UIResultLine[] Lines { get; set; }

        private ResultsResources _resources;

        public UIResultsScreenModel Model { get; set; }

        public UIResultScreen(ResultsResources resources, UIResultsScreenModel model)
        {
            Model = model;
            _resources = resources;

            Background = new UIBackground() { 
                BackgroundColor = resources.BackgroundColor,
                BackgroundOverlayColor = resources.BackgroundOverlayColor,
                BackgroundOverlay= resources.BackgroundOverlay,
            };

            Title = new UISprite()
            {
                Sprite = resources.TitleResults
            };

            OK = new UIButton()
            {
                Text = model.BackLabel,
                Font = resources.Font,
                Background = resources.ButtonSelected,
            };

            Goblet = new UIGoblet(resources.Font, resources.Goblet, resources.Banner, resources.BannerOverlay, model.BannerText, model.BannerColor);


            Lines = model.Lines.Select(x => {
                return new UIResultLine(x, _resources.Font, _resources.ResultLine, _resources.ResultLineOutline);
            }).ToArray();

            AddComponent(Background);
            foreach (var line in Lines) AddComponent(line);
            AddComponent(Goblet);
            AddComponent(Title);
            AddComponent(OK);

            FadeIn();
        }
        public override void UpdateLayout(AABB bounds)
        {
            base.UpdateLayout(bounds);

            // Manual layouting
            Background.UpdateLayout(bounds);
            Title.UpdateLayout(AABB.Create(bounds.TopLeft + new Vector2(105, 55), Vector2.Zero));

            Goblet.UpdateLayout(AABB.Create(bounds.RightCenter + new Vector2(-80, 16), Vector2.Zero));

            OK.UpdateLayout(AABB.Create(bounds.BottomRight - new Vector2(64 + 6, 20 + 6), new Vector2(64, 20)));

            // Layout the lines :)
            float width = 164;
            float height = 19;
            float spacing = 1;

            Vector2 totalSize = new Vector2(width, Lines.Length * height + (Lines.Length - 1) * spacing);
            Vector2 offset = bounds.LeftCenter + new Vector2(16, -4);

            foreach (var line in Lines)
            {
                line.UpdateLayout(AABB.Create(offset, new Vector2(width, height)));

                offset += new Vector2(0, height + spacing);
            }

        }

        public void FadeIn()
        {
            var titleAnimation = new UIEaseAnimation(0.5f, 0.5f);
            titleAnimation.PositionAnimation.Animate(new Vector2(0, -80), Vector2.One);
            Title.SetAnimation(titleAnimation);

            var gobletAnimation = new UIEaseAnimation(0.7f, 1.5f + Lines.Length * 0.5f);
            gobletAnimation.PositionAnimation.Animate(new Vector2(0, -16), Vector2.Zero);
            gobletAnimation.ScaleAnimation.Animate(new Vector2(2f, 2f), Vector2.One);
            gobletAnimation.AlphaAnimation.Animate(0, 1);
            Goblet.SetAnimation(gobletAnimation);

            var okAnimation = new UIEaseAnimation(0.6f, 2.5f + Lines.Length * 0.5f);
            okAnimation.VisibleAnimation.Animate(false, true);
            okAnimation.PositionAnimation.Animate(new Vector2(80, 0), Vector2.Zero);
            OK.SetAnimation(okAnimation);

            for (int i = 0; i < Lines.Length; i++)
            {
                var lineAnimation = new UIEaseAnimation(0.5f, 1.0f + (Lines.Length - i) * 0.5f);
                lineAnimation.PositionAnimation.Animate(new Vector2(0, 16), Vector2.Zero);
                lineAnimation.AlphaAnimation.Animate(0, 1);

                Lines[i].SetAnimation(lineAnimation);
            }
        }

        public void FadeOut()
        {
            OK.Background = _resources.ButtonPressed;

            var okAnimation = new UIEaseAnimation(0.5f, 0);
            okAnimation.PositionAnimation.Animate(Vector2.Zero, new Vector2(80, 0));
            okAnimation.ScaleAnimation.Animate(new Vector2(1.2f, 1.2f), Vector2.One);
            OK.SetAnimation(okAnimation);
        }

    }
}
