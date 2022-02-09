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
    public class UIIntermediateResultsScreenModel
    {
        public string NextLabel { get; set; }
        public UIResultLineModel[] Lines;
    }

    public class UIIntermediateResultsScreen : UIComponent
    {
        public UIBackgroundColor Background { get; set; }
        public UISprite Title { get; set; }
        public UIResultLine[] Lines { get; set; }

        public UIButton OK { get; set; }

        public UIIntermediateResultsScreenModel Model { get; set; }

        private IntermediateResultsResources _resources;

        public UIIntermediateResultsScreen(IntermediateResultsResources resources, UIIntermediateResultsScreenModel model)
        {
            Model = model;
            _resources = resources;

            Background = new UIBackgroundColor() {
                BackgroundColor = resources.BackgroundColor
            };

            Title = new UISprite()
            {
                Sprite = resources.TitleResults
            };

            OK = new UIButton()
            {
                Text = model.NextLabel,
                Font = resources.Font,
                Background = resources.ButtonSelected,
            };

            Lines = model.Lines.Select(x => {
                return new UIResultLine(x, _resources.Font, _resources.ResultLine, _resources.ResultLineOutline);
            }).ToArray();

            AddComponent(Background);
            AddComponent(Title);

            foreach (var line in Lines) AddComponent(line);

            AddComponent(OK);

            FadeIn();
        }
        public override void UpdateLayout(AABB bounds)
        {
            base.UpdateLayout(bounds);

            // Manual layouting
            Background.UpdateLayout(bounds);
            Title.UpdateLayout(AABB.Create(bounds.TopCenter + new Vector2(0, 32), Vector2.Zero));

            OK.UpdateLayout(AABB.Create(bounds.BottomRight - new Vector2(64 + 6, 20 + 6), new Vector2(64, 20)));

            // Layout the lines :)
            float width = 164;
            float height = 19;
            float spacing = 1;

            Vector2 totalSize = new Vector2(width, Lines.Length * height + (Lines.Length - 1) * spacing);
            Vector2 offset = bounds.Center + new Vector2(0, 16) - totalSize / 2;

            foreach(var line in Lines)
            {
                line.UpdateLayout(AABB.Create(offset, new Vector2(width, height)));

                offset += new Vector2(0, height + spacing);
            }
        }

        public void FadeIn()
        {
            var titleAnimation = new UIEaseAnimation(0.3f, 0.2f);
            titleAnimation.ScaleAnimation.Animate(new Vector2(1.2f, 1.2f), Vector2.One);
            titleAnimation.PositionAnimation.Animate(new Vector2(0, -64), Vector2.One);
            Title.SetAnimation(titleAnimation);

            var backgroundAnimation = new UIEaseAnimation(0.5f, 0);
            backgroundAnimation.AlphaAnimation.Animate(0, 1);
            Background.SetAnimation(backgroundAnimation);

            var okAnimation = new UIEaseAnimation(0.3f, 1.0f);
            okAnimation.VisibleAnimation.Animate(false, true);
            okAnimation.PositionAnimation.Animate(new Vector2(80, 0), Vector2.Zero);
            OK.SetAnimation(okAnimation);

            for(int i = 0; i < Lines.Length; i++)
            {
                var lineAnimation = new UIEaseAnimation(0.3f, 0.5f + i * 0.1f);
                lineAnimation.PositionAnimation.Animate(new Vector2(0, 16), Vector2.Zero);
                lineAnimation.AlphaAnimation.Animate(0, 1);

                Lines[i].SetAnimation(lineAnimation);
            }
        }

        public void FadeOut()
        {
            OK.Background = _resources.ButtonPressed;

            var titleAnimation = new UIEaseAnimation(0.3f, 0);
            titleAnimation.PositionAnimation.Animate(Vector2.Zero, new Vector2(0, -64));
            Title.SetAnimation(titleAnimation);

            var backgroundAnimation = new UIEaseAnimation(0.5f, 0);
            backgroundAnimation.AlphaAnimation.Animate(1, 0);
            Background.SetAnimation(backgroundAnimation);

            var okAnimation = new UIEaseAnimation(0.5f, 0);
            okAnimation.PositionAnimation.Animate(Vector2.Zero, new Vector2(80, 0));
            okAnimation.ScaleAnimation.Animate(new Vector2(1.2f, 1.2f), Vector2.One);
            OK.SetAnimation(okAnimation);


            for (int i = 0; i < Lines.Length; i++)
            {
                var lineAnimation = new UIEaseAnimation(0.3f, (Lines.Length - i) * 0.1f);
                lineAnimation.PositionAnimation.Animate(Vector2.Zero, new Vector2(0, 16));
                lineAnimation.AlphaAnimation.Animate(1, 0);

                Lines[i].SetAnimation(lineAnimation);
            }
        }

    }
}
