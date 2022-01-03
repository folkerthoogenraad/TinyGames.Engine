using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace Cryptica
{
    public class CrypticaGame : Game
    {
        private GraphicsDeviceManager _graphics;

        public Graphics2D Graphics;
        public Camera Camera;

        public CrypticaGame()
        {
            _graphics = new GraphicsDeviceManager(this) 
            { 
                GraphicsProfile = GraphicsProfile.HiDef,
                PreferMultiSampling = true,
            };


            Content.RootDirectory = "Content"; 

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            GraphicsDevice.PresentationParameters.MultiSampleCount = 16;

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;

            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            //var effect = Content.Load<Effect>("Effects/StandardEffect");

            var effect = new BasicEffect(GraphicsDevice)
            {
                VertexColorEnabled = true
            };

            Graphics = new Graphics2D(GraphicsDevice, effect);
            Camera = new Camera(720, 16f / 9f);
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

            Graphics.DrawCircle(Vector2.Zero, 64, Color.Blue, 2);

            Graphics.DrawLine(new Vector2(-128, -128), new Vector2(256, 0), 50, 1, Color.Red);
            Graphics.DrawLine(new Vector2(200, -100), new Vector2(-128, 100), 50, 0, Color.Green);

            Graphics.End();

            base.Draw(gameTime);
        }
    }
}
