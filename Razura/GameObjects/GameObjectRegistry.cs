using Razura.Registries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyGames.Engine.Scenes;

namespace Razura.GameObjects
{
    public class GameObjectRegistry : RegistryBase<Func<GameObject>>
    {
        public static void Init()
        {

        }

        // =========================================== //
        // Singleton
        // =========================================== //
        static GameObjectRegistry()
        {
            Instance = new GameObjectRegistry();
        }
        public static GameObjectRegistry Instance { get; }
    }
}
