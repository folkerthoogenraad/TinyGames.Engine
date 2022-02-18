using Microsoft.Xna.Framework;
using PinguinGame.GameStates;
using PinguinGame.MiniGames.Ice.GameStates;
using PinguinGame.Screens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TinyGames.Engine.Maths;

namespace PinguinGame.MiniGames.Ice.GameModes
{
    public class DeathMatchGameMode : IceGameMode<Fight>
    {
        public IceGame Game { get; set; }

        public DeathMatchGameMode(IceGame game)
        {
            Game = game;
        }

        public async override Task RunSelf()
        {
            var fight = new Fight(Game.Players);

            while(true)
            {
                await GotoState(new WaitGameState(Game, new WaitGameStateSettings { Time = 1, ShouldUpdateWorld = false }));
            
                SpawnCharacters();

                await GotoState(new CountdownPlayState(Game));

                var result = await GotoState(new PlayingGameState(Game, fight));
                
                await GotoState(new FinishPlayState(Game));

                fight.AddRound(result);

                if (fight.HasWinner)
                {
                    break;
                }

                await GotoState(new ResultsGameState(Game, fight, result));

                Game.RemoveAllCharacters();
                Game.ResetIceBlocks();
            }

            SetResult(fight);
        }

        public void SpawnCharacters()
        {
            Vector2 spawnLocation = Game.FindApplicableSpawnLocation();

            float angle = 0;
            float anglePerPlayer = (MathF.PI * 2) / Game.Players.Length;

            foreach (var player in Game.Players)
            {
                Vector2 pos = spawnLocation + Tools.AngleVector(angle) * 16;

                Game.SpawnCharacter(pos, player);

                angle += anglePerPlayer;
            }

            Game.PlaceCharactersOnGround();

        }
    }
}
