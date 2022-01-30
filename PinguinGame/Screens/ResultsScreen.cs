using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Audio;
using PinguinGame.Input;
using PinguinGame.Player;
using PinguinGame.Screens.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TinyGames.Engine.Graphics.Fonts;
using TinyGames.Engine.Graphics.Fonts.LoadersAndGenerators;

namespace PinguinGame.Screens
{
    public class ResultsScreen : Screen
    {
        private readonly IInputService _inputService;
        private readonly IScreenService _screens;
        private readonly IMusicService _musicService;

        private PlayerInfo _winner;
        private Fight _fight;

        public UIResultScreen _ui;


        public ResultsScreen(IScreenService screens, IInputService inputService, IMusicService musicService, Fight fight)
        {
            _inputService = inputService;
            _screens = screens;
            _fight = fight;
            _winner = _fight.Scoreboard.First().Player;
            _musicService = musicService;
        }

        public override void Init(GraphicsDevice device, ContentManager content)
        {
            base.Init(device, content);

            var winner = _winner;
            var text = winner.Name;
            var color = winner.Color;

            _ui = new UIResultScreen(new Resources.ResultsResources(content), text, color);
            _ui.UpdateLayout(Camera.Bounds);

            _musicService.PlayVictoryMusic();
        }

        public override void UpdateSelf(float delta)
        {
            base.UpdateSelf(delta);

            var state = _inputService.GetInputStateForDevice(_winner.InputDevice);

            if (state.ActionPressed)
            {
                _ui.FadeOut();
                _screens.ShowTitleScreen();
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
