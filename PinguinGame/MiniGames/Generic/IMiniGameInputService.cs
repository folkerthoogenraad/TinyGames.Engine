﻿using PinguinGame.Input;
using PinguinGame.Player;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.MiniGames.Generic
{
    public interface IMiniGameInputService<T>
    {
        public T GetInputForPlayer(PlayerInfo player);
        public T GetInputForInputDevice(InputDevice device);
    }
}
