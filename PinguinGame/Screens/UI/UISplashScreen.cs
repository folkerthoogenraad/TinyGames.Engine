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

    public class UISplashScreen : UIComponent
    {
        public UIBackground Background { get; set; }
        public UISprite Title { get; set; }

        private SplashResources _resources;

        public UISplashScreen(SplashResources resources)
        {
            _resources = resources;

            Background = new UIBackground() { 
                BackgroundColor = resources.BackgroundColor,
            };

            Title = new UISprite()
            {
                Sprite = resources.JustFGames
            };

            AddComponent(Background);
            AddComponent(Title);
        }

        public void FadeIn()
        {
            var animation = new UIEaseAnimation(1);
            animation.AlphaAnimation.Animate(0, 1);
            animation.ScaleAnimation.Animate(new Vector2(1.5f, 1.5f), new Vector2(1, 1));

            Title.SetAnimation(animation);
        }

        public void FadeOut()
        {
            var animation = new UIEaseAnimation(1);
            //animation.AlphaAnimation.Animate(1, 0);
            animation.ScaleAnimation.Animate(new Vector2(1, 1), new Vector2(0, 0));

            Title.SetAnimation(animation);
        }

        public override void UpdateLayout(AABB bounds)
        {
            base.UpdateLayout(bounds);

            // Manual layouting
            Background.UpdateLayout(bounds);
            Title.UpdateLayout(AABB.Create(bounds.Center, Vector2.Zero));
        }

    }
}
