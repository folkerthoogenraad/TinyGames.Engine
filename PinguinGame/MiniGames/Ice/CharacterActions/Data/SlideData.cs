using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.MiniGames.Ice.CharacterActions.Data
{
    public class SlideData
    {
        public Vector2 InitialDirection { get; set; }
        public float SlideTimer { get; set; }
        public bool CanStopSliding => SlideTimer > 0.3f;
    }
}
