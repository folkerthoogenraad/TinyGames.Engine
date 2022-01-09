﻿using Microsoft.Xna.Framework;
using PinguinGame.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Player
{
    public class PlayerInfo
    {
        public int Index = 0;

        public bool Joined { get; set; } = false;
        public InputDevice InputDevice { get; set; }

        public Color Color => GetColorFromIndex(Index);

        protected static Color GetColorFromIndex(int index)
        {
            if (index == 0) return Color.Red;
            if (index == 1) return Color.Blue;
            if (index == 2) return Color.Yellow;
            if (index == 3) return Color.Green;

            return Color.Black;
        }
    }
}
