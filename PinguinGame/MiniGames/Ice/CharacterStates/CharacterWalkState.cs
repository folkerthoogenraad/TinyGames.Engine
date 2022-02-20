using Microsoft.Xna.Framework;
using PinguinGame.MiniGames.Generic;
using PinguinGame.MiniGames.Ice.CharacterActions;
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

        public override void Init(CharacterGameObject character)
        {
            base.Init(character);


            // This is not a very nice way of doing it, but wahtever
            character.Actions.PushActions();

            character.Actions.CurrentActions = new CharacterActionsSet()
            {
                Move = new WalkAction(character),
                Primary = new StartSlideAction(character),
                Secondary = new NoAction<bool>(character),
            };
        }
        public override void Destroy()
        {
            base.Destroy();
            Walking = false;

            Character.Actions.PopActions();
        }

        public override CharacterState Update(CharacterGameObject character, CharacterInput input, float delta)
        {
            AnimationTimer += delta;
            
            Walking = input.MoveDirection.LengthSquared() > 0;

            if (input.ActionSecondaryPressed)
            {
                if (character.SnowballGathering.HasSnowball && input.MoveDirection.LengthSquared() > 0)
                {
                    var direction = input.MoveDirection.Normalized();
                    var snowball = character.SnowballGathering.CreateSnowball(character, direction);

                    character.Scene.AddGameObject(snowball);
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
