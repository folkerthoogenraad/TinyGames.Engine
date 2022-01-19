using PinguinGame.Input;
using PinguinGame.MiniGames.Generic;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.MiniGames.Minecarts
{
    public class MinecartInputService : MiniGameInputService<MinecartInput>
    {
        public MinecartInputService(InputService input) : base(input) { }

        public override MinecartInput Convert(InputState input)
        {
            return new MinecartInput()
            {
                JumpLeft = input.MenuLeftPressed,
                JumpRight = input.MenuRightPressed,

                Boost = input.Action,
                Brake = input.Back,
            };
        }
    }
}
