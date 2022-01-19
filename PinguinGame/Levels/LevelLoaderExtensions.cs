using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PinguinGame.Levels.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;

namespace PinguinGame.Levels
{
    public static class LevelLoaderExtensions
    {
        public static TiledLevel LoadTiledLevel(this ContentManager manager, string asset)
        {
            return new LevelLoader(manager, asset).Level;
        }
    }
}
