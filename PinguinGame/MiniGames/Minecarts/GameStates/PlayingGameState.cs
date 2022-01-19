using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.MiniGames.Ice;
using PinguinGame.MiniGames.Ice.GameStates;
using PinguinGame.Player;
using PinguinGame.Screens.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PinguinGame.MiniGames.Minecarts.GameStates
{
    public class PlayingGameState : GameState
    {
        public override void Init(MinecartGame game, GraphicsDevice device, ContentManager content)
        {
            base.Init(game, device, content);
        }

        public override GameState Update(float delta)
        {
            Game.UpdateMinecartsWithInput(delta);
            Game.UpdateCamera(delta);
            Game.UpdateMinecartOffRoad();

            return this;
        }
    }
}
