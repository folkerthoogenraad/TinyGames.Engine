using Razura.Registries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Razura.Tiles
{
    public class TileRegistry : RegistryBase<Tile>
    {
        private TileRegistry() : base() { }

        public static void Init()
        {

        }

        // =========================================== //
        // Singleton
        // =========================================== //
        static TileRegistry()
        {
            Instance = new TileRegistry();
        }
        public static TileRegistry Instance { get; }
    }
}
