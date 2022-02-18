using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Audio;
using PinguinGame.Input;
using PinguinGame.MiniGames.Ice;
using PinguinGame.MiniGames.Ice.GameStates;
using PinguinGame.MiniGames.Ice.CharacterStates;
using PinguinGame.Player;
using PinguinGame.Screens.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Graphics.Fonts.LoadersAndGenerators;
using TinyGames.Engine.Util;
using PinguinGame.MiniGames.Generic;
using PinguinGame.MiniGames.Ice.GameModes;
using PinguinGame.GameStates;

namespace PinguinGame.Screens
{
    public class InGameIceScreen : Screen
    {
        public IServiceProvider Services { get; }

        private readonly PlayerInfo[] _players;

        private readonly IInputService _inputService;
        private readonly IScreenService _screens;
        private readonly IMusicService _musicService;
        private readonly IUISoundService _uiSoundService;
        private readonly LevelInfo _level;

        public IceGame World;
        public IceGameMode<Fight> GameMode;

        public InGameIceScreen(IServiceProvider services, IScreenService screens, IInputService inputService, IMusicService music, IUISoundService uiSoundService, PlayerInfo[] players, LevelInfo level)
        {
            Services = services;
            _players = players;
            _inputService = inputService;
            _screens = screens;
            _musicService = music;
            _uiSoundService= uiSoundService;
            _level = level;
        }

        public override void Init(GraphicsDevice device, ContentManager content)
        {
            base.Init(device, content);

            // TODO this is very weirdly coupled tbh
            World = new IceGame(Services, _level, _players, new IceInput(_inputService), _uiSoundService, _screens);
            World.Camera = Camera;

            GameMode = new DeathMatchGameMode(World);
            GameMode.Run();

            _musicService.PlayInGameMusic();
        }

        public override void UpdateSelf(float delta)
        {
            base.UpdateSelf(delta);

            GameMode.Update(delta);

            if (GameMode.Done)
            {
                _screens.ShowResultScreen(GameMode.GetResult());
            }
        }

        public override void Draw()
        {
            base.Draw();

            Graphics.Begin(Camera.GetMatrix());

            GameMode.Draw(Graphics);

            Graphics.End();
        }

        public override void Destroy()
        {
            base.Destroy();

            World.Destroy();
            GameMode.Destroy();
        }
    }
}
