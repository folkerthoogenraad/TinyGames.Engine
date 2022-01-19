using Microsoft.Xna.Framework.Content;
using PinguinGame.Levels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PinguinGame.MiniGames.Minecarts.Loader
{
    public static class MinecartLevelLoader
    {
        public static MinecartLevel LoadMinecartLevel(this ContentManager manager, string asset)
        {
            // Open level loader
            LevelLoader loader = new LevelLoader(manager, asset);
            var level = loader.Level;

            // Load layers
            var layers = level.Layers.Select(layer =>
            {
                if (!layer.IsTileLayer) return null;

                var sprites = layer.Data.Select(tile => loader.GetSpriteForTile(tile)).ToArray();
                var tilemapLayer = new TilemapLayer(sprites, layer.Width, layer.Height) { 
                    Name = layer.Name,
                };

                return tilemapLayer;
            }).Where(layer => layer != null).ToArray();

            // Load spawns
            var spawns = level.Layers.SelectMany(x => {
                if (!x.IsObjectLayer) return Enumerable.Empty<MinecartSpawn>();

                return x.Objects
                    .Where(obj => obj.Type == "Spawn")
                    .Select(obj => new MinecartSpawn(obj.GetIntProperty("Index"), obj.Center));
            }).ToArray();

            // Create tilemap
            var tilemap = new Tilemap(layers, level.Width, level.Height, level.TileWidth, level.TileHeight);

            // Return level
            return new MinecartLevel() {
                Tilemap = tilemap,
                Spawns = spawns
            };
        }
    }
}
