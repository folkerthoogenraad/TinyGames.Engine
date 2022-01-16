using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PinguinGame.Pinguins.Levels.Loader.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using TinyGames.Engine.Maths;

namespace PinguinGame.Pinguins.Levels.Loader
{
    public static class LevelLoader
    {
        public static Level LoadLevel(this ContentManager manager, string asset)
        {
            using (var reader = new StreamReader(TitleContainer.OpenStream(manager.RootDirectory + Path.DirectorySeparatorChar + asset + ".json")))
            {
                string input = reader.ReadToEnd();

                var levelIO = JsonSerializer.Deserialize<TileMapLevel>(input, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                });

                var level = new Level();

                var levelSize = new Vector2(levelIO.Width * levelIO.TileWidth, levelIO.Height * levelIO.TileHeight);

                var center = levelSize / 2;

                var blocks = new List<IceBlock>();


                foreach(var layer in levelIO.Layers)
                {
                    if (layer.Type != TileMapLayer.LayerTypeObjectGroup) continue;

                    foreach(var obj in layer.Objects){
                        if (obj.Type != "IceBlock") continue;

                        var position = new Vector2(obj.X, obj.Y);
                        var polygon = new Polygon(obj.Polygon.Select(x => position + x.ToVector2() - center).ToArray());

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
}
