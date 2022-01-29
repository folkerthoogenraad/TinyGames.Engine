using PinguinGame.Input;
using PinguinGame.Player;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.MiniGames.Generic
{
    public abstract class MiniGameInputService<T> : IMiniGameInputService<T>
    {
        private InputService _inputService;
        public MiniGameInputService(InputService inputService)
        {
            _inputService = inputService;
        }

        public T GetInputForPlayer(PlayerInfo player)
        {
            return GetInputForInputDevice(player.InputDevice);
        }

        public T GetInputForInputDevice(InputDeviceType device)
        {
            return Convert(_inputService.GetInputStateForDevice(device));
        }

        public abstract T Convert(InputState input);
    }
}
