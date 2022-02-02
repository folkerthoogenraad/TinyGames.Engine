using Microsoft.Xna.Framework.Content;
using PinguinGame.IO;
using PinguinGame.MiniGames.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.IO;

namespace PinguinGame.Player
{
    // TODO create a "CharacterInfoProvider" or whatever that does the reading and writing to
    // disk. Now currently this thing does not only the fetching from disk, it also
    // does the management. This should be seperated.
    public class CharactersService : ICharactersService
    {
        private readonly string CharacterDataLocation = "Data/Characters.json";
        //private readonly string CharacterUnlockDataLocation = "SaveData/Characters.json";

        private IStorageSystem _storage;

        private CharacterInfo[] _characters;
        
        public CharactersService(IStorageSystem storage, ContentManager content)
        {
            _storage = storage;

            MiniGameLoader loader = new MiniGameLoader(storage, content);

            _characters = loader.LoadCharacterInfos(CharacterDataLocation).ToArray();
        }

        public CharacterInfo[] GetCharacters()
        {
            return _characters;
        }

        public CharacterInfo GetDefaultForPlayer(PlayerInfo players)
        {
            return _characters[players.Index];
        }

        public bool IsCharacterUnlocked(CharacterInfo characterInfo)
        {
            // TODO implement
            return true;
        }
    }
}
