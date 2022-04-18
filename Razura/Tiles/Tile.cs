using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Razura.Tiles
{
    public class Tile
    {
        // =========================================== //
        // Tile class
        // =========================================== //
        public string Id { get; }

        public Tile(string id)
        {
            Id = id;
        }

        // =========================================== //
        // Helpers
        // =========================================== //
        public static Tile GetById(string id)
        {
            return Registry.Get(id);
        }

        public static TileRegistry Registry => TileRegistry.Instance;
    }
}
