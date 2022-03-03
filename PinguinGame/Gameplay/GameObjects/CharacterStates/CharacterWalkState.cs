using Microsoft.Xna.Framework;
using PinguinGame.Gameplay.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;

namespace PinguinGame.Gameplay.GameObjects.CharacterStates
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

        public override void Init(CharacterGameObject character)
        {
            base.Init(character);
        }
        public override void Destroy()
        {
            base.Destroy();
            Walking = false;
        }

        public override CharacterState Update(CharacterGameObject character, CharacterInput input, float delta)
        {
            AnimationTimer += delta;

            Character.Physics = Character.Physics.Move(delta, input.MoveDirection * Character.Settings.MoveSpeed, Character.Settings.Acceleration);

            Walking = input.MoveDirection.LengthSquared() > 0;

            if (input.ActionPressed)
            {
                return new CharacterSlideState(Character.Facing);
            }

            if (input.ActionSecondaryPressed)
            {
                if (character.Inventory.HasSnowball && input.MoveDirection.LengthSquared() > 0)
                {
                    var direction = input.MoveDirection.Normalized();
                    var snowball = character.Inventory.CreateSnowball(character, direction);

                    character.Scene.AddGameObject(snowball);
                    character.Sound.PlaySnowballThrow();
                    character.Physics = character.Physics.StartBonk(-direction * 48);
                }
                else if(!character.Inventory.HasSnowball)
                {
                    return new CharacterGatherSnowState();
                }
            }

            return this;
        }

        public override void Draw(Graphics2D graphics, CharacterGameObject penguin, CharacterGraphics penguinGraphics)
        {
            var facing = CharacterGraphics.GetFacingFromVector(penguin.Facing);

            if (Walking)
            {
                penguinGraphics.DrawWalk(graphics, facing, penguin.Position, penguin.Height, AnimationTimer);
            }
            else
            {
                penguinGraphics.DrawIdle(graphics, facing, penguin.Position, penguin.Height, AnimationTimer);
            }
        }

    }
}
