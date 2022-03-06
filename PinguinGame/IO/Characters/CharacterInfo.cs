using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.IO.Characters
{
    public class CharacterInfo
    {
        public string Name { get; set; }
        public string Identifier { get; set; }
        public string Sheet { get; set; }
        public IOSprite Icon { get; set; }
    }
}
