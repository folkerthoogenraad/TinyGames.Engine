using PinguinGame.Player;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PinguinGame.Screens
{
    public class FightSettings
    {
        public int GoalScore = 10;

        private Dictionary<int, int[]> _scoresForPlayerCount;

        public FightSettings()
        {
            _scoresForPlayerCount = new Dictionary<int, int[]>();

            _scoresForPlayerCount[2] = new int[] { 3, 0 };
            _scoresForPlayerCount[3] = new int[] { 3, 1, 0 };
            _scoresForPlayerCount[4] = new int[] { 4, 2, 1, 0 };

            Debug.Assert(ValidateScoresForPlayerCount());
        }


        public int[] GetScoresForPlayerCount(int playerCount)
        {
            if (!_scoresForPlayerCount.ContainsKey(playerCount)) throw new ArgumentException("Invalid player count (" + playerCount + ")");
            return _scoresForPlayerCount[playerCount];
        }

        private bool ValidateScoresForPlayerCount()
        {
            foreach(var key in _scoresForPlayerCount.Keys)
            {
                if (key != _scoresForPlayerCount[key].Length) return false;
            }
            return true;
        }
    }

    public class RoundResults
    {
        public PlayerInfo[] Order;
        public PlayerInfo Winner => Order.FirstOrDefault();

        public RoundResults(PlayerInfo[] order)
        {
            Order = order;
        }
    }

    public class Fight
    {
        public FightSettings Settings { get; set; }

        private List<RoundResults> _rounds;

        public PlayerInfo Winner { get; private set; } = null;

        public bool HasWinner => Winner != null;

        public PlayerInfo[] Players { get; }
        public int[] Scores { get; private set; }

        public Fight(PlayerInfo[] players)
        {
            Players = players;
            Scores = new int[Players.Length];

            _rounds = new List<RoundResults>();
            Settings = new FightSettings();
        }

        public int GetScoreForPlayer(PlayerInfo player)
        {
            Debug.Assert(Players.Contains(player));

            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i] == player) return Scores[i];
            }

            throw new ArgumentException("Player does not exist in the list of players.");
        }

        public bool IsPlayerWinning(PlayerInfo player)
        {
            return GetScoreForPlayer(player) >= Settings.GoalScore;
        }

        public void AddScoreForPlayer(PlayerInfo player, int scoreAdd)
        {
            Debug.Assert(Players.Contains(player));

            SetScoreForPlayer(player, GetScoreForPlayer(player) + scoreAdd);
        }

        public void SetScoreForPlayer(PlayerInfo player, int score)
        {
            Debug.Assert(Players.Contains(player));

            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i] == player)
                {
                    Scores[i] = score;
                }
            }
        }

        public IEnumerable<(PlayerInfo Player, int Score, bool Winning)> All()
        {
            for(int i = 0; i < Players.Length; i++)
            {
                yield return (Players[i], Scores[i], IsPlayerWinning(Players[i]));
            }
        }

        public void AddRound(RoundResults result)
        {
            _rounds.Add(result);

            if (IsPlayerWinning(result.Winner))
            {
                Winner = result.Winner;
            }

            var playerCount = Players.Length;
            var scores = Settings.GetScoresForPlayerCount(Players.Length);

            Debug.Assert(playerCount == result.Order.Length);
            Debug.Assert(playerCount == scores.Length);

            for (int i = 0; i < playerCount; i++)
            {
                var score = scores[i];
                var player = result.Order[i];

                AddScoreForPlayer(player, score);
            }
        }

        public IEnumerable<(PlayerInfo Player, int Score, bool Winning)> Scoreboard => All().OrderByDescending(x => x.Score);
    }
}
