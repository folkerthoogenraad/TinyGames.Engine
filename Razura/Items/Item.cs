using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Razura.Items
{
    public class Item
    {
        // =========================================== //
        // Tile class
        // =========================================== //
        public string Id { get; }

        public Item(string id)
        {
            Id = id;
        }

        // =========================================== //
        // Helpers
        // =========================================== //
        public static Item GetById(string id)
        {
            return Registry.Get(id);
        }

        public static ItemRegistry Registry => ItemRegistry.Instance;
    }
}
