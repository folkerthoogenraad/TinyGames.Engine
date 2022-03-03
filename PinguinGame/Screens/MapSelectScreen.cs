using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Audio;
using PinguinGame.Input;
using PinguinGame.Levels;
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
    public class MapSelectScreen : Screen
    {
        private readonly IInputService _inputService;
        private readonly IScreenService _screens;
        private readonly IMusicService _musicService;
        private readonly ILevelsService _levelsService;
        private readonly IUISoundService _uiSound;
        private PlayerInfo[] _players { get; }

        private UIMapSelectScreen _ui;
        private int _selectedIndex = 0;
        private bool _ready = false;

        public MapSelectScreen(IScreenService screens, IInputService inputService, IMusicService music, ILevelsService levelsService, IUISoundService uiSound, PlayerInfo[] players)
        {
            _inputService = inputService;
            _screens = screens;

            _players = players;
            _musicService = music;
            _levelsService = levelsService;
            _uiSound = uiSound;
        }

        public override void Init(IGraphicsService graphicsService, ContentManager content)
        {
            base.Init(graphicsService, content);

            _ui = new UIMapSelectScreen(new MapSelectResources(content), new UIMapSelectModel()
            {
                Maps = GetMaps().ToArray(),
                Ready = _ready,
                SelectedIndex = _selectedIndex
            });

            _ui.UpdateLayout(Camera.Bounds);

            _musicService.PlayMenuMusic();
        }

        public override void UpdateSelf(float delta)
        {
            base.UpdateSelf(delta);

            foreach (var input in _inputService.InputStates)
            {
                if (_ready)
                {
                    if (input.ActionPressed)
                    {
                        var level = _levelsService.GetLevels().Skip(_selectedIndex).FirstOrDefault();
                        _screens.ShowInGameScreen(_players, level);
                        _ui.AcceptAnimation();
                        _uiSound.PlayStart();
                    }
                    if (input.BackPressed)
                    {
                        _ready = false;
                        _uiSound.PlayBack();
                    }
                }
                else
                {
                    if (input.ActionPressed)
                    {
                        _ready = true;
                        _uiSound.PlayAccept();
                    }
                    if (input.MenuLeftPressed && _selectedIndex > 0)
                    {
                        _selectedIndex--;
                        _uiSound.PlaySelect();
                    }
                    if (input.MenuRightPressed && _selectedIndex < _levelsService.GetLevels().Count() - 1)
                    {
                        _selectedIndex++;
                        _uiSound.PlaySelect();
                    }

                    if (input.BackPressed)
                    {
                        _uiSound.PlayBack();
                        _screens.ShowCharacterSelectScreen(_players);
                        _ui.BackAnimation();
                    }
                }
            }

            _ui.SetModel(new UIMapSelectModel()
            {
                Maps = GetMaps().ToArray(),
                Ready = _ready,
                SelectedIndex = _selectedIndex
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

        public IEnumerable<UIMapModel> GetMaps()
        {
            foreach(var level in _levelsService.GetLevels())
            {
                yield return new UIMapModel() { Locked = false, Text = level.Description, Icon = level.Icon };
            }
        }
    }
}
