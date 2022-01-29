using Microsoft.Xna.Framework;
using PinguinGame.MiniGames.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;

namespace PinguinGame.MiniGames.Ice.CharacterStates
{
    internal class CharacterSlideState : CharacterState
    {
        public Vector2 Direction { get; set; }

        private float SlideTimer = 0;

        public CharacterSlideState(Vector2 speed)
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

        public override void Init(Character penguin)
        {
            base.Init(penguin);

            penguin.Physics = penguin.Physics.StartSlide(Direction * penguin.Settings.SlideSpeed);
            penguin.Bounce.Height = 4;
        }

        public override CharacterState Update(Character penguin, CharacterInput input, float delta)
        {
            SlideTimer += delta;

            if(CanStopSlide && !input.SlideHold)
            {
                penguin.Bounce.Height = 3;
                return new CharacterWalkState();
            }

            penguin.Physics = penguin.Physics.Slide(delta);

            return this;
        }

        public override void Draw(Graphics2D graphics, Character penguin, CharacterGraphics penguinGraphics)
        {
            var facing = CharacterGraphics.GetFacingFromVector(penguin.Facing);
            var color = penguin.Player.Color;

            penguinGraphics.DrawSlide(graphics, facing, penguin.DrawPosition, SlideTimer);
        }

        public bool CanStopSlide => SlideTimer > 0.3f;
    }
}
