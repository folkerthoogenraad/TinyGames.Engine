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
    public class MenuScreen : Screen
    {
        private readonly InputService _inputService;
        private readonly IScreenService _screens;
        private readonly IMusicService _musicService;

        private UIMenuScreen _ui;

        private int Selected = 0;

        public MenuScreen(IScreenService screens, InputService inputService, IMusicService music)
        {
            _inputService = inputService;
            _screens = screens;
            _musicService = music;
        }

        public override void Init(GraphicsDevice device, ContentManager content)
        {
            base.Init(device, content);

            _ui = new UIMenuScreen(new MenuResources(content));
            _ui.UpdateLayout(Camera.Bounds);

            _musicService.PlayMenuMusic();
        }

        public override void UpdateSelf(float delta)
        {
            base.UpdateSelf(delta);

            bool accepted = false;
            bool back = false;

            foreach (var input in _inputService.InputStates)
            {
                if (input.ActionPressed)
                {
                    accepted = true;
                    if(Selected == 0)
                    {
                        _screens.ShowPlayerSelectScreen();
                    }
                    else if(Selected == 1)
                    {
                        _screens.ShowResultScreen(new Fight(new PlayerInfo[] { new PlayerInfo() { Index = 0, InputDevice = InputDeviceType.Keyboard0 } }));
                    }
                    else if(Selected == 2)
                    {
                        _screens.Exit();
                    }
                }
                if (input.BackPressed)
                {
                    _screens.ShowTitleScreen();
                    back = true;
                }

                if (input.MenuDownPressed)
                {
                    Selected++;
                }
                if (input.MenuUpPressed)
                {
                    Selected--;
                }
            }

            Selected = Math.Clamp(Selected, 0, 2);

            if (accepted)
            {
                _ui.AcceptAnimation();
            }
            if (back)
            {
                _ui.BackAnimation();
            }

            _ui.SetSelected(Selected);
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
