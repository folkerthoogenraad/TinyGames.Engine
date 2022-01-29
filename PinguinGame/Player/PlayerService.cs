using PinguinGame.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PinguinGame.Player
{
    // Join players?
    // This can be more of a factory where you can 
    // Do a "create players" to get a Players players; object.
    public class PlayerService
    {
        private PlayerInfo[] _players;

        public IEnumerable<PlayerInfo> AllPlayers => _players;
        public IEnumerable<PlayerInfo> Players => _players.Where(x => x.Joined);
        public int PlayerCount => Players.Count();

        public PlayerService()
        {
            _players = new PlayerInfo[4];

            for(int i = 0; i < _players.Length; i++)
            {
                _players[i] = new PlayerInfo() 
                { 
                    Index = i,
                };
            }
        }

        public PlayerInfo GetPlayerByIndex(int index)
        {
            return _players[index];
        }

        public PlayerInfo GetOrJoinPlayerByInputDevice(InputDeviceType controller)
        {
            var result = GetPlayerByInputDevice(controller);

            if (result != null) return result;

            result = GetNextAvailablePlayerInfo();

            if (result == null) return null;

            result.Joined = true;
            result.InputDevice = controller;

            return result;
        }
        public bool IsPlayerJoinedByInputDevice(InputDeviceType device)
        {
            var player = GetPlayerByInputDevice(device);

            return player != null && player.Joined == true;
        }
        public PlayerInfo GetPlayerByInputDevice(InputDeviceType device)
        {
            return Players.Where(x => x.InputDevice == device).FirstOrDefault();
        }

        public PlayerInfo GetNextAvailablePlayerInfo()
        {
            return _players.Where(x => !x.Joined).FirstOrDefault();
        }

        public void UnJoinAll()
        {
            foreach(var player in Players)
            {
                player.Joined = false;
            }
        }
    }
}
