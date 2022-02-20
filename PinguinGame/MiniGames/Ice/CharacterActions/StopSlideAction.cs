using Microsoft.Xna.Framework;
using PinguinGame.MiniGames.Ice.CharacterStates;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.MiniGames.Ice.CharacterActions
{
    public class StopSlideAction : CharacterAction<bool>
    {
        public StopSlideAction(CharacterGameObject character) : base(character)
        {
        }

        public override void Update(float delta, bool sliding)
        {
            if (!sliding)
            {
                Character.State = new CharacterWalkState();
            }
        }
    }
}
