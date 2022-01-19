using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace PinguinGame.MiniGames.Ice.PenguinStates
{
    internal class PenguinWalkState : PenguinState
    {
        private float AnimationTimer = 0;
        private bool Walking = false;

        public override PenguinState Update(Penguin penguin, PenguinInput input, float delta)
        {
            AnimationTimer += delta;
            Walking = input.MoveDirection.LengthSquared() > 0;

            penguin.Physics = penguin.Physics.Move(delta, input.MoveDirection * penguin.Settings.MoveSpeed, penguin.Settings.Acceleration);

            if (input.SlideStart)
            {
                Vector2 direction = penguin.Physics.Velocity / penguin.Settings.MoveSpeed;

                direction += input.MoveDirection;

                if (direction.LengthSquared() <= 0) direction = input.MoveDirection;
                if (direction.LengthSquared() <= 0) direction = penguin.Facing;
                if (direction.LengthSquared() <= 0) direction = new Vector2(1, 0);

                return new PenguinSlideState(direction);
            }

            return this;
        }

        public override void Draw(Graphics2D graphics, Penguin penguin, CharacterGraphics penguinGraphics)
        {
            var facing = CharacterGraphics.GetFacingFromVector(penguin.Facing);
            var color = penguin.Player.Color;

            if (Walking)
            {
                penguinGraphics.DrawWalk(graphics, facing, penguin.DrawPosition, AnimationTimer);
                penguinGraphics.DrawWalkOverlay(graphics, facing, penguin.DrawPosition, AnimationTimer, color);
            }
            else
            {
                penguinGraphics.DrawIdle(graphics, facing, penguin.DrawPosition, AnimationTimer);
                penguinGraphics.DrawIdleOverlay(graphics, facing, penguin.DrawPosition, AnimationTimer, color);
            }
        }
    }
}
