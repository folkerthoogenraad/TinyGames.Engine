using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TinyGames.Engine.Extensions;
using TinyGames.Engine.Graphics;

using System.Linq;
using TinyGames.Engine.Maths;
using TinyGames.Engine.Collisions;
using System.Collections.Generic;
using PinguinGame.Input;
using PinguinGame.Player;
using PinguinGame.Screens;

namespace PinguinGame
{
    public class PinguinGame : Game, IScreenService
    {
        private GraphicsDeviceManager _graphics;

        private InputService inputService;
        private PlayerService playerService;

        private Screen _screen;

        public Screen Screen
        {
            get => _screen;
            set
            {
                if (_screen != value)
                {
                    _screen?.Destroy();
                    _screen = value;
                    _screen?.Init(GraphicsDevice, Content);
                }
            }
        }

        public PinguinGame()
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

            inputService = new InputService();
            playerService = new PlayerService();

            ShowPlayerSelectScreen();
        }

        protected override void LoadContent()
        {
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float delta = gameTime.GetDeltaInSeconds();

            inputService.Poll();

            Screen.Update(delta);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Screen.Draw();
        }

        public void ShowPlayerSelectScreen()
        {
            Screen = new PlayerSelectScreen(this, playerService, inputService);
        }
        public void ShowInGameScreen()
        {
            Screen = new InGameScreen(this, playerService, inputService);
        }
    }
}
