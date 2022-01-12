using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Animations;

namespace TinyGames.Engine.UI.Animations
{
    public class UIBounceAnimation : UIAnimation
    {
        private BounceAnimation Bounce { get; set; }

        public float ScalePerHeight { get; set; } = 0.5f;
        public override Vector2? Scale => new Vector2(Bounce.Height * ScalePerHeight + 1, Bounce.Height * ScalePerHeight + 1);

        public UIBounceAnimation(float gravity = 50)
        {
            Bounce = new BounceAnimation();
            Bounce.Height = 1;
            Bounce.Gravity = gravity;
        }

        public override void Update(float delta)
        {
            Bounce.Update(delta);
        }
    }

}
