using Microsoft.Xna.Framework;
using PinguinGame.MiniGames.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;

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

                if (value) Character?.Sound.PlayStartWalking();
                if (!value) Character?.Sound.PlayStopWalking();
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
                if (character.SnowballGathering.HasSnowball && input.MoveDirection.LengthSquared() > 0)
                {
                    var direction = input.MoveDirection.Normalized();
                    var snowball = character.SnowballGathering.CreateSnowball(character, direction);

                    character.Level.AddSnowball(snowball);
                    character.Sound.PlaySnowballThrow();
                    character.Physics = character.Physics.StartBonk(-direction * 48);
                }
                else if(!character.SnowballGathering.HasSnowball)
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
                penguinGraphics.DrawWalk(graphics, facing, penguin.Position, penguin.Height, AnimationTimer);
            }
            else
            {
                penguinGraphics.DrawIdle(graphics, facing, penguin.Position, penguin.Height, AnimationTimer);
            }
        }

        public override void Destroy()
        {
            base.Destroy();
            Walking = false;
        }
    }
}
