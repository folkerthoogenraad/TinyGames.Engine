using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;

namespace PinguinGame.Screens.Fades
{
    public abstract class Fade
    {
        public abstract void Draw(Graphics2D graphics, float amount, AABB bounds);
    }
}
