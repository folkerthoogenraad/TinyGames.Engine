using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.IO.Levels
{
    public class LevelInfo
    {
        public string Name { get; set; }
        public string Identifier { get; set; }
        public string Description { get; set; }
        public string File { get; set; }
        public IOSprite Icon { get; set; }
    }
}
