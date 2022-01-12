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

        public ScreenManager Manager;

        public PinguinGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();

            //_graphics.PreferredBackBufferWidth = 1440 * 16 / 9;
            //_graphics.PreferredBackBufferHeight = 1440;
            //_graphics.IsFullScreen = true;
            //_graphics.ApplyChanges();

            inputService = new InputService();
            playerService = new PlayerService();

            Manager = new ScreenManager(GraphicsDevice, Content);

            ShowSplashScreen();
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

            Manager.Update(delta);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Manager.Draw();
        }

        public void ShowPlayerSelectScreen()
        {
            Manager.Screen = new PlayerSelectScreen(this, playerService, inputService);
        }
        public void ShowInGameScreen(PlayerInfo[] players)
        {
            Manager.Screen = new InGameScreen(this, inputService, players);
        }

        public void ShowResultScreen(Fight fight)
        {
            Manager.Screen = new ResultsScreen(this, inputService, fight);
        }

        public void ShowTitleScreen()
        {
            Manager.Screen = new TitleScreen(this, inputService);
        }

        public void ShowMenuScreen()
        {
            Manager.Screen = new MenuScreen(this, inputService);
        }
        public void ShowSplashScreen()
        {
            Manager.Screen = new SplashScreen(this);
        }

        public new void Exit()
        {
            base.Exit();
        }

        public void ShowMapSelectScreen(PlayerInfo[] players)
        {
            Manager.Screen = new MapSelectScreen(this, inputService, players);
        }
    }
}
