using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Animations;

namespace TinyGames.Engine.UI.Animations
{
    // TODO Update naming
    public class UIUpDownAnimation : UIAnimation
    {
        public Vector2 CurrentScale { get; set; }
        public Vector2 TargetScale { get; set; } = new Vector2(1, 1);
        public float Speed { get; set; } = 8;

        public Vector2 HighScale { get; set; }
        public float Frequency { get; set; } = 0;
        public float Phase { get; set; } = 0;
        public override Vector2? Scale => CurrentScale;

        public UIUpDownAnimation(float frequency, Vector2 scale)
        {
            HighScale = scale;
            CurrentScale = scale;
            Frequency = frequency;
        }

        public override void Update(float delta)
        {
            Phase += delta * Frequency;

            if(Phase > 1)
            {
                Phase -= 1;
                CurrentScale = HighScale;
            }

            CurrentScale = Vector2.Lerp(CurrentScale, TargetScale, delta * Speed);
        }
    }

}
