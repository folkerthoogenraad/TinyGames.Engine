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
    }
}
