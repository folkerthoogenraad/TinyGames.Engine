using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Audio;
using PinguinGame.Input;
using PinguinGame.MiniGames.Minecarts;
using PinguinGame.MiniGames.Minecarts.GameStates;
using PinguinGame.MiniGames.Minecarts.Loader;
using PinguinGame.Player;
using PinguinGame.Screens.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Graphics.Fonts.LoadersAndGenerators;
using TinyGames.Engine.Util;

namespace PinguinGame.Screens
{
    public class InGameMinecartScreen : Screen
    {
        private readonly PlayerInfo[] _players;
        private readonly InputService _inputService;
        private readonly IScreenService _screens;
        private readonly IMusicService _musicService;

        public MinecartGame Game { get; set; }

        private GameState _gameState;
        public GameState State
        {
            get => _gameState;
            set
            {
                if (value != _gameState)
                {
                    _gameState?.Destroy();
                    _gameState = value;
                    _gameState?.Init(Game, Device, Content);
                }
            }
        }

        public InGameMinecartScreen(IScreenService screens, InputService inputService, IMusicService music, PlayerInfo[] players)
        {
            _players = players;
            _inputService = inputService;
            _screens = screens;
            _musicService = music;

        }

        public override void Init(GraphicsDevice device, ContentManager content)
        {
            base.Init(device, content);

            Camera.Height = 180;

            var level = content.LoadMinecartLevel("Levels/minecart_level0");
            var levelGraphics = new MinecartLevelGraphics(device, new MinecartLevelGraphicsSettings());

            Game = new MinecartGame(content, device, level, levelGraphics, _players, new MinecartInputService(_inputService));
            State = new PlayingGameState();


            _musicService.PlayInGameMusic();
        }

        public override void UpdateSelf(float delta)
        {
            base.UpdateSelf(delta);

            State = State.Update(delta);

            if (_inputService.InputStates.Any(x => x.StartPressed))
            {
                Game.Spawn();
            }
        }

        public override void Draw()
        {
            base.Draw();

            Game.Draw();
        }

    }
}
