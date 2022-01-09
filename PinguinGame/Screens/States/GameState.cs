using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Pinguins;
using PinguinGame.Screens.UI;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace PinguinGame.Screens.States
{
    public abstract class GameState
    {
        public PenguinWorld World { get; private set; }
        public ContentManager Content { get; private set; }
        public GraphicsDevice GraphicsDevice { get; private set; }
        public GameUISkin Skin { get; private set; }

        public virtual void Init(PenguinWorld world, GraphicsDevice device, ContentManager content, GameUISkin skin)
        {
            World = world;
            Content = content;
            GraphicsDevice = device;
            Skin = skin;
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
