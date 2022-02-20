using Microsoft.Xna.Framework;
using PinguinGame.MiniGames.Ice.CharacterStates;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.MiniGames.Ice.CharacterActions
{
    public class StartSlideAction : CharacterAction<bool>
    {
        public StartSlideAction(CharacterGameObject character) : base(character)
        {
        }

        public override void Update(float delta, bool sliding)
        {
            // Old code
            //if (input.ActionPressed)
            //{
            //    Vector2 direction = character.Physics.Velocity / character.Settings.MoveSpeed;

            //    direction += input.MoveDirection;

            //    if (direction.LengthSquared() <= 0) direction = input.MoveDirection;
            //    if (direction.LengthSquared() <= 0) direction = character.Facing;
            //    if (direction.LengthSquared() <= 0) direction = new Vector2(1, 0);

            //    return new CharacterSlideState(direction);
            //}


            if (sliding)
            {
                Character.State = new CharacterSlideState(Character.Facing);
            }
        }
    }
}
