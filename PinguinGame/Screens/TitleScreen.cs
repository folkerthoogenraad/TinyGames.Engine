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

namespace PinguinGame.Screens
{
    public class TitleScreen : Screen
    {
        private readonly IInputService _inputService;
        private readonly IMusicService _musicService;
        private readonly IScreenService _screens;
        private readonly IUISoundService _uiSound;

        private UITitleScreen _ui;

        public TitleScreen(IScreenService screens, IInputService inputService, IMusicService music, IUISoundService uiSound)
        {
            _inputService = inputService;
            _screens = screens;
            _musicService = music;
            _uiSound = uiSound;
        }

        public override void Init(GraphicsDevice device, ContentManager content)
        {
            base.Init(device, content);

            _ui = new UITitleScreen(new TitleResources(content));
            _ui.UpdateLayout(Camera.Bounds);

            _musicService.PlayTitleMusic();
        }

        public override void UpdateSelf(float delta)
        {
            base.UpdateSelf(delta);

            foreach(var input in _inputService.InputStates)
            {
                if (input.ActionPressed)
                {
                    _uiSound.PlayAccept();
                    _ui.FadeOut();
                    _screens.ShowMenuScreen();
                }
            }
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
