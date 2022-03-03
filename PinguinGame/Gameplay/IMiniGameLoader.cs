using Microsoft.Xna.Framework.Content;
using PinguinGame.IO.Characters;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.IO;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PinguinGame.Gameplay
{
    public interface IMiniGameLoader
    {
        public List<CharacterInfo> LoadCharacterInfos(string file);
        public List<LevelInfo> LoadLevelInfos(string file);
    }
}
