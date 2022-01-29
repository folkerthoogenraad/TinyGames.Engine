using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace PinguinGame.MiniGames.Ice
{
    public class IceLevelGraphics
    {
        public RenderTarget2D RenderTarget { get; set; }
        public Graphics2D Graphics { get; set; }
        public IceLevelGraphicsSettings Settings { get; set; }

        public IceLevelGraphics(GraphicsDevice device, int width, int height, IceLevelGraphicsSettings settings)
        {
            RenderTarget = new RenderTarget2D(
                device,
                width,
                height,
                false,
                SurfaceFormat.Color,
                DepthFormat.Depth24);

            Graphics = new Graphics2D(device);
            Graphics.SetRenderTarget(RenderTarget);
            Settings = settings;
        }

        public void Draw(Camera camera, IceLevel level)
        {
            Draw(camera.GetHeightDepthMatrix(), level);
        }

        public void Draw(Matrix matrix, IceLevel level)
        {
            Graphics.Begin(matrix);
            Graphics.Clear(Color.Transparent);

            DrawShadows(level);
            DrawUnderWater(level);
            DrawSnow(level);
            DrawWaterOutline(level);

            Graphics.End();
        }

        public void DrawShadows(IceLevel level)
        {
            var vertex = new VertexPositionColorTexture()
            {
                Color = Settings.SideColor,
                TextureCoordinate = new Vector2(0, 0),
            };

            foreach (var block in level.Blocks)
            {
                var list = new List<VertexPositionColorTexture>();

                if (block.Height < 0) continue;

                foreach (var line in block.Polygon.Lines)
                {
                    vertex.Position = GetVector3(line.From, block.Height);
                    list.Add(vertex);
                    vertex.Position = GetVector3(line.To, block.Height);
                    list.Add(vertex);
                    vertex.Position = GetVector3(line.From, 0);
                    list.Add(vertex);

                    vertex.Position = GetVector3(line.From, 0);
                    list.Add(vertex);
                    vertex.Position = GetVector3(line.To, block.Height);
                    list.Add(vertex);
                    vertex.Position = GetVector3(line.To, 0);
                    list.Add(vertex);
                }
                Graphics.DrawRaw(list);
            }
        }
        public void DrawUnderWater(IceLevel level)
        {
            var vertex = new VertexPositionColorTexture()
            {
                Color = Settings.SideWaterColor,
                TextureCoordinate = new Vector2(0, 0),
            };

            foreach (var block in level.Blocks)
            {
                var list = new List<VertexPositionColorTexture>();

                if (block.Height < -8) continue;

                var start = MathF.Min(0, block.Height);

                foreach (var line in block.Polygon.Lines)
                {
                    vertex.Position = GetVector3(line.From, start);
                    list.Add(vertex);
                    vertex.Position = GetVector3(line.To, start);
                    list.Add(vertex);
                    vertex.Position = GetVector3(line.From, -8);
                    list.Add(vertex);

                    vertex.Position = GetVector3(line.From, -8);
                    list.Add(vertex);
                    vertex.Position = GetVector3(line.To, start);
                    list.Add(vertex);
                    vertex.Position = GetVector3(line.To, -8);
                    list.Add(vertex);
                }
                Graphics.DrawRaw(list);
            }
        }
        public void DrawSnow(IceLevel level)
        {
            var vertex = new VertexPositionColorTexture()
            {
                Color = Settings.SnowColor,
                TextureCoordinate = new Vector2(0, 0),
            };

            foreach (var block in level.Blocks)
            {
                if (block.Height <= -8) continue;

                if(block.Height < 0)
                {
                    vertex.Color = Settings.SideWaterColor;
                    
                }
                else
                {
                    if (block.Highlighted)
                    {
                        vertex.Color = Settings.SnowColorHighlighted;
                    }
                    else
                    {
                        vertex.Color = Settings.SnowColor;
                    }
                }

                var list = new List<VertexPositionColorTexture>();

                foreach (var triangle in block.Polygon.Triangles)
                {
                    vertex.Position = GetVector3(triangle.A, block.Height);
                    list.Add(vertex);
                    vertex.Position = GetVector3(triangle.B, block.Height);
                    list.Add(vertex);
                    vertex.Position = GetVector3(triangle.C, block.Height);
                    list.Add(vertex);
                }
                Graphics.DrawRaw(list);
            }
        }
        public void DrawWaterOutline(IceLevel level)
        {
            foreach (var block in level.Blocks)
            {
                if (block.Height < 0) continue;

                foreach (var line in block.Polygon.Lines)
                {
                    Graphics.DrawLine(line.From, line.To, 4, 0, Color.White);
                }
            }
        }

        public Vector3 GetVector3(Vector2 v, float h)
        {
            return new Vector3(v, h);
        }
    }
}
