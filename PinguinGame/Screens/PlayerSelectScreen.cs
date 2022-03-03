using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Audio;
using PinguinGame.Input;
using PinguinGame.Player;
using PinguinGame.Screens.Resources;
using PinguinGame.Screens.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Graphics.Fonts;
using TinyGames.Engine.Graphics.Fonts.LoadersAndGenerators;

namespace PinguinGame.Screens
{
    public class PlayerSelectScreen : Screen
    {
        private readonly PlayerCollection _playerCollection;

        private readonly IInputService _inputService;
        private readonly IScreenService _screens;
        private readonly IMusicService _musicService;
        private readonly IUISoundService _uiSound;

        private HashSet<PlayerInfo> _readyPlayers;
        private UISelectPlayers _ui;

        public PlayerSelectScreen(IScreenService screens, IInputService inputService, IMusicService music, IUISoundService uiSound)
        {
            _playerCollection = new PlayerCollection();
            _inputService = inputService;
            _screens = screens;
            _uiSound = uiSound;

            _readyPlayers = new HashSet<PlayerInfo>();
            _musicService = music;
        }

        public override void Init(IGraphicsService graphicsService, ContentManager content)
        {
            base.Init(graphicsService, content);

            _ui = new UISelectPlayers(new PlayerSelectResources(content));
            _ui.UpdateLayout(Camera.Bounds);

            _ui.SetModel(new UISelectPlayersModel()
            {
                PlayerStates = _playerCollection.AllPlayers.Select(x => PlayerInfoToState(x)).ToArray(),
                ShowContinueButton = false,
            });

            _musicService.PlayMenuMusic();
        }

        public override void UpdateSelf(float delta)
        {
            base.UpdateSelf(delta);

            bool fadeForward = false;
            bool fadeBackwards = false;

            foreach (var input in _inputService.InputStates)
            {
                var isJoined = _playerCollection.IsPlayerJoinedByInputDevice(input.Type);
                var player = _playerCollection.GetPlayerByInputDevice(input.Type);

                if (!isJoined && input.ActionPressed)
                {
                    _uiSound.PlayAccept();
                    _playerCollection.GetOrJoinPlayerByInputDevice(input.Type);
                }
                else if (isJoined && input.ActionPressed && !_readyPlayers.Contains(player))
                {
                    _uiSound.PlayAccept();
                    _readyPlayers.Add(player);
                }
                else if (isJoined && input.ActionPressed && _readyPlayers.Contains(player) && CanStart)
                {
                    _uiSound.PlayNextScreen();
                    _screens.ShowCharacterSelectScreen(_playerCollection.Players.ToArray());
                    fadeForward = true;
                }

                else if (isJoined && input.BackPressed && !_readyPlayers.Contains(player))
                {
                    _uiSound.PlayBack();
                    player.Joined = false;
                }
                else if (isJoined && input.BackPressed && _readyPlayers.Contains(player))
                {
                    _uiSound.PlayBack();
                    _readyPlayers.Remove(player);
                }
                else if(!isJoined && input.BackPressed)
                {
                    _uiSound.PlayBack();
                    _screens.ShowMenuScreen();
                    fadeBackwards = true;
                }

            }

            _ui.SetModel(new UISelectPlayersModel()
            {
                PlayerStates = _playerCollection.AllPlayers.Select(x => PlayerInfoToState(x)).ToArray(),
                ShowContinueButton = CanStart,
                FadeFoward = fadeForward,
                FadeBackwards = fadeBackwards
            });
        }
        public override void UpdateAnimation(float delta)
        {
            base.UpdateAnimation(delta);

            _ui.Update(delta);
        }

        public override void Draw()
        {
            base.Draw();

            Graphics.Clear(Color.MistyRose);

            Graphics.Begin(Camera.GetMatrix());

            _ui.Draw(Graphics);

            Graphics.End();
        }

        private UISelectPlayersModel.PlayerState PlayerInfoToState(PlayerInfo info)
        {
            if (info.Joined)
            {
                if (_readyPlayers.Contains(info))
                {
                    return UISelectPlayersModel.PlayerState.Ready;
                }
                return UISelectPlayersModel.PlayerState.Joined;
            }
            else
            {
                return UISelectPlayersModel.PlayerState.UnJoined;
            }
        }

        public bool AllReady => _playerCollection.Players.All(x => _readyPlayers.Contains(x));
        public bool CanStart => _playerCollection.PlayerCount >= 2 && AllReady;
    }
}
