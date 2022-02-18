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
    public class UICountdownScreen : UIComponent
    {
        private InGameResources _resources;

        private UISprite Countdown;
        private int CurrentSecond = 3;

        public UICountdownScreen(InGameResources resources)
        {
            _resources = resources;

            Countdown = new UISprite()
            {
                Sprite = GetSpriteForSecond(3),
            };

            AddComponent(Countdown);
            Animate();
        }
        public void SetCurrentSecond(int second)
        {
            if (second == CurrentSecond) return;

            CurrentSecond = second;

            Countdown.Sprite = GetSpriteForSecond(CurrentSecond);
            Animate();
        }

        private void Animate()
        {
            var animation = new UIEaseAnimation();

            animation.ScaleAnimation.Animation.EaseFunction = Ease.Overshoot;
            animation.ScaleAnimation.Animation.Duration = 0.2f;
            animation.ScaleAnimation.Animation.PreDelay = 0;
            animation.ScaleAnimation.Animate(new Vector2(2f, 2f), new Vector2(1, 1));

            Countdown.SetAnimation(animation);
        }

        public override void UpdateLayout(AABB bounds)
        {
            base.UpdateLayout(bounds);

            // Manual layouting
            Countdown.UpdateLayout(AABB.Create(bounds.Center, Vector2.Zero));
        }

        private Sprite GetSpriteForSecond(int second)
        {
            if (second == 0) return _resources.Go;
            if(second == 1) return _resources.One;
            if(second == 2) return _resources.Two;
            if(second == 3) return _resources.Three;

            return _resources.Three;
        }

    }
}
