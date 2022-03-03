using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PinguinGame.Gameplay.Behaviours;
using PinguinGame.Gameplay.GameObjects;
using PinguinGame.Graphics;
using PinguinGame.Levels;
using PinguinGame.Levels.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using TinyGames.Engine.Collections;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;
using TinyGames.Engine.Scenes;
using TinyGames.Engine.Scenes.Extensions;

namespace PinguinGame.Gameplay.CharacterStates
{
    public static class IceLevelLoader
    {
        public static Scene LoadScene(this ContentManager content, string asset)
        {
            var services = content.ServiceProvider;

            var tiledLevelLoader = new TiledLevelLoader(content, asset);
            var level = tiledLevelLoader.Level;
            var register = new TiledSceneLoaderRegister();
            var loader = new TiledSceneLoader(level, register);

            // Load the tilemap
            var map = new Tilemap(level.Layers.Select(layer =>
            {
                if (!layer.IsTileLayer) return null;

                var sprites = layer.Data.Select(tile => tiledLevelLoader.GetSpriteForTile(tile)).ToArray();
                var tilemapLayer = new TilemapLayer(sprites, layer.Width, layer.Height)
                {
                    Name = layer.Name,
                };

                return tilemapLayer;
            }).Where(layer => layer != null).ToArray(), level.Width, level.Height, level.TileWidth, level.TileHeight);


            register.OnInit = (level, scene) => {
                var graphicsService = services.GetService<IGraphicsService>();

                var graphics = scene.AddBehaviour(new GraphicsSceneBehaviour(graphicsService));
                scene.AddBehaviour(new IceBlockGraphicsSceneBehaviour(content, graphicsService.Device));
                scene.AddBehaviour(new ParticleSystem());
                scene.AddBehaviour(new WalkablesSceneBehaviour());
                scene.AddBehaviour(new IceGameEffects(content));
                scene.AddBehaviour(new IceGameUIGraphics(content));
                scene.AddBehaviour(new IceGameGraphics(content));
                scene.AddBehaviour(new CharacterCollisionBehaviour());
                scene.AddBehaviour(new CharacterIndicatorBehaviour());
                scene.AddBehaviour(new TilemapSceneBehaviour(map));

                // Setup camera
                graphics.Camera.Position = new Vector2(level.Width * level.TileWidth, level.Height * level.TileHeight) / 2;

                if(level.BackgroundColor != null) graphics.BackgroundColor = GraphicsUtils.ParseColor(level.BackgroundColor);
            };

            register.RegisterObjectType("IceBlock", obj => {
                if (obj.Properties.GetBoolProperty("Disabled", false)) return null;

                var position = new Vector2(obj.X, obj.Y);
                var polygon = new Polygon(obj.Polygon.Select(x => position + x.ToVector2()).ToArray());

                if (polygon.IsCounterClockwise()) polygon.Reverse();

                var block = new IceBlockGameObject(polygon);

                block.Behaviour = obj.Properties.GetStringProperty("Behaviour", "None");
                block.TimerOffset = obj.Properties.GetFloatProperty("TimerOffset", 0);
                block.TimerTrigger = obj.Properties.GetFloatProperty("TimerTrigger", 0);
                block.TimerCycleDuration = obj.Properties.GetFloatProperty("TimerCycleDuration", 0);

                block.DriftDirection.X = obj.Properties.GetFloatProperty("DriftDirectionX", 0);
                block.DriftDirection.Y = obj.Properties.GetFloatProperty("DriftDirectionY", 0);

                return block;
            });
            register.RegisterObjectType("Spawn", obj => new SpawnGameObject(obj.Position));
            register.RegisterObjectType("Bridge", obj => new BridgeGameObject(obj.Position, obj.Size));
            register.RegisterObjectType("Wall", obj => new WallGameObject(obj.Position, obj.Size));
            register.RegisterObjectType("Geyser", obj => new GeyserGameObject(obj.Position));
            register.RegisterObjectType("Grass", obj => new GrassGameObject(obj.Position));
            register.RegisterObjectType("Tree", obj => new TreeGameObject(obj.Position));
            register.RegisterObjectType("Water", obj => new WaterGameObject());
            register.RegisterObjectType("ShoppingCart", obj => new ShoppingCartGameObject(content, obj.Position));

            return loader.Load(content.ServiceProvider);
        }
    }
}
