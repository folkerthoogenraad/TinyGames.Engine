using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace PinguinGame.MiniGames.Generic
{
    public class LevelInfo
    {
        public string Name { get; set; }
        public string Identifier { get; set; }
        public string Description { get; set; }
        public string File { get; set; }
        public Sprite Icon { get; set; }
    }
}
