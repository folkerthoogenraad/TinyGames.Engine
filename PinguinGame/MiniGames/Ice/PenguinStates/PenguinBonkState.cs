using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace PinguinGame.MiniGames.Ice.PenguinStates
{
    internal class PenguinBonkState : PenguinState
    {
        public Vector2 Velocity { get; set; }
        private float BonkTimer = 0;

        public PenguinBonkState(Vector2 velocity)
        {
            Velocity = velocity;
        }

        public override void Init(Penguin penguin)
        {
            base.Init(penguin);

            penguin.Bounce.Height = 4;
            penguin.Physics = penguin.Physics.StartBonk(Velocity);
        }

        public override PenguinState Update(Penguin penguin, PenguinInput input, float delta)
        {
            BonkTimer += delta;

            if(BonkTimer > 0.3f)
            {
                return new PenguinWalkState();
            }

            penguin.Physics = penguin.Physics.Slide(delta);

            return this;
        }

        public override void Draw(Graphics2D graphics, Penguin penguin, PenguinGraphics penguinGraphics)
        {
            var facing = PenguinGraphics.GetFacingFromVector(penguin.Facing);
            var color = penguin.Player.Color;

            penguinGraphics.DrawIdle(graphics, facing, penguin.DrawPosition, BonkTimer);
            penguinGraphics.DrawIdleOverlay(graphics, facing, penguin.DrawPosition, BonkTimer, color);
        }
    }
}
