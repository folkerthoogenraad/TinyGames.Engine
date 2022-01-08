using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;

namespace PinguinGame.Pinguins
{
    internal class PenguinSlideState : PenguinState
    {
        public Vector2 Direction { get; set; }

        private float SlideTimer = 0;

        public PenguinSlideState(Vector2 speed)
        {
            if(speed.LengthSquared() > 0)
            {
                Direction = speed;
            }
            else
            {
                Direction = new Vector2(1, 0);
            }

            Direction = Direction.Normalized();
        }

        public override void Init(Penguin penguin)
        {
            base.Init(penguin);

            penguin.Physics = penguin.Physics.StartSlide(Direction * penguin.Settings.SlideSpeed);
            penguin.Bounce.Height = 4;
        }

        public override PenguinState Update(Penguin penguin, PenguinInput input, float delta)
        {
            SlideTimer += delta;

            if(CanStopSlide && !input.SlideHold)
            {
                penguin.Bounce.Height = 3;
                return new PenguinWalkState();
            }

            penguin.Physics = penguin.Physics.Slide(delta);

            return this;
        }

        public override void Draw(Graphics2D graphics, Penguin penguin, PenguinGraphics penguinGraphics)
        {
            var facing = PenguinGraphics.GetFacingFromVector(penguin.Facing);
            var color = GetColorFromIndex(penguin.Player.Index);

            penguinGraphics.DrawSlide(graphics, facing, penguin.DrawPosition, SlideTimer);
            penguinGraphics.DrawSlideOverlay(graphics, facing, penguin.DrawPosition, SlideTimer, color);
        }

        public bool CanStopSlide => SlideTimer > 0.3f;
    }
}
