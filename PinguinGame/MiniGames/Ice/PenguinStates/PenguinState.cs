using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace PinguinGame.MiniGames.Ice.PenguinStates
{
    internal abstract class PenguinState
    {
        public virtual void Init(Penguin penguin) { }
        public virtual void Destroy() { }
        public abstract PenguinState Update(Penguin penguin, PenguinInput input, float delta);

        public virtual void Draw(Graphics2D graphics, Penguin penguin, PenguinGraphics penguinGraphics)
        {

        }

    }
}
