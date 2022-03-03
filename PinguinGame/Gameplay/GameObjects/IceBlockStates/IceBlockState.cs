using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Gameplay.GameObjects.IceBlockStates
{
    public abstract class IceBlockState
    {
        public IceBlockGameObject Block { get; }
        public virtual bool Highlighted => false;
        public virtual bool Solid => true;
        public virtual float Height => 8;

        public IceBlockState(IceBlockGameObject block)
        {
            Block = block;
        }

        public virtual IceBlockState Update(float delta)
        {
            return this;
        }
    }
}
