using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PinguinGame.Levels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using TinyGames.Engine.Maths;

namespace PinguinGame.MiniGames.Ice.CharacterStates
{
    public static class IceLevelLoader
    {
        public static IceLevel LoadIceLevel(this ContentManager manager, string asset)
        {
            var levelIO = manager.LoadTiledLevel(asset);

            var level = new IceLevel();

            var levelSize = new Vector2(levelIO.Width * levelIO.TileWidth, levelIO.Height * levelIO.TileHeight);

            var center = levelSize / 2;

            var blocks = new List<IceBlock>();


            foreach(var layer in levelIO.Layers)
            {
                if (!layer.IsObjectLayer) continue;

                foreach(var obj in layer.Objects){
                    if (obj.Type != "IceBlock") continue;
                    if (obj.GetBoolProperty("Disabled", false)) continue;

                    var position = new Vector2(obj.X, obj.Y);
                    var polygon = new Polygon(obj.Polygon.Select(x => position + x.ToVector2() - center).ToArray());

                    if (polygon.IsCounterClockwise()) polygon.Reverse();

                    var block = new IceBlock(polygon);

                    block.Sinkable = obj.GetBoolProperty("Sinkable", true);

                    blocks.Add(block);
                }
            }

            level.Blocks = blocks.ToArray();

            return level;
            

        }
    }
}
