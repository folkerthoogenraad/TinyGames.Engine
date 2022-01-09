using PinguinGame.Player;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PinguinGame.Screens
{
    public class Fight
    {
        public PlayerInfo[] Players { get; }
        public int[] Scores { get; private set; }

        public Fight(PlayerInfo[] players)
        {
            Players = players;
            Scores = new int[Players.Length];
        }

        public int GetScoreForPlayer(PlayerInfo player)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i] == player) return Scores[i];
            }
            return 0;
        }

        public void AddScoreForPlayer(PlayerInfo player, int scoreAdd)
        {
            SetScoreForPlayer(player, GetScoreForPlayer(player) + scoreAdd);
        }

        public void SetScoreForPlayer(PlayerInfo player, int score)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i] == player)
                {
                    Scores[i] = score;
                }
            }
        }

        public IEnumerable<(PlayerInfo Player, int Score)> All()
        {
            for(int i = 0; i < Players.Length; i++)
            {
                yield return (Players[i], Scores[i]);
            }
        }

        public bool Done => Scores.Any(x => x >= 3);

        public IEnumerable<(PlayerInfo Player, int Score)> Scoreboard => All().OrderByDescending(x => x.Score);
    }
}
