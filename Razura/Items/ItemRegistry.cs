using Razura.Registries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Razura.Items
{
    public class ItemRegistry : RegistryBase<Item>
    {
        private ItemRegistry() : base() { }
        
        public static void Init()
        {

        }

        // =========================================== //
        // Singleton
        // =========================================== //
        static ItemRegistry()
        {
            Instance = new ItemRegistry();
        }
        public static ItemRegistry Instance { get; }
    }
}
