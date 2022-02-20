using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.MiniGames.Ice.CharacterActions
{
    public class NoAction<T> : CharacterAction<T>
    {
        public NoAction(CharacterGameObject character) : base(character)
        {
        }

        public override void Update(float delta, T status)
        {
        }
    }
}
