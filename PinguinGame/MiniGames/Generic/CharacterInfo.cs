﻿using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace PinguinGame.MiniGames.Generic
{
    public class CharacterInfo
    {
        public string Name { get; set; }
        public string Identifier { get; set; }
        public Sprite Icon { get; set; }
        public CharacterGraphics Graphics { get; set; }
    }
}