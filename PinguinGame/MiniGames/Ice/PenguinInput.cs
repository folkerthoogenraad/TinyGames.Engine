using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.MiniGames.Ice
{
    public class PenguinInput
    {
        public Vector2 MoveDirection { get; set; }
        public bool SlideStart { get; set; }
        public bool SlideHold { get; set; }

        public static PenguinInput Empty => new PenguinInput();
    }
}
