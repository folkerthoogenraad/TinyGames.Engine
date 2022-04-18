using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Razura.GameObjects;
using Razura.Items;
using Razura.Tiles;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace Razura
{
    public class RazuraGame : Game
    {
        private GraphicsDeviceManager _graphics;

        public Graphics2D Graphics;
        public Camera Camera;

        public RazuraGame()
        {
            _graphics = new GraphicsDeviceManager(this) {  };

            Content.RootDirectory = "Content"; 

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;

            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            var effect = new AlphaTestEffect(GraphicsDevice)
            {
                VertexColorEnabled = true
            };

            Graphics = new Graphics2D(GraphicsDevice, effect);
            Camera = new Camera(720, 16f / 9f);

            ItemRegistry.Init();
            TileRegistry.Init();
            GameObjectRegistry.Init();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Graphics.Clear(Color.White);

            Graphics.Begin(Camera.GetMatrix());


            Graphics.End();

            base.Draw(gameTime);
        }
    }
}
