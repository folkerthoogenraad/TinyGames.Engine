using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.MiniGames.Ice;
using PinguinGame.Screens.UI;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace PinguinGame.MiniGames.Ice.GameStates
{
    public abstract class GameState
    {
        public IceGame World { get; private set; }
        public ContentManager Content { get; private set; }
        public GraphicsDevice GraphicsDevice { get; private set; }

        public virtual void Init(IceGame world, GraphicsDevice device, ContentManager content)
        {
            World = world;
            Content = content;
            GraphicsDevice = device;
        }

        public abstract GameState Update(float delta);

        public virtual void Draw(Graphics2D graphics)
        {
            World.DrawWorld(graphics);
        }

        public virtual void Destroy()
        {

        }
    }
}
