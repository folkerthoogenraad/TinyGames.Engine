using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TinyGames.Engine.Extensions;
using TinyGames.Engine.Graphics;

using System.Linq;
using TinyGames.Engine.Maths;
using TinyGames.Engine.Collisions;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using TinyGames.Engine.Graphics.Fonts;
using TinyGames.Engine.Graphics.Fonts.LoadersAndGenerators;
using TinyGames.Engine.Graphics.Fonts.LoadersAndGenerators.Contracts;
using System.IO;

namespace TinyGames
{

    public class FontTestGame : Game
    {
        private GraphicsDeviceManager _graphics;

        public Graphics2D Graphics;
        public Camera Camera;

        public Font Font;
        public Font OutlineFont;

        public FontTestGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            // graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

        }

        protected override void LoadContent()
        {
            var effect = Content.Load<Effect>("Effects/StandardEffect");
            effect = new BasicEffect(GraphicsDevice);

            Graphics = new Graphics2D(GraphicsDevice, effect);

            Camera = new Camera(360, 16f / 9f);


            Font = Content.LoadFont("Fonts/Font16x16");
            OutlineFont = FontOutline.Create(GraphicsDevice, Font);
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            float delta = gameTime.GetDeltaInSeconds();
            var keyState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Graphics.Clear(Color.DarkSlateGray);

            Graphics.Begin(Camera.GetMatrix());

            string text = "The quick brown fox jumps over the lazy frog";
            string text2 = "0123456789";
            string text3 = "Aardbei is een lekkere groente";

            DrawText(text, new Vector2(0, -16));
            DrawText(text2, new Vector2(0, 16));
            DrawText(text3, new Vector2(0, 16 + 32));

            Graphics.End();

            base.Draw(gameTime);
        }

        public void DrawText(string text, Vector2 position)
        {
            Vector2 size = Font.Measure(text);
            Vector2 scale = new Vector2(2, 2);

            //Graphics.DrawRectangle(position - size * scale * 0.5f, size * scale, Color.Green);

            Graphics.DrawString(Font, text, position, scale, Color.White, FontHAlign.Center, FontVAlign.Center);
            Graphics.DrawString(OutlineFont, text, position, scale, Color.Black, FontHAlign.Center, FontVAlign.Center);
        }
    }
}
