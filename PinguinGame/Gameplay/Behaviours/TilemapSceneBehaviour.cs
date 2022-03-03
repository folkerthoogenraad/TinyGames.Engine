using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Scenes;
using TinyGames.Engine.Scenes.Extensions;

namespace PinguinGame.Gameplay.Behaviours
{
    [RequireSceneBehaviour(typeof(GraphicsSceneBehaviour))]
    public class TilemapSceneBehaviour : SceneBehaviour, IDrawable2D, IWalkable
    {
        public Tilemap Map { get; set; }

        private GraphicsSceneBehaviour _graphics;
        private WalkablesSceneBehaviour _walkables;

        public TilemapSceneBehaviour(Tilemap map)
        {
            Map = map;
        }

        public override void Init(Scene scene)
        {
            base.Init(scene);

            _graphics = scene.GetSceneGraphics();
            _walkables = scene.GetWalkables();

            _graphics.AddManualDrawable(this);
            _walkables.AddManualWalkable(this);
        }

        public override void Destroy()
        {
            base.Destroy();

            _graphics.RemoveManualDrawable(this);
            _walkables.RemoveManualWalkable(this);
        }

        public void Draw(Graphics2D graphics)
        {
            foreach(var layer in Map.Layers)
            {
                DrawLayer(graphics, layer);
            }
        }

        public void DrawLayer(Graphics2D graphics, TilemapLayer layer)
        {
            for (int i = 0; i < Map.Width; i++)
            {
                for (int j = 0; j < Map.Height; j++)
                {
                    var sprite = layer.GetSprite(i, j);

                    if (sprite == null) continue;

                    graphics.DrawSprite(sprite, new Vector2(i * Map.TileWidth, j * Map.TileHeight));
                }
            }
        }

        public GroundInfo GetGroundInfo(Vector2 point)
        {
            if (Map.Layers.Length > 0)
            {
                return GroundInfo.Solid(GroundMaterial.Wood);
            }
            else return GroundInfo.Empty();
        }
    }
}
