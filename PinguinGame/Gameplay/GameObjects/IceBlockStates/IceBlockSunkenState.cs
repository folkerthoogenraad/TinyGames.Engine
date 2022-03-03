using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Gameplay.GameObjects.IceBlockStates
{
    public class IceBlockSunkenState : IceBlockState
    {
        public override bool Highlighted => false;
        public override bool Solid => false;
        public override float Height => -8;

        public IceBlockSunkenState(IceBlockGameObject block) : base(block) { }

        public override IceBlockState Update(float delta)
        {
            return this;
        }
    }
}
