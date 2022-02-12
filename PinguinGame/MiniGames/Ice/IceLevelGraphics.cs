using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;
using PinguinGame.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PinguinGame.MiniGames.Ice
{
    public class IceBlockSubdividedSprite
    {
        public struct Division
        {
            public float SidePositionFrom;
            public float SidePositionTo;
            public float DepthOffsetFrom;
            public float DepthOffsetTo;
        }

        public Sprite Sprite;
        public List<Division> Divisions;
    }

    public class IceLevelGraphics : IDisposable
    {
        public GraphicsDevice Device { get; set; }
        public List<RenderTarget2D> RenderTargets { get; set; }
        public IceLevelGraphicsSettings Settings { get; set; }
        public IceLevel Level { get; set; }

        private Dictionary<IceBlock, Sprite> _topSprites;
        private Dictionary<IceBlock, IceBlockSubdividedSprite> _sideSprites;

        public Texture2D SnowTexture { get; set; }
        public Effect Effect { get; set; }

        public IceLevelGraphics(ContentManager manager, GraphicsDevice device, IceLevel level, IceLevelGraphicsSettings settings)
        {
            Device = device;
            Level = level;
            Settings = settings;

            _topSprites = new Dictionary<IceBlock, Sprite>();
            _sideSprites = new Dictionary<IceBlock, IceBlockSubdividedSprite>();

            RenderTargets = new List<RenderTarget2D>();

            Effect = manager.Load<Effect>("Effects/WaterEdgeEffect");
            SnowTexture = manager.Load<Texture2D>("Sprites/Ice/SnowTexture");

            Effect.Parameters["AboveWaterColor"].SetValue(Settings.SideColor.ToVector4());
            Effect.Parameters["BelowWaterColor"].SetValue(Settings.SideWaterColor.ToVector4());

            SetupDrawNormalSprites();
            SetupDrawSideParts();
        }

        public void SetupDrawNormalSprites()
        {
            int width = 512;
            int height = 512;

            var target = new RenderTarget2D(
                Device,
                width,
                height,
                false,
                SurfaceFormat.Color,
                DepthFormat.Depth24);

            RenderTargets.Add(target);

            var camera = new Camera(target.Width, target.Width / ((float)target.Height));
            camera.Position += new Vector2(target.Width / 2, target.Height / 2);

            using (var graphics = new Graphics2D(Device))
            {
                graphics.SetRenderTarget(target);

                graphics.Begin(camera.GetMatrix());
                graphics.Clear(Color.Transparent);

                foreach (var (block, rectangle) in Level.Blocks.PackRectangles(width, height, block => new Point(
                        (int)Math.Ceiling(block.LocalPolygon.GetBounds().Width),
                        (int)Math.Ceiling(block.LocalPolygon.GetBounds().Height)),
                        (block, rect) => (block, rect)))
                {
                    DrawBlockInRectangle(graphics, block, rectangle);

                    // TODO this should be better but w/e
                    _topSprites.Add(block, new Sprite(target, rectangle));
                }

                graphics.End();
            }
        }

        // This part is _really_ inefficient but it works.
        public void SetupDrawSideParts()
        {
            int width = 512;
            int height = 512;

            // This can be a quick "CreateAndRenderToTexture" routine,
            // That creates a new render texture, returns a Texture2D,
            // gives you a graphics object in callback, disposes
            // the graphics object afterwards and you can draw whatever
            // you like in texture space.
            var target = new RenderTarget2D(
                Device,
                width,
                height,
                false,
                SurfaceFormat.Color,
                DepthFormat.Depth24);

            RenderTargets.Add(target);

            var camera = new Camera(target.Width, target.Width / ((float)target.Height));
            camera.Position += new Vector2(target.Width / 2, target.Height / 2);

            using (var graphics = new Graphics2D(Device))
            {
                graphics.SetRenderTarget(target);

                graphics.Begin(camera.GetMatrix());
                graphics.Clear(Color.Transparent);

                foreach (var (block, rectangle) in Level.Blocks.PackRectangles(width, height, block => new Point(
                        (int)Math.Ceiling(block.LocalPolygon.GetBounds().Width),
                        (int)Math.Ceiling(block.LocalPolygon.GetBounds().Height + 16)),
                        (block, rect) => (block, rect)))
                {
                    var list = DrawSideInRectangle(graphics, block, rectangle);

                    _sideSprites.Add(block, new IceBlockSubdividedSprite() { 
                        Sprite = new Sprite(target, rectangle),
                        Divisions = list,
                    });
                }

                graphics.End();
            }
        }

        private void DrawBlockInRectangle(Graphics2D graphics, IceBlock block, Rectangle rectangle)
        {

            AABB bounds = block.LocalPolygon.GetBounds();
            Vector2 offset = -bounds.TopLeft + new Vector2(rectangle.X, rectangle.Y);

            // Debug outlines :)
            //Graphics.DrawRectangle(AABB.Create(rectangle), Color.Blue);
            //Graphics.DrawRectangle(AABB.Create(rectangle).Shrink(2), Color.Red);

            foreach (var triangle in block.LocalPolygon.Triangles)
            {
                float uvScale = 1 / 16.0f;

                Vector2 a = triangle.A + offset;
                Vector2 b = triangle.B + offset;
                Vector2 c = triangle.C + offset;

                graphics.DrawTexturedTriangle(SnowTexture, a, b, c, a * uvScale, b * uvScale, c * uvScale, Color.White);
                // graphics.DrawTexturedTriangle(SnowTexture, a, b, c, new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), Color.White);
            }
        }

        private List<IceBlockSubdividedSprite.Division> DrawSideInRectangle(Graphics2D renderer, IceBlock block, Rectangle rectangle)
        {
            var list = new List<IceBlockSubdividedSprite.Division>();

            AABB bounds = block.LocalPolygon.GetBounds();
            Vector2 offset = -bounds.TopLeft + new Vector2(rectangle.X, rectangle.Y);

            // Debug outlines :)
            //renderer.DrawRectangle(AABB.Create(rectangle), Color.Blue);
            //renderer.DrawRectangle(AABB.Create(rectangle).Shrink(2), Color.Red);

            foreach (var line in block.LocalPolygon.Lines)
            {
                // Only draw lines facing towards us
                // This is basically the _cheapest_ dot product with
                // Vector2.Right
                if (line.Direction.X > 0) continue;

                list.Add(new IceBlockSubdividedSprite.Division() { 
                    DepthOffsetFrom = (line.From - bounds.TopLeft).Y,
                    DepthOffsetTo = (line.To - bounds.TopLeft).Y,
                    SidePositionFrom = line.From.X - bounds.TopLeft.X,
                    SidePositionTo = line.To.X - bounds.TopLeft.X
                });

                Vector2 heightOffset = new Vector2(0, 16);

                renderer.DrawQuadWithColors(
                    line.From + offset, 
                    line.To + offset, 
                    line.To + heightOffset + offset, 
                    line.From + heightOffset + offset, 
                    Color.White,
                    Color.White,
                    Color.Black,
                    Color.Black,
                    GraphicsHelper.YToDepth(line.From + offset), 
                    GraphicsHelper.YToDepth(line.To + offset),
                    GraphicsHelper.YToDepth(line.To + offset), 
                    GraphicsHelper.YToDepth(line.From + offset));
            }

            return list;
        }

        public void DrawWorld(Graphics2D target)
        {
            // Draw sides
            target.SetShader(Effect);
            
            foreach (var block in Level.Blocks)
            {
                target.Flush();

                AABB bounds = block.LocalPolygon.GetBounds();
                Vector2 offset = bounds.TopLeft + block.Position;

                float height = block.Height;

                var side = _sideSprites[block];
                Sprite sprite = side.Sprite;

                float textureWidth = sprite.Texture.Width;
                float textureHeight = sprite.Texture.Height;

                Effect.Parameters["WaterLine"].SetValue(1 - height / 16);

                foreach (var division in side.Divisions)
                {
                    float uvTop = sprite.SourceRectangle.Top / textureHeight;
                    float uvBottom = sprite.SourceRectangle.Bottom / textureHeight;
                    float uvLeft = (sprite.SourceRectangle.Left + division.SidePositionFrom) / textureWidth;
                    float uvRight = (sprite.SourceRectangle.Left + division.SidePositionTo) / textureWidth;

                    // With texture mapping
                    target.DrawQuadWithColorsFull(
                        offset + new Vector2(division.SidePositionFrom, -height),
                        offset + new Vector2(division.SidePositionTo, -height),
                        offset + new Vector2(division.SidePositionTo, sprite.Height- height),
                        offset + new Vector2(division.SidePositionFrom, sprite.Height- height),
                        Color.White, Color.White, Color.White, Color.White,
                        new Vector2(uvLeft, uvTop),
                        new Vector2(uvRight, uvTop),
                        new Vector2(uvRight, uvBottom),
                        new Vector2(uvLeft, uvBottom),
                        GraphicsHelper.YToDepth(offset.Y + division.DepthOffsetFrom - 1), // small depth offset to prevent walls clipping through floors :)
                        GraphicsHelper.YToDepth(offset.Y + division.DepthOffsetTo - 1),
                        GraphicsHelper.YToDepth(offset.Y + division.DepthOffsetTo - 1),
                        GraphicsHelper.YToDepth(offset.Y + division.DepthOffsetFrom - 1),
                        sprite.Texture);
                }

            }

            target.ResetShader();

            // Draw top
            foreach (var block in Level.Blocks)
            {
                AABB bounds = block.LocalPolygon.GetBounds();
                Vector2 offset = bounds.TopLeft + block.Position;

                float height = block.Height;

                Sprite sprite = _topSprites[block];

                var color = Settings.SnowColor;

                if (block.Highlighted) color = Settings.SnowColorHighlighted;
                if (block.Height < 0) color = Settings.SideWaterColor;

                target.DrawSpriteWithDepths(sprite, 
                    offset - new Vector2(0, height), 
                    GraphicsHelper.YToDepth(offset.Y), 
                    GraphicsHelper.YToDepth(offset.Y + sprite.Height), color);
            }
        }

        public void Dispose()
        {
            foreach(var target in RenderTargets)
            {
                target.Dispose();
            }
        }

#if false
        public void Draw(Camera camera)
        {
            Draw(camera.GetHeightDepthMatrix());
        }

        public void Draw(Matrix matrix)
        {
            //Graphics.Begin(matrix);
            //Graphics.Clear(Color.Transparent);

            //DrawShadows(level);
            //DrawUnderWater(level);
            //DrawSnow(level);
            //DrawWaterOutline(level);

            //Graphics.End();
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
#endif
    }
}
