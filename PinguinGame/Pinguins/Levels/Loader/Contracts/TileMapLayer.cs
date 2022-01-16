using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Pinguins.Levels.Loader.Contracts
{
    public class TileMapLayer
    {
        public const string LayerTypeObjectGroup = "objectgroup";

        public string Name { get; set; }
        public string Type { get; set; }

        public TileMapObject[] Objects { get; set; }
    }
}
