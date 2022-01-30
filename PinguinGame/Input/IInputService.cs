using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PinguinGame.Input
{
    public interface IInputService
    {
        public void Poll();
        public IEnumerable<InputState> InputStates { get; }
        public InputState GetInputStateForDevice(InputDeviceType device);
    }
}
