using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.MiniGames.Ice;
using PinguinGame.Screens.UI;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace PinguinGame.MiniGames.Minecarts.GameStates
{
    public abstract class GameState
    {
        public MinecartGame Game { get; private set; }
        public ContentManager Content { get; private set; }
        public GraphicsDevice GraphicsDevice { get; private set; }

        public virtual void Init(MinecartGame game, GraphicsDevice device, ContentManager content)
        {
            Game = game;
            Content = content;
            GraphicsDevice = device;
        }

        public abstract GameState Update(float delta);

        public virtual void Draw(Graphics2D graphics)
        {
            Game.Draw();
        }

        public virtual void Destroy()
        {

        }
    }
}
