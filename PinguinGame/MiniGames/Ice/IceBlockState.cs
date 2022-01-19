using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.MiniGames.Ice
{
    public abstract class IceBlockState
    {
        public IceBlock Block { get; }
        public virtual bool Highlighted => false;
        public virtual bool Solid => true;
        public virtual float Height => 8;

        public IceBlockState(IceBlock block)
        {
            Block = block;
        }

        public virtual IceBlockState Update(float delta)
        {
            return this;
        }
    }
}
