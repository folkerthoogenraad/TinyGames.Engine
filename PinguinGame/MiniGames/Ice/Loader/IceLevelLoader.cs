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
            var spawns = new List<Vector2>();

            foreach (var layer in levelIO.Layers)
            {
                if (!layer.IsObjectLayer) continue;

                foreach (var obj in layer.Objects)
                {
                    if (obj.Type != "IceBlock") continue;
                    if (obj.GetBoolProperty("Disabled", false)) continue;

                    var position = new Vector2(obj.X, obj.Y);
                    var polygon = new Polygon(obj.Polygon.Select(x => position + x.ToVector2() - center).ToArray());

                    if (polygon.IsCounterClockwise()) polygon.Reverse();

                    var block = new IceBlock(polygon);

                    block.Behaviour = obj.GetStringProperty("Behaviour", "None");
                    block.TimerOffset = obj.GetFloatProperty("TimerOffset", 0);
                    block.TimerTrigger = obj.GetFloatProperty("TimerTrigger", 0);
                    block.TimerCycleDuration = obj.GetFloatProperty("TimerCycleDuration", 0);

                    block.DriftDirection.X = obj.GetFloatProperty("DriftDirectionX", 0);
                    block.DriftDirection.Y = obj.GetFloatProperty("DriftDirectionY", 0);

                    blocks.Add(block);
                }
                foreach (var obj in layer.Objects.Where(x => x.Type == "Spawn"))
                {
                    spawns.Add(obj.Position - center);
                }
            }

            level.Blocks = blocks.ToArray();
            level.Spawns = spawns.ToArray();

            return level;
            

        }
    }
}
