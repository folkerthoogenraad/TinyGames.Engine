using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.MiniGames.Ice
{
    public class IceBlockSinkingState : IceBlockState
    {
        public override bool Highlighted => (_timer % 0.5f) < 0.25f && Solid;
        public override bool Solid => Height > 0;
        public override float Height => 8 - SinkDistance;
        public float SinkDistance => _timer * 4;

        private float _timer = 0;

        public IceBlockSinkingState(IceBlock block) : base(block) { }

        public override IceBlockState Update(float delta)
        {
            _timer += delta;

            if(SinkDistance < -8)
            {
                return new IceBlockSunkenState(Block);
            }

            return this;
        }
    }
}
