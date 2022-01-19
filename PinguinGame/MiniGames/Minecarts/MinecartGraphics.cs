using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace PinguinGame.MiniGames.Minecarts
{
    public class MinecartGraphics
    {
        public Sprite Background { get; set; }
        public Sprite Foreground { get; set; }
        public Sprite Player { get; set; }
        public Sprite Shadow { get; set; }
        public Animation Dust { get; set; }
        public Animation Sparks { get; set; }
    }
}
