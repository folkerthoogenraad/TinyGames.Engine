using Microsoft.Xna.Framework.Content;
using PinguinGame.GameStates;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace PinguinGame.MiniGames.Ice.GameStates
{
    public class IceGameState<T> : GameState<T>
    {
        public IceGame Game { get; set; }
        public ContentManager Content => Game.Content;

        public IceGameState(IceGame game)
        {
            Game = game;
        }

        public override void Draw(Graphics2D graphics)
        {
            base.Draw(graphics);

            Game.DrawWorld(graphics);
        }
    }
}
