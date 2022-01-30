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
    public class MapSelectScreen : Screen
    {
        private readonly IInputService _inputService;
        private readonly IScreenService _screens;
        private readonly IMusicService _musicService;
        private PlayerInfo[] _players { get; }

        private UIMapSelectScreen _ui;
        private int _selectedIndex = 0;
        private bool _ready = false;

        public MapSelectScreen(IScreenService screens, IInputService inputService, IMusicService music, PlayerInfo[] players)
        {
            _inputService = inputService;
            _screens = screens;

            _players = players;
            _musicService = music;
        }

        public override void Init(GraphicsDevice device, ContentManager content)
        {
            base.Init(device, content);

            _ui = new UIMapSelectScreen(new MapSelectResources(content));

            _ui.UpdateLayout(Camera.Bounds);
            _ui.SetModel(new UIMapSelectModel()
            {
                Maps = GetMaps().ToArray(),
                Ready = _ready,
                SelectedIndex = _selectedIndex
            }, false);

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
                        _screens.ShowInGameScreen(_players);
                        _ui.AcceptAnimation();
                    }
                    if (input.BackPressed)
                    {
                        _ready = false;
                    }
                }
                else
                {
                    if (input.ActionPressed)
                    {
                        _ready = true;
                    }
                    if (input.MenuLeftPressed && _selectedIndex > 0)
                    {
                        _selectedIndex--;
                    }
                    if (input.MenuRightPressed && _selectedIndex < 3)
                    {
                        _selectedIndex++;
                    }

                    if (input.BackPressed)
                    {
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
            yield return new UIMapModel() { Locked = false, Text = "Default level" };
            yield return new UIMapModel() { Locked = true, Text = "Locked" };
            yield return new UIMapModel() { Locked = true, Text = "Locked" };
            yield return new UIMapModel() { Locked = true, Text = "Locked" };
        }
    }
}
