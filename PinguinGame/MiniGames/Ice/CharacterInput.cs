using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.MiniGames.Ice
{
    public class CharacterInput
    {
        public Vector2 MoveDirection { get; set; }
        public bool SlideStart { get; set; }
        public bool SlideHold { get; set; }
        public bool GatherSnow { get; set; }
        public bool ThrowSnowball { get; set; }
        public static CharacterInput Empty => new CharacterInput();
    }
}
