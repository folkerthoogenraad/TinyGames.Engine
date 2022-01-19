using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.MiniGames.Ice
{
    public class IceBlockIdleState : IceBlockState
    {
        public override bool Highlighted => false;
        public override bool Solid => true;
        public override float Height => 8;

        public IceBlockIdleState(IceBlock block) : base(block) { }

        public override IceBlockState Update(float delta)
        {
            return this;
        }
    }
}
