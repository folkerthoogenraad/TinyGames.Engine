﻿using Microsoft.Xna.Framework;
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
        private readonly IInputService _inputService;
        private readonly IScreenService _screens;
        private readonly IMusicService _musicService;
        private readonly IUISoundService _uiSound;
        private readonly ICharactersService _charactersService;

        private UIMenuScreen _ui;

        private int Selected = 0;

        public MenuScreen(IScreenService screens, IInputService inputService, IMusicService music, IUISoundService uiSound, ICharactersService charactersService)
        {
            _inputService = inputService;
            _screens = screens;
            _musicService = music;
            _uiSound = uiSound;
            _charactersService = charactersService;
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
                        _uiSound.PlayNextScreen();
                        _screens.ShowPlayerSelectScreen();
                    }
                    else if(Selected == 1)
                    {
                        var players = new PlayerInfo[] {
                            new PlayerInfo() { Index = 0, InputDevice = InputDeviceType.Keyboard0 },
                            new PlayerInfo() { Index = 1, InputDevice = InputDeviceType.Keyboard1 },
                            new PlayerInfo() { Index = 2, InputDevice = InputDeviceType.Gamepad0 },
                            new PlayerInfo() { Index = 3, InputDevice = InputDeviceType.Gamepad1 },
                        };

                        foreach (var player in players.Where(x => x.CharacterInfo == null))
                        {
                            player.CharacterInfo = _charactersService.GetDefaultForPlayer(player);
                        }

                        var round = new RoundResults(players);
                        var fight = new Fight(players);

                        while (!fight.HasWinner) fight.AddRound(round);

                        _screens.ShowResultScreen(fight);                        
                    }
                    else if(Selected == 2)
                    {
                        _screens.Exit();
                    }
                }
                if (input.BackPressed)
                {
                    _uiSound.PlayBack();
                    _screens.ShowTitleScreen();
                    back = true;
                }

                if (input.MenuDownPressed)
                {
                    _uiSound.PlaySelect();
                    Selected++;
                }
                if (input.MenuUpPressed)
                {
                    _uiSound.PlaySelect();
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
