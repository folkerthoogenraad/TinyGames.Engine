using Microsoft.Xna.Framework.Content;
using PinguinGame.IO;
using PinguinGame.MiniGames.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.IO;

namespace PinguinGame.Levels
{
    public class LevelsService : ILevelsService
    {
        private readonly string LevelDataLocation = "Data/Levels.json";

        private IStorageSystem _storage;

        private LevelInfo[] _levels;

        public LevelsService(IStorageSystem storage, ContentManager content)
        {
            _storage = storage;

            MiniGameLoader loader = new MiniGameLoader(storage, content);

            _levels = loader.LoadLevelInfos(LevelDataLocation).ToArray();
        }

        public IEnumerable<LevelInfo> GetLevels()
        {
            return _levels;
        }
    }
}
