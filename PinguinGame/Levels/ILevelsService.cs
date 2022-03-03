using PinguinGame.Gameplay;
using PinguinGame.Gameplay.Generic;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Levels
{
    public interface ILevelsService
    {
        public IEnumerable<LevelInfo> GetLevels();
    }
}
