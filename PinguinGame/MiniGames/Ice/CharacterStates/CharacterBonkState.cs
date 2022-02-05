using Microsoft.Xna.Framework;
using PinguinGame.MiniGames.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace PinguinGame.MiniGames.Ice.CharacterStates
{
    internal class CharacterBonkState : CharacterState
    {
        public Vector2 Velocity { get; set; }
        private float BonkTimer = 0;

        public CharacterBonkState(Vector2 velocity, float timeout = 0.3f)
        {
            Velocity = velocity;
            BonkTimer = timeout;
        }

        public override void Init(Character penguin)
        {
            base.Init(penguin);

            penguin.Bounce.Height = 4;
            penguin.Physics = penguin.Physics.StartBonk(Velocity);
        }

        public override CharacterState Update(Character penguin, CharacterInput input, float delta)
        {
            BonkTimer -= delta;

            if(BonkTimer < 0)
            {
                return new CharacterWalkState();
            }

            penguin.Physics = penguin.Physics.Slide(delta);

            return this;
        }

        public override void Draw(Graphics2D graphics, Character penguin, CharacterGraphics penguinGraphics)
        {
            var facing = CharacterGraphics.GetFacingFromVector(penguin.Facing);
            var color = penguin.Player.Color;

            penguinGraphics.DrawIdle(graphics, facing, penguin.Position, penguin.Height, BonkTimer);
        }
    }
}
