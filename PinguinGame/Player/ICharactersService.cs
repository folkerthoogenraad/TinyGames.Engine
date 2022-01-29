using PinguinGame.MiniGames.Generic;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Player
{
    public interface ICharactersService
    {
        public CharacterInfo[] GetCharacters();
        public bool IsCharacterUnlocked(CharacterInfo characterInfo);
        public CharacterInfo GetDefaultForPlayer(PlayerInfo players);
    }
}
