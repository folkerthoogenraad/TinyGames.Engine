using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Animations;
using TinyGames.Engine.Maths;

namespace TinyGames.Engine.UI.Animations
{
    public class UIEaseAnimation : UIAnimation
    {
        public AnimatedValue<Vector2> ScaleAnimation { get; set; }
        public AnimatedValue<Vector2> PositionAnimation { get; set; }
        public AnimatedValue<float> AlphaAnimation { get; set; }
        public AnimatedValue<bool> VisibleAnimation { get; set; }

        public override Vector2? Scale => ScaleAnimation.Value;
        public override Vector2? Position => PositionAnimation.Value;
        public override float Alpha => AlphaAnimation.Value;

        public UIEaseAnimation(float duration = 0.5f, float predelay = 0)
        {
            ScaleAnimation = new AnimatedValue<Vector2>(new Vector2(1, 1), Vector2.Lerp, new EaseAnimation(Ease.EaseInOut, duration, predelay));
            PositionAnimation = new AnimatedValue<Vector2>(new Vector2(1, 1), Vector2.Lerp, new EaseAnimation(Ease.EaseInOut, duration, predelay));
            AlphaAnimation = new AnimatedValue<float>(1, Tools.Lerp, new EaseAnimation(Ease.EaseInOut, duration, predelay));
            VisibleAnimation = new AnimatedValue<bool>(true, Tools.Lerp, new EaseAnimation(Ease.EaseInOut, duration, predelay));
        }

        public override void Update(float delta)
        {
            PositionAnimation.Update(delta);
            ScaleAnimation.Update(delta);
            AlphaAnimation.Update(delta);
            VisibleAnimation.Update(delta);
        }
    }
}
