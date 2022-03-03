using PinguinGame.Input;
using PinguinGame.Gameplay.Generic;
using PinguinGame.Gameplay;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Gameplay
{
    public class IceInput : MiniGameInputService<CharacterInput>
    {
        public IceInput(IInputService input) : base(input) { }

        public override CharacterInput Convert(InputState input)
        {
            return new CharacterInput()
            {
                MoveDirection = input.MoveDirection,
                Action = input.Action,
                ActionPressed = input.ActionPressed,
                ActionSecondary = input.ActionSecondary,
                ActionSecondaryPressed = input.ActionSecondaryPressed,
            };
        }
    }
}
