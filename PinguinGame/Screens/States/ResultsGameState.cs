using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Pinguins;
using PinguinGame.Screens.UI;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Graphics.Fonts;
using TinyGames.Engine.Graphics.Fonts.LoadersAndGenerators;

namespace PinguinGame.Screens.States
{
    public class ResultsGameState : GameState
    {
        public RoundResults Results;

        public CenterTextUI _ui;

        public float Timer = 0;

        public ResultsGameState(RoundResults results)
        {
            Results = results;
        }

        public override void Init(PenguinWorld world, GraphicsDevice device, ContentManager content, GameUISkin skin)
        {
            base.Init(world, device, content, skin);
            
            var result = $"Player {Results.Winner.Index + 1} wins!";

            _ui = new CenterTextUI(skin, result);
        }

        public override GameState Update(float delta)
        {
            Timer += delta;

            if(Timer > 1.5f)
            {
                World.Fight.AddScoreForPlayer(Results.Winner, 1);

                return new PreGameState();
            }

            _ui.Update(delta, World.Camera.Bounds);

            return this;
        }

        public override void Draw(Graphics2D graphics)
        {
            base.Draw(graphics);

            _ui.Draw(graphics);
        }
    }
}
