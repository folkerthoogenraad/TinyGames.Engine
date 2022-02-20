using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.MiniGames.Ice.CharacterActions
{
    public class WalkAction : CharacterAction<Vector2>
    {
        public WalkAction(CharacterGameObject character) : base(character)
        {
        }

        public override void Update(float delta, Vector2 status)
        {
            Character.Physics = Character.Physics.Move(delta, status * Character.Settings.MoveSpeed, Character.Settings.Acceleration);
        }
    }
}
