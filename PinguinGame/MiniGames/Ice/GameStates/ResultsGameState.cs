using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Audio;
using PinguinGame.Screens;
using PinguinGame.Screens.Resources;
using PinguinGame.Screens.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Graphics.Fonts;
using TinyGames.Engine.Graphics.Fonts.LoadersAndGenerators;

namespace PinguinGame.MiniGames.Ice.GameStates
{
    public class ResultsGameState : IceGameState<int>
    {
        private Fight _fight;
        private RoundResults _results;

        public float Timer = 0;
        public bool Accepted = false;

        public IUISoundService _uiSound;
        public UIIntermediateResultsScreen _ui;

        public ResultsGameState(IceGame game, Fight fight, RoundResults results) : base(game)
        {
            _results = results;
            _fight = fight;

            _uiSound = game.UISoundService;
        }

        public override void Init()
        {
            base.Init();

            _ui = new UIIntermediateResultsScreen(new IntermediateResultsResources(Content), new UIIntermediateResultsScreenModel()
            {
                NextLabel = "Next",
                Lines = _fight.Scoreboard.Select(x => new UIResultLineModel() { 
                    PlayerName = x.Player.Name,
                    Icon = x.Player.CharacterInfo.Icon,
                    Color = x.Player.Color,
                    
                    Score = x.Score,
                    ScoreLabel = "pnt.",

                    IsWinning = x.Winning,
                    WinningLabel = "Winning!",
                }).ToArray()
            });
            _ui.UpdateLayout(Game.Camera.Bounds);
        }

        public override void Update(float delta)
        {
            Timer += delta;

            if(Timer > 1.5f && !Accepted && Game.Players.Any(x => Game.InputService.GetInputForPlayer(x).ActionPressed))
            {
                Accepted = true;
                Timer = 0;
                _ui.FadeOut();

                _uiSound.PlayNextScreen();

            }
            if(Timer > 0.5f && Accepted)
            {
                Complete(0);
                return;
            }

            _ui.UpdateLayout(Game.Camera.Bounds);
            _ui.Update(delta);
        }

        public override void Draw(Graphics2D graphics)
        {
            base.Draw(graphics);

            graphics.ClearDepthBuffer();

            _ui.Draw(graphics);
        }
    }
}
