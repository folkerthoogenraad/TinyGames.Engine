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
using TinyGames.Engine.IO;
using TinyGames.Engine.Collections;
using PinguinGame.Levels;
using PinguinGame.MiniGames.Generic;
using PinguinGame.Settings;

namespace PinguinGame
{
    public class PinguinGame : Game, IScreenService
    {
        private GraphicsDeviceManager _graphics;

        public ScreenManager Manager;
        public IInputService Input;

        public PinguinGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();

            //_graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            //_graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            //_graphics.IsFullScreen = true;
            //_graphics.ApplyChanges();

            Services.AddService<IStorageSystem>(new StorageSystem(new DiskStorageProvider(Content.RootDirectory)));
            Services.AddService<ISettingsService, SettingsService>();
            Services.AddService<IInputService>(new InputService());
            Services.AddService<IMusicService>(new MusicService(Content, Services.GetService<ISettingsService>()));
            Services.AddService<IUISoundService>(new UISoundService(Content));
            Services.AddService<ICharactersService>(new CharactersService(Services.GetService<IStorageSystem>(), Content));
            Services.AddService<ILevelsService>(new LevelsService(Services.GetService<IStorageSystem>(), Content));
            Services.AddService<IScreenService>(this);

            Input = Services.GetService<IInputService>();

            Manager = new ScreenManager(GraphicsDevice, Content);

            var players = new PlayerInfo[] {
                new PlayerInfo() { Index = 0, InputDevice = InputDeviceType.Gamepad0 },
                new PlayerInfo() { Index = 1, InputDevice = InputDeviceType.Keyboard1 },
            };

            foreach (var player in players.Where(x => x.CharacterInfo == null))
            {
                player.CharacterInfo = Services.GetService<ICharactersService>().GetDefaultForPlayer(player);
            }

            ShowMapSelectScreen(players);
            // ShowInGameScreen(players);
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

            Input.Poll();
            Manager.Update(delta);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Manager.Draw();
        }

        public void ShowPlayerSelectScreen()
        {
            Manager.Screen = new PlayerSelectScreen(
                Services.GetService<IScreenService>(),
                Services.GetService<IInputService>(),
                Services.GetService<IMusicService>(),
                Services.GetService<IUISoundService>()
                );
        }

        public void ShowCharacterSelectScreen(PlayerInfo[] players)
        {
            Manager.Screen = new CharacterSelectScreen(
                Services.GetService<IScreenService>(),
                Services.GetService<IInputService>(),
                Services.GetService<IMusicService>(),
                Services.GetService<ICharactersService>(),
                Services.GetService<IUISoundService>(),
                players);
        }

        public void ShowInGameScreen(PlayerInfo[] players, LevelInfo info)
        {
            Manager.Screen = new InGameIceScreen(
                Services.GetService<IScreenService>(),
                Services.GetService<IInputService>(),
                Services.GetService<IMusicService>(),
                Services.GetService<IUISoundService>(),
                players,
                info);
        }

        public void ShowResultScreen(Fight fight)
        {
            Manager.Screen = new ResultsScreen(
                Services.GetService<IScreenService>(),
                Services.GetService<IInputService>(),
                Services.GetService<IMusicService>(),
                Services.GetService<IUISoundService>(),
                fight);
        }

        public void ShowTitleScreen()
        {
            Manager.Screen = new TitleScreen(
                Services.GetService<IScreenService>(),
                Services.GetService<IInputService>(),
                Services.GetService<IMusicService>(),
                Services.GetService<IUISoundService>());
        }

        public void ShowMenuScreen()
        {
            Manager.Screen = new MenuScreen(
                Services.GetService<IScreenService>(),
                Services.GetService<IInputService>(),
                Services.GetService<IMusicService>(),
                Services.GetService<IUISoundService>());
        }
        public void ShowSplashScreen()
        {
            Manager.Screen = new SplashScreen(
                Services.GetService<IScreenService>());
        }

        public new void Exit() // Because of the screen service :) // This shouldn't be here probably
        {
            base.Exit();
        }

        public void ShowMapSelectScreen(PlayerInfo[] players)
        {
            Manager.Screen = new MapSelectScreen(
                Services.GetService<IScreenService>(),
                Services.GetService<IInputService>(),
                Services.GetService<IMusicService>(),
                Services.GetService<ILevelsService>(),
                Services.GetService<IUISoundService>(),
                players);
        }

    }
}
