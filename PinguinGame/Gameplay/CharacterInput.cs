using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Gameplay
{
    public class CharacterInput
    {
        public Vector2 MoveDirection { get; set; }
        public bool ActionPressed { get; set; }
        public bool Action { get; set; }
        public bool ActionSecondary { get; set; }
        public bool ActionSecondaryPressed { get; set; }
        public static CharacterInput Empty => new CharacterInput();
    }
}
