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
        private readonly IUISoundService _uiSound;

        private PlayerInfo _winner;
        private Fight _fight;

        public UIResultScreen _ui;


        public ResultsScreen(IScreenService screens, IInputService inputService, IMusicService musicService, IUISoundService sound, Fight fight)
        {
            _inputService = inputService;
            _screens = screens;
            _fight = fight;
            _winner = _fight.Winner;
            _musicService = musicService;
            _uiSound = sound;
        }

        public override void Init(GraphicsDevice device, ContentManager content)
        {
            base.Init(device, content);

            var winner = _winner;
            var text = winner.Name;
            var color = winner.Color;

            _ui = new UIResultScreen(new Resources.ResultsResources(content), new UIResultsScreenModel()
            {
                BannerText = winner.Name,
                BannerColor = winner.Color,
                BackLabel = "Back",
                Lines = _fight.Scoreboard.Select(x => new UIResultLineModel()
                {
                    PlayerName = x.Player.Name,
                    Icon = x.Player.CharacterInfo.Icon,
                    Color = x.Player.Color,

                    Score = x.Score,
                    ScoreLabel = "pnt.",

                    IsWinning = x.Player == winner,
                    WinningLabel = "Winner!",
                }).ToArray()
            });
            _ui.UpdateLayout(Camera.Bounds);

            _musicService.PlayVictoryMusic();
        }

        public override void UpdateSelf(float delta)
        {
            base.UpdateSelf(delta);
            _ui.UpdateLayout(Camera.Bounds);

            var state = _inputService.GetInputStateForDevice(_winner.InputDevice);

            if (state.ActionPressed)
            {
                _uiSound.PlayAccept();
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
