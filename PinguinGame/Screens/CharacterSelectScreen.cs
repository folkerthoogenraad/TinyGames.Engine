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
    public class CharacterSelectScreen : Screen
    {
        private readonly IInputService _inputService;
        private readonly IScreenService _screens;
        private readonly IMusicService _musicService;
        private readonly ICharactersService _characterService;
        private readonly IUISoundService _uiSound;

        private PlayerInfo[] _players;
        private UICharacterSelect _ui;

        private HashSet<PlayerInfo> _readyPlayers;
        private Dictionary<PlayerInfo, int> _playerIndices;

        public CharacterSelectScreen(IScreenService screens, IInputService inputService, IMusicService music, ICharactersService characterService, IUISoundService uiSound, PlayerInfo[] players)
        {
            _players = players;
            _inputService = inputService;
            _screens = screens;

            _musicService = music;
            _characterService = characterService;

            _uiSound = uiSound;

            foreach (var player in players.Where(x => x.CharacterInfo == null))
            {
                player.CharacterInfo = _characterService.GetDefaultForPlayer(player);
            }

            _readyPlayers = new HashSet<PlayerInfo>();
            _playerIndices = new Dictionary<PlayerInfo, int>();

            foreach(var player in players)
            {
                _playerIndices[player] = player.Index;
            }
        }

        public override void Init(IGraphicsService graphicsService, ContentManager content)
        {
            base.Init(graphicsService, content);

            _ui = new UICharacterSelect(new CharacterSelectResources(content), CreateModel());
            _ui.UpdateLayout(Camera.Bounds);

            _musicService.PlayMenuMusic();
        }

        public override void UpdateSelf(float delta)
        {
            base.UpdateSelf(delta);

            foreach (var input in _inputService.InputStates)
            {
                var player = _players.Where(x => x.InputDevice == input.Type).FirstOrDefault();

                if (player == null) continue;

                if (input.ActionPressed)
                {
                    if (_readyPlayers.Contains(player) && AllReady)
                    {
                        _uiSound.PlayNextScreen();
                        _ui.FadeOut();
                        _screens.ShowMapSelectScreen(_players);
                    }
                    else
                    {
                        _uiSound.PlayAccept();
                        _readyPlayers.Add(player);
                    }
                }
                if (input.BackPressed)
                {
                    if (_readyPlayers.Contains(player))
                    {
                        _uiSound.PlayBack();
                        _readyPlayers.Remove(player);
                    }
                    else
                    {
                        _uiSound.PlayBack();
                        _screens.ShowPlayerSelectScreen();
                    }
                }
                if (input.MenuRightPressed && !_readyPlayers.Contains(player))
                {
                    int index = _playerIndices[player];
                    index++;

                    while (index < _characterService.GetCharacters().Length)
                    {
                        if(_playerIndices.All(x => x.Value != index))
                        {
                            _uiSound.PlaySelect();
                            _playerIndices[player] = index;
                            break;
                        }
                        index++;
                    }
                }
                if (input.MenuLeftPressed && !_readyPlayers.Contains(player))
                {
                    int index = _playerIndices[player];
                    index--;

                    while (index >= 0)
                    {
                        if (_playerIndices.All(x => x.Value != index))
                        {
                            _uiSound.PlaySelect();
                            _playerIndices[player] = index;
                            break;
                        }
                        index--;
                    }
                }
            }


            foreach (var (p, index) in _playerIndices.Select(x => (x.Key, x.Value)))
            {
                p.CharacterInfo = _characterService.GetCharacters()[index];
            }

            _ui.SetModel(CreateModel());

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

        private UICharacterSelectModel CreateModel()
        {
            return new UICharacterSelectModel()
            {
                CharacterIcons = _characterService.GetCharacters().Select(x => x.Icon).ToArray(),
                Colors = _players.Select(x => x.Color).ToArray(),
                Ready = _players.Select(x => _readyPlayers.Contains(x)).ToArray(),
                DisplayOk = AllReady,
                SelectedIndices = _players.Select(x => _playerIndices[x]).ToArray(),
            };
        }
        public bool AllReady => _players.All(x => _readyPlayers.Contains(x));
    }
}
