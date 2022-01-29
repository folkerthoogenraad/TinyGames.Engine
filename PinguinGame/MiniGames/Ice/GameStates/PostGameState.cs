using PinguinGame.Player;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PinguinGame.MiniGames.Ice.GameStates
{
    public class RoundResults
    {
        public PlayerInfo[] Order;
        public PlayerInfo Winner => Order.FirstOrDefault();

        public RoundResults(PlayerInfo[] order)
        {
            Order = order;
        }
    }

    internal class PostGameState : GameState
    {
        private RoundResults Results;
        private float Timer = 0;

        public PostGameState(RoundResults results)
        {
            Results = results;
        }

        public override GameState Update(float delta)
        {
            Timer += delta;

            World.Update(delta);

            World.TryBonkCharacters();
            World.TryDrownCharacters();

            if (Timer > 1)
            {
                World.RemoveAllPenguins();

                return new ResultsGameState(Results);
            }
            return this;
        }
    }
}
