using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Screens.Resources;
using PinguinGame.Screens.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using TinyGames.Engine.Graphics;

namespace PinguinGame.MiniGames.Ice.GameStates
{
    public class IntroductionGameState : IceGameState<int>
    {
        public IntroductionGameState(IceGame game) : base(game)
        {
        }

        public override void Update(float delta)
        {
            Complete(0);
        }

        public override void Draw(Graphics2D graphics)
        {
            base.Draw(graphics);
        }
    }
}
