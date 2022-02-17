using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Player;
using PinguinGame.Screens;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PinguinGame.MiniGames.Ice.GameStates
{
    internal class PostGameState : GameState
    {
        private IScreenService _screenService;
        private RoundResults Results;
        private float Timer = 0;

        public PostGameState(RoundResults results)
        {
            Results = results;
        }

        public override void Init(IceGame world, GraphicsDevice device, ContentManager content)
        {
            base.Init(world, device, content);

            _screenService = world.ScreenService;
        }

        public override GameState Update(float delta)
        {
            Timer += delta;

            World.Update(delta);

            World.CharacterCollisions.TryBonkCharacters();
            World.CharacterCollisions.TryDrownCharacters();

            if (Timer > 1)
            {
                if (World.Fight.HasWinner)
                {
                    _screenService.ShowResultScreen(World.Fight);
                }
                else
                {
                    World.RemoveAllCharacters();

                    return new ResultsGameState(Results);
                }
            }
            return this;
        }
    }
}
