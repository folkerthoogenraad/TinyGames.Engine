using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Gameplay.GameObjects.IceBlockStates
{
    public class IceBlockRaisingState : IceBlockState
    {
        public override bool Highlighted => (_timer % 0.5f) < 0.25f && Solid;
        public override bool Solid => Height > 0;
        public override float Height => -8 + RaiseDistance;
        public float RaiseDistance => _timer * 4;

        private float _timer = 0;

        public IceBlockRaisingState(IceBlockGameObject block) : base(block) { }

        public override IceBlockState Update(float delta)
        {
            _timer += delta;

            if(RaiseDistance >= 16)
            {
                return new IceBlockIdleState(Block);
            }

            return this;
        }
    }
}
