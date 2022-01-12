using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
        private readonly InputService _inputService;
        private readonly IScreenService _screens;
        private PlayerInfo[] _players { get; }

        private UIMapSelectScreen _ui;
        private int SelectedIndex = 0;
        private bool _ready = false;

        public MapSelectScreen(IScreenService screens, InputService inputService, PlayerInfo[] players)
        {
            _inputService = inputService;
            _screens = screens;

            _players = players;
        }

        public override void Init(GraphicsDevice device, ContentManager content)
        {
            base.Init(device, content);

            _ui = new UIMapSelectScreen(new MapSelectResources(content));
            _ui.UpdateLayout(Camera.Bounds);
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
                    if (input.MenuLeftPressed && SelectedIndex > 0)
                    {
                        SelectedIndex--;
                        _ui.SetSelected(SelectedIndex);
                    }
                    if (input.MenuRightPressed && SelectedIndex < 3)
                    {
                        SelectedIndex++;
                        _ui.SetSelected(SelectedIndex);
                    }

                    if (input.BackPressed)
                    {
                        _screens.ShowPlayerSelectScreen();
                        _ui.BackAnimation();
                    }
                }
            }

            _ui.SetReady(_ready);
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
    }
}
