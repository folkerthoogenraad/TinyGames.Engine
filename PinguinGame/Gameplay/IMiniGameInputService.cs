using PinguinGame.Input;
using PinguinGame.Player;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Gameplay
{
    public interface IMiniGameInputService<T>
    {
        public T GetInputForPlayer(PlayerInfo player);
        public T GetInputForInputDevice(InputDeviceType device);
    }
}
