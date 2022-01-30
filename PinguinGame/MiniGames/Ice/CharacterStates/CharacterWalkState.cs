using Microsoft.Xna.Framework;
using PinguinGame.MiniGames.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace PinguinGame.MiniGames.Ice.CharacterStates
{
    internal class CharacterWalkState : CharacterState
    {
        private float AnimationTimer = 0;


        private bool _walking = false;

        private bool Walking
        {
            get { return _walking; }
            set {
                if (value == _walking) return;

                if (value) Character?.Sound.StartWalking();
                if (!value) Character?.Sound.StopWalking();
                _walking = value; 
            }
        }

        public override CharacterState Update(Character character, CharacterInput input, float delta)
        {
            AnimationTimer += delta;
            

            Walking = input.MoveDirection.LengthSquared() > 0;

            character.Physics = character.Physics.Move(delta, input.MoveDirection * character.Settings.MoveSpeed, character.Settings.Acceleration);

            if (input.ActionPressed)
            {
                Vector2 direction = character.Physics.Velocity / character.Settings.MoveSpeed;

                direction += input.MoveDirection;

                if (direction.LengthSquared() <= 0) direction = input.MoveDirection;
                if (direction.LengthSquared() <= 0) direction = character.Facing;
                if (direction.LengthSquared() <= 0) direction = new Vector2(1, 0);

                return new CharacterSlideState(direction);
            }

            if (input.ActionSecondaryPressed)
            {
                if (character.SnowballGathering.HasSnowball)
                {
                    character.SnowballGathering.RemoveSnowball();

                    Vector2 direction = input.MoveDirection;

                    if(direction.LengthSquared() <= 0) direction = character.Facing;
                    if (direction.LengthSquared() <= 0) direction = new Vector2(1, 0);

                    if(direction.LengthSquared() > 0)
                    {
                        direction.Normalize();
                    }

                    // Throw snowball
                    character.Level.AddSnowball(new Snowball()
                    {
                        Position = character.Position,
                        Velocity = direction * 128,
                        Lifetime = 1,
                        Height = character.GroundHeight + 8,
                        Player = character.Player,
                    });
                    return new CharacterBonkState(-direction * 16, 0.1f);
                }
                else
                {
                    return new CharacterGatherSnowState();
                }
            }

            return this;
        }

        public override void Draw(Graphics2D graphics, Character penguin, CharacterGraphics penguinGraphics)
        {
            var facing = CharacterGraphics.GetFacingFromVector(penguin.Facing);
            var color = penguin.Player.Color;

            if (Walking)
            {
                penguinGraphics.DrawWalk(graphics, facing, penguin.DrawPosition, AnimationTimer);
            }
            else
            {
                penguinGraphics.DrawIdle(graphics, facing, penguin.DrawPosition, AnimationTimer);
            }
        }

        public override void Destroy()
        {
            base.Destroy();
            Walking = false;
        }
    }
}
