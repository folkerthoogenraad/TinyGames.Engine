using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Animations;

namespace TinyGames.Engine.UI.Animations
{
    public class UITransformAnimation : UIAnimation
    {
        public Vector2 TargetScale { get; set; } = new Vector2(1, 1);
        public Vector2 TargetPosition { get; set; } = new Vector2(0, 0);
        public Vector2 CurrentScale { get; set; }
        public Vector2 CurrentPosition { get; set; }
        public float Speed { get; set; } = 8;

        public override Vector2? Position => CurrentPosition;
        public override Vector2? Scale => CurrentScale;

        public UITransformAnimation(Vector2 position, Vector2 scale)
        {
            CurrentPosition = position;
            CurrentScale = scale;
        }

        public override void Update(float delta)
        {
            CurrentScale = Vector2.Lerp(CurrentScale, TargetScale, delta * Speed);
            CurrentPosition = Vector2.Lerp(CurrentPosition, TargetPosition, delta * Speed);
        }
    }

}
