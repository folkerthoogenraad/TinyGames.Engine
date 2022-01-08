using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace PinguinGame.Pinguins
{
    internal abstract class PenguinState
    {
        public virtual void Init(Penguin penguin) { }
        public virtual void Destroy() { }
        public abstract PenguinState Update(Penguin penguin, PenguinInput input, float delta);

        public virtual void Draw(Graphics2D graphics, Penguin penguin, PenguinGraphics penguinGraphics)
        {

        }

        protected Color GetColorFromIndex(int index)
        {
            if (index == 0) return Color.Red;
            if (index == 1) return Color.Blue;
            if (index == 2) return Color.Yellow;
            if (index == 3) return Color.Green;

            return Color.Black;
        }
    }
}
