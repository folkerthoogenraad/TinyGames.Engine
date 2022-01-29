using PinguinGame.Input;
using PinguinGame.MiniGames.Generic;
using PinguinGame.MiniGames.Ice;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.MiniGames.Ice
{
    public class IceInput : MiniGameInputService<CharacterInput>
    {
        public IceInput(InputService input) : base(input) { }

        public override CharacterInput Convert(InputState input)
        {
            return new CharacterInput()
            {
                MoveDirection = input.MoveDirection,
                SlideStart = input.ActionPressed,
                SlideHold = input.Action,
                GatherSnow = input.Back,
                ThrowSnowball = input.BackPressed,
            };
        }
    }
}
