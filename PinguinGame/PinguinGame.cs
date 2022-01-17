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
using PinguinGame.Audio;

namespace PinguinGame
{
    public class PinguinGame : Game, IScreenService
    {
        private GraphicsDeviceManager _graphics;

        private InputService inputService;
        private PlayerService playerService;
        private MusicService musicService;

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

            // _graphics.PreferredBackBufferWidth = 1024;
            // _graphics.PreferredBackBufferHeight = 768;
            // _graphics.ApplyChanges();

            _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();


            inputService = new InputService();
            playerService = new PlayerService();
            musicService = new MusicService(Content);

            Manager = new ScreenManager(GraphicsDevice, Content);

            ShowInGameScreen(new PlayerInfo[] {
                new PlayerInfo() { Index = 0, InputDevice = InputDevice.Keyboard0 },
                new PlayerInfo() { Index = 1, InputDevice = InputDevice.Keyboard1 },
            });
            // ShowSplashScreen();
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
            Manager.Screen = new PlayerSelectScreen(this, playerService, inputService, musicService);
        }
        public void ShowInGameScreen(PlayerInfo[] players)
        {
            Manager.Screen = new InGameScreen(this, inputService, musicService, players);
        }

        public void ShowResultScreen(Fight fight)
        {
            Manager.Screen = new ResultsScreen(this, inputService, musicService, fight);
        }

        public void ShowTitleScreen()
        {
            Manager.Screen = new TitleScreen(this, inputService, musicService);
        }

        public void ShowMenuScreen()
        {
            Manager.Screen = new MenuScreen(this, inputService, musicService);
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
            Manager.Screen = new MapSelectScreen(this, inputService, musicService, players);
        }
    }
}
