using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Audio;
using PinguinGame.Input;
using PinguinGame.Pinguins;
using PinguinGame.Pinguins.Levels.Loader;
using PinguinGame.Player;
using PinguinGame.Screens.States;
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
    public class InGameScreen : Screen
    {
        private readonly PlayerInfo[] _players;
        private readonly InputService _inputService;
        private readonly IScreenService _screens;
        private readonly IMusicService _musicService;

        private GameUISkin Skin;

        public PenguinWorld World;

        private GameState _gameState;
        public GameState State
        {
            get => _gameState;
            set
            {
                if(value != _gameState)
                {
                    _gameState?.Destroy();
                    _gameState = value;
                    _gameState?.Init(World, Device, Content, Skin);
                }
            }
        }

        public InGameScreen(IScreenService screens, InputService inputService, IMusicService music, PlayerInfo[] players)
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

            World = new PenguinWorld(device, content.LoadLevel("Levels/level0"), _players, _inputService);
            World.Camera = Camera;

            Skin = new GameUISkin();
            Skin.Font = content.LoadFont("Fonts/Font8x10");
            Skin.FontOutline = FontOutline.Create(device, Skin.Font);

            State = new PreGameState();

            _musicService.PlayInGameMusic();
        }

        public override void UpdateSelf(float delta)
        {
            base.UpdateSelf(delta);

            State = State.Update(delta);

            if (World.Fight.Done)
            {
                _screens.ShowResultScreen(World.Fight);
            }
        }

        public override void Draw()
        {
            base.Draw();


            Graphics.Begin(Camera.GetMatrix());

            State.Draw(Graphics);

            Graphics.End();
        }

    }
}
