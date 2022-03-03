using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TinyGames.Engine.Extensions;
using TinyGames.Engine.Graphics;

using System.Linq;
using TinyGames.Engine.Maths;
using TinyGames.Engine.Collisions;
using TinyGames.Engine.Collisions.Contracts;
using System.Collections.Generic;
using TinyGames.Engine.Graphics.Fonts;
using TinyGames.Engine.Graphics.Fonts.LoadersAndGenerators;

namespace TinyGames
{


    public class TextWritingTestGame : Game
    {
        private GraphicsDeviceManager _graphics;

        public Graphics2D Graphics;
        public Camera Camera;

        public Vector2 MousePosition;

        public Font Font;
        public string Text = "";

        public TextWritingTestGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _graphics.PreferredBackBufferWidth = 1600;
            _graphics.PreferredBackBufferHeight = 900;
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            Graphics = new Graphics2D(GraphicsDevice);
            Camera = new Camera(180, 16f / 9f);

            Font = Content.LoadFont("Fonts/SansSerifFont");

            Window.TextInput += Window_TextInput;
        }

        private void Window_TextInput(object sender, TextInputEventArgs e)
        {
            if(e.Key == Keys.Back
                || e.Key == Keys.Escape
                || e.Key == Keys.Enter)
            {
                Console.WriteLine(e.Key + " " + e.Character);
                return;
            }

            Text += e.Character;
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }


        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float delta = gameTime.GetDeltaInSeconds();
            var keyState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            MousePosition = Camera.TransformMousePosition(
                new Vector2(((float)mouseState.X / _graphics.PreferredBackBufferWidth), ((float)mouseState.Y / _graphics.PreferredBackBufferHeight)));

            var keys = keyState.GetPressedKeys();
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Graphics.Clear(Color.White);

            Graphics.Begin(Camera.GetMatrix());

            Graphics.DrawString(Font, Text, new Vector2(0, 0), Color.Black);

            Graphics.End();
        }
    }
}

