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
    public class ResultsGameState : GameState
    {
        public RoundResults Results;

        public float Timer = 0;
        public bool Accepted = false;

        public IUISoundService _uiSound;
        public UIIntermediateResultsScreen _ui;

        public ResultsGameState(RoundResults results)
        {
            Results = results;
        }

        public override void Init(IceGame world, GraphicsDevice device, ContentManager content)
        {
            base.Init(world, device, content);

            _uiSound = world.UISoundService;

            _ui = new UIIntermediateResultsScreen(new IntermediateResultsResources(content), new UIIntermediateResultsScreenModel()
            {
                NextLabel = "Next",
                Lines = World.Fight.Scoreboard.Select(x => new UIResultLineModel() { 
                    PlayerName = x.Player.Name,
                    Icon = x.Player.CharacterInfo.Icon,
                    Color = x.Player.Color,
                    
                    Score = x.Score,
                    ScoreLabel = "pnt.",

                    IsWinning = x.Winning,
                    WinningLabel = "Winning!",
                }).ToArray()
            });
            _ui.UpdateLayout(World.Camera.Bounds);
        }

        public override GameState Update(float delta)
        {
            Timer += delta;

            if(Timer > 1.5f && !Accepted && World.Players.Any(x => World.InputService.GetInputForPlayer(x).ActionPressed))
            {
                Accepted = true;
                Timer = 0;
                _ui.FadeOut();

                _uiSound.PlayNextScreen();

            }
            if(Timer > 0.5f && Accepted)
            {
                World.ResetIceBlocks();
                return new PreGameState();
            }

            _ui.UpdateLayout(World.Camera.Bounds);
            _ui.Update(delta);

            return this;
        }

        public override void Draw(Graphics2D graphics)
        {
            base.Draw(graphics);

            graphics.ClearDepthBuffer();

            _ui.Draw(graphics);
        }
    }
}
