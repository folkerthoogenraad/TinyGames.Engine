using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;

namespace PinguinGame.MiniGames.Minecarts
{
    public class MinecartLevelGraphics
    {
        public RenderTarget2D RenderTarget { get; set; }
        public Graphics2D Graphics { get; set; }
        public MinecartLevelGraphicsSettings Settings { get; set; }

        public MinecartLevelGraphics(GraphicsDevice device, MinecartLevelGraphicsSettings settings, RenderTarget2D target = null)
        {
            RenderTarget = target;
            Settings = settings;
            Graphics = new Graphics2D(device);
            Graphics.SetRenderTarget(RenderTarget);
        }

        public void Draw(Camera camera, MinecartLevel level)
        {
            Draw(camera.GetMatrix(), camera.Bounds, level);
        }

        public void Draw(Matrix matrix, AABB bounds, MinecartLevel level)
        {
            Graphics.Begin(matrix);
            Graphics.Clear(Settings.BackgroundColor);

            DrawTilemap(level.Tilemap, bounds);

            Graphics.End();
        }

        private void DrawTilemap(Tilemap tilemap, AABB bounds)
        {
            foreach(var layer in tilemap.Layers)
            {
                DrawTilemapLayer(tilemap, layer, bounds);
            }
        }

        private void DrawTilemapLayer(Tilemap map, TilemapLayer layer, AABB bounds)
        {
            for(int i = 0; i < map.Width; i++)
            {
                for(int j = 0; j < map.Height; j++)
                {
                    var sprite = layer.GetSprite(i, j);

                    if (sprite == null) continue;

                    Graphics.DrawSprite(sprite, new Vector2(i * map.TileWidth, j * map.TileHeight));
                }
            }
        }
    }
}
