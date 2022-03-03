using PinguinGame.Input;
using PinguinGame.Player;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Gameplay
{
    public abstract class MiniGameInputService<T> : IMiniGameInputService<T>
    {
        private IInputService _inputService;

        public MiniGameInputService(IInputService inputService)
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
