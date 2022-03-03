using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Gameplay.GameObjects.IceBlockStates
{
    public class IceBlockIdleState : IceBlockState
    {
        public override bool Highlighted => false;
        public override bool Solid => true;
        public override float Height => 8;

        public IceBlockIdleState(IceBlockGameObject block) : base(block) { }

        public override IceBlockState Update(float delta)
        {
            return this;
        }
    }
}
