using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TinyGames.Engine.Maths;

namespace TinyGames.Engine.Graphics
{

    public class Graphics2D : IDisposable
    {
        public GraphicsDevice Device { get; set; }

        private VertexPositionColorTexture[] Vertices;

        private Effect CurrentEffect = null;
        private Effect DefaultEffect = null;

        private Texture2D Pixel;
        private RenderTarget2D RenderTarget;

        private DepthStencilState DefaultDepthStencilState;
        private BlendState DefaultBlendState;

        private Matrix ProjectionMatrix;

        private Stack<float> AlphaStack;
        private float Alpha = 1;

        private Stack<Matrix> TransformStack;
        private Matrix Matrix;

        private Texture2D[] Textures;

        public bool Drawing { get; private set; }

        private int VertexIndex = 0;

        public Graphics2D(GraphicsDevice device): this(device, new AlphaTestEffect(device)) { }
        public Graphics2D(GraphicsDevice device, Effect defaultEffect)
        {
            Device = device;
            DefaultEffect = defaultEffect;

            Vertices = new VertexPositionColorTexture[1024];
            
            Textures = new Texture2D[16];

            CurrentEffect = defaultEffect;

            Pixel = new Texture2D(device, 1, 1);
            Pixel.SetData(new Color[] { Color.White });

            TransformStack = new Stack<Matrix>();
            AlphaStack= new Stack<float>();
            
            DefaultDepthStencilState = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.Always,
                StencilPass = StencilOperation.Replace,
                ReferenceStencil = 1,
                DepthBufferEnable = true,
            };


            DefaultBlendState = new BlendState()
            {
                AlphaBlendFunction = BlendFunction.Add,
                AlphaSourceBlend = Blend.One,
                AlphaDestinationBlend = Blend.InverseSourceAlpha,

                ColorBlendFunction = BlendFunction.Add,
                ColorSourceBlend = Blend.SourceAlpha,
                ColorDestinationBlend = Blend.InverseSourceAlpha
            };
        }

        public void DrawSprite(Sprite sprite, Vector2 position, Vector2 scale, float angle = 0, float depth = 0)
        {
            DrawSprite(sprite, position, scale, angle, depth, Color.White);
        }
        
        public void DrawSprite(Sprite sprite, Vector2 position, Vector2 scale, float angle, float depth, Color blend)
        {
            if (!Drawing) return;
            if (VertexIndex >= Vertices.Length - 6) Flush();

            SetTexture(sprite.Texture);

            float s = 0;
            float c = 1;

            if (angle != 0)
            {
                s = MathF.Sin(angle * Tools.DegToRad);
                c = MathF.Cos(angle * Tools.DegToRad);
            }

            // Basically create a matrix if you will
            Vector3 right = new Vector3(c, s, 0);
            Vector3 up = new Vector3(s, -c, 0);

            float l = (-sprite.Origin.X) * scale.X;
            float r = (-sprite.Origin.X + sprite.Width) * scale.X;

            float t = (-sprite.Origin.Y) * scale.Y;
            float b = (-sprite.Origin.Y + sprite.Height) * scale.Y;

            Vector3 tl = new Vector3(position.X, position.Y, depth) + l * right - t * up;
            Vector3 tr = new Vector3(position.X, position.Y, depth) + r * right - t * up;
            Vector3 bl = new Vector3(position.X, position.Y, depth) + l * right - b * up;
            Vector3 br = new Vector3(position.X, position.Y, depth) + r * right - b * up;

            float tWidth = sprite.Texture.Width;
            float tHeight = sprite.Texture.Height;

            Rectangle rect = sprite.SourceRectangle;

            Vector2 uvTL = new Vector2(rect.Left / tWidth, rect.Top / tHeight);
            Vector2 uvTR = new Vector2(rect.Right / tWidth, rect.Top / tHeight);
            Vector2 uvBL = new Vector2(rect.Left / tWidth, rect.Bottom / tHeight);
            Vector2 uvBR = new Vector2(rect.Right / tWidth, rect.Bottom / tHeight);

            // Clockwise
            // TL--- TR
            // | \   |
            // |   \ |
            // BL--- BR

            Vertex(tl, uvTL, blend);
            Vertex(tr, uvTR, blend);
            Vertex(br, uvBR, blend);

            Vertex(tl, uvTL, blend);
            Vertex(br, uvBR, blend);
            Vertex(bl, uvBL, blend);
        }

        public void DrawSprite(Sprite sprite, Vector2 position, float angle, float depth, Color blend)
        {
            DrawSprite(sprite, position, Vector2.One, angle, depth, blend);
        }

        public void DrawSprite(Sprite sprite, Vector2 position, float angle = 0, float depth = 0)
        {
            DrawSprite(sprite, position, Vector2.One, angle, depth, Color.White);
        }

        public void DrawTexture(Texture2D texture, Vector2 position, Vector2 size, float depth = 0)
        {
            if (VertexIndex >= Vertices.Length - 6) Flush();

            SetTexture(texture);


            // Clockwise
            // TL--- TR
            // | \   |
            // |   \ |
            // BL--- BR

            Vector3 pos = new Vector3(position, depth);

            Vertex(pos + new Vector3(0, 0, 0), new Vector2(0, 0), Color.White);
            Vertex(pos + new Vector3(size.X, 0, 0), new Vector2(1, 0), Color.White);
            Vertex(pos + new Vector3(size.X, size.Y, 0), new Vector2(1, 1), Color.White);

            Vertex(pos + new Vector3(0, 0, 0), new Vector2(0, 0), Color.White);
            Vertex(pos + new Vector3(size.X, size.Y, 0), new Vector2(1, 1), Color.White);
            Vertex(pos + new Vector3(0, size.Y, 0), new Vector2(0, 1), Color.White);
        }

        public void DrawTextureRegion(Texture2D texture, Rectangle sourceRectangle, Vector2 position, Vector2 size, float depth = 0)
        {
            DrawTextureRegion(texture, sourceRectangle, position, size, Color.White, depth);
        }
        public void DrawTextureRegion(Texture2D texture, Rectangle sourceRectangle, Vector2 position, Vector2 size, Color blend, float depth = 0)
        {
            DrawTextureRegion(texture, AABB.Create(sourceRectangle), position, size, blend, depth);
        }
        public void DrawTextureRegion(Texture2D texture, AABB sourceRectangle, Vector2 position, Vector2 size, Color blend, float depth = 0)
        {
            if (VertexIndex >= Vertices.Length - 6) Flush();

            SetTexture(texture);

            float u0 = sourceRectangle.Left / texture.Width;
            float u1 = sourceRectangle.Right / texture.Width;
            float v0 = sourceRectangle.Top / texture.Height;
            float v1 = sourceRectangle.Bottom / texture.Height;

            // Clockwise
            // TL--- TR
            // | \   |
            // |   \ |
            // BL--- BR

            Vector3 pos = new Vector3(position, depth);

            Vertex(pos + new Vector3(0, 0, 0), new Vector2(u0, v0), blend);
            Vertex(pos + new Vector3(size.X, 0, 0), new Vector2(u1, v0), blend);
            Vertex(pos + new Vector3(size.X, size.Y, 0), new Vector2(u1, v1), blend);

            Vertex(pos + new Vector3(0, 0, 0), new Vector2(u0, v0), blend);
            Vertex(pos + new Vector3(size.X, size.Y, 0), new Vector2(u1, v1), blend);
            Vertex(pos + new Vector3(0, size.Y, 0), new Vector2(u0, v1), blend);
        }

        public void DrawLine(Vector2 start, Vector2 end, float width, float depth, Color blend)
        {
            if (VertexIndex >= Vertices.Length - 6) Flush();

            Vector3 a = new Vector3(start, depth);
            Vector3 b = new Vector3(end, depth);

            Vector3 dir = (b - a);
            dir.Normalize();


            Vector3 perp = new Vector3(dir.Y, -dir.X, 0);

            SetPixelTexture();
            Vector2 uv = new Vector2(0, 0);

            float w = width / 2;

            // Clockwise
            // TL--- TR
            // | \   |
            // |   \ |
            // BL--- BR

            Vertex(a - perp * w, uv, blend);
            Vertex(a + perp * w, uv, blend);
            Vertex(b + perp * w, uv, blend);

            Vertex(a - perp * w, uv, blend);
            Vertex(b + perp * w, uv, blend);
            Vertex(b - perp * w, uv, blend);
        }

        public void DrawRectangle(Vector2 position, Vector2 size, Color color, float depth = 0)
        {
            if (VertexIndex >= Vertices.Length - 6) Flush();

            SetPixelTexture();

            Vector3 tl = new Vector3(position.X, position.Y, depth);
            Vector3 tr = new Vector3(position.X + size.X, position.Y, depth);
            Vector3 bl = new Vector3(position.X, position.Y + size.Y, depth);
            Vector3 br = new Vector3(position.X + size.X, position.Y + size.Y, depth);

            Vertex(tl, Vector2.Zero, color);
            Vertex(tr, Vector2.Zero, color);
            Vertex(br, Vector2.Zero, color);

            Vertex(tl, Vector2.Zero, color);
            Vertex(br, Vector2.Zero, color);
            Vertex(bl, Vector2.Zero, color);
        }

        public void DrawRectangle(Rectangle rect, Color color, float depth = 0)
        {
            DrawRectangle(new Vector2(rect.X, rect.Y), new Vector2(rect.Width, rect.Height), color, depth);
        }
        public void DrawRectangle(float x, float y, float w, float h, Color color, float depth = 0)
        {
            DrawRectangle(new Vector2(x, y), new Vector2(w, h), color, depth);
        }

        public void DrawRectangle(AABB bounds, Color color, float depth = 0)
        {
            DrawRectangle(bounds.TopLeft, bounds.Size, color, depth);
        }

        public void DrawTriangle(Vector2 a, Vector2 b, Vector2 c, Color color, float depth = 0)
        {
            if (VertexIndex >= Vertices.Length - 3) Flush();

            SetPixelTexture();

            Vertex(new Vector3(a.X, a.Y, depth), Vector2.Zero, color);
            Vertex(new Vector3(b.X, b.Y, depth), Vector2.Zero, color);
            Vertex(new Vector3(c.X, c.Y, depth), Vector2.Zero, color);
        }

        public void DrawTexturedTriangle(Texture2D texture, Vector2 a, Vector2 b, Vector2 c, Vector2 uvA, Vector2 uvB, Vector2 uvC, Color color, float depth = 0)
        {
            if (VertexIndex >= Vertices.Length - 3) Flush();

            SetTexture(texture);

            Vertex(new Vector3(a.X, a.Y, depth), uvA, color);
            Vertex(new Vector3(b.X, b.Y, depth), uvB, color);
            Vertex(new Vector3(c.X, c.Y, depth), uvC, color);
        }


        public void DrawQuad(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Color color, float depth = 0)
        {
            if (VertexIndex >= Vertices.Length - 6) Flush();

            SetPixelTexture();

            // a - b
            // | \ |
            // d - c

            Vertex(new Vector3(a.X, a.Y, depth), Vector2.Zero, color);
            Vertex(new Vector3(b.X, b.Y, depth), Vector2.Zero, color);
            Vertex(new Vector3(c.X, c.Y, depth), Vector2.Zero, color);

            Vertex(new Vector3(a.X, a.Y, depth), Vector2.Zero, color);
            Vertex(new Vector3(c.X, c.Y, depth), Vector2.Zero, color);
            Vertex(new Vector3(d.X, d.Y, depth), Vector2.Zero, color);
        }
        public void DrawQuadWithColors(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Color colorA, Color colorB, Color colorC, Color colorD, float depthA = 0, float depthB = 0, float depthC = 0, float depthD = 0)
        {
            if (VertexIndex >= Vertices.Length - 6) Flush();

            SetPixelTexture();

            // a - b
            // | \ |
            // d - c

            Vertex(new Vector3(a.X, a.Y, depthA), Vector2.Zero, colorA);
            Vertex(new Vector3(b.X, b.Y, depthB), Vector2.Zero, colorB);
            Vertex(new Vector3(c.X, c.Y, depthC), Vector2.Zero, colorC);

            Vertex(new Vector3(a.X, a.Y, depthA), Vector2.Zero, colorA);
            Vertex(new Vector3(c.X, c.Y, depthC), Vector2.Zero, colorC);
            Vertex(new Vector3(d.X, d.Y, depthD), Vector2.Zero, colorD);
        }
        public void DrawQuadWithColorsFull(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Color colorA, Color colorB, Color colorC, Color colorD, Vector2 uvA, Vector2 uvB, Vector2 uvC, Vector2 uvD, float depthA = 0, float depthB = 0, float depthC = 0, float depthD = 0, Texture2D texture = null)
        {
            if (VertexIndex >= Vertices.Length - 6) Flush();

            if(texture == null) SetPixelTexture();
            else SetTexture(texture);


            // a - b
            // | \ |
            // d - c

            Vertex(new Vector3(a.X, a.Y, depthA), uvA, colorA);
            Vertex(new Vector3(b.X, b.Y, depthB), uvB, colorB);
            Vertex(new Vector3(c.X, c.Y, depthC), uvC, colorC);

            Vertex(new Vector3(a.X, a.Y, depthA), uvA, colorA);
            Vertex(new Vector3(c.X, c.Y, depthC), uvC, colorC);
            Vertex(new Vector3(d.X, d.Y, depthD), uvD, colorD);
        }

        public void DrawCircle(Vector2 pos, float radius, Color color, float depth = 0)
        {
            const int subdivisions = 32;

            if (VertexIndex >= Vertices.Length - subdivisions * 3) Flush();

            float radiansPerSubdivision = MathF.PI * 2 / subdivisions;

            Vector3 center = new Vector3(pos.X, pos.Y, depth);

            SetPixelTexture();

            for (int i = 0; i < subdivisions; i++)
            {
                float radStart = i * radiansPerSubdivision; //bro
                float radEnd = (i + 1) * (radiansPerSubdivision);

                Vertex(center, Vector2.Zero, color);
                Vertex(center + new Vector3(MathF.Cos(radStart) * radius, MathF.Sin(radStart) * radius, depth), Vector2.Zero, color);
                Vertex(center + new Vector3(MathF.Cos(radEnd) * radius, MathF.Sin(radEnd) * radius, depth), Vector2.Zero, color);
            }
        }

        public void Clear(Color color, bool all = true)
        {
            Flush();

            if(all)
            {
                Device.Clear(color);
            }
            else
            {
                Device.Clear(ClearOptions.DepthBuffer, Color.Transparent, Device.Viewport.MaxDepth, 0);
            }
        }

        public void ClearDepthBuffer()
        {
            Flush();

            Device.Clear(ClearOptions.DepthBuffer, Color.Transparent, Device.Viewport.MaxDepth, 0);
        }

        public void Begin(Matrix m)
        {
            Drawing = true;
            VertexIndex = 0;

            TransformStack.Clear();
            Matrix = Matrix.Identity;
            ProjectionMatrix = m; //Matrix.CreateOrthographicOffCenter(0, Device.PresentationParameters.BackBufferWidth, Device.PresentationParameters.BackBufferHeight, 0, -100, 100);
        }

        public void Push()
        {
            Flush();
            TransformStack.Push(Matrix);
            AlphaStack.Push(Alpha);
        }

        public void Translate(Vector2 v)
        {
            Translate(v.X, v.Y);
        }

        public void Translate(float x, float y)
        {
            Flush();
            Matrix = Matrix.CreateTranslation(x, y, 0) * Matrix;
        }

        public void Scale(Vector2 v)
        {
            Scale(v.X, v.Y);
        }

        public void Scale(float x, float y)
        {
            Flush();
            Matrix = Matrix.CreateScale(x, y, 1) * Matrix;
        }

        public void Pop()
        {
            Flush();
            Matrix = TransformStack.Pop();
            Alpha = AlphaStack.Pop();
        }

        public Matrix GetMatrix()
        {
            return Matrix;
        }

        public void SetMatrix(Matrix matrix)
        {
            Matrix = matrix;
        }

        public void DrawRaw(IEnumerable<VertexPositionColorTexture> vertices)
        {
            DrawRaw(vertices, Pixel);
        }

        public void DrawRaw(IEnumerable<VertexPositionColorTexture> vertices, Texture2D texture)
        {
            SetTexture(texture);

            if (VerticesLeftUntilFlush() < vertices.Count()) Flush();

            foreach(var vertex in vertices)
            {
                Vertex(vertex.Position, vertex.TextureCoordinate, vertex.Color);
            }
        }

        public void SetAlpha(float newAlpha, bool overwrite = false)
        {
            if (!overwrite) Alpha *= newAlpha;
            else Alpha = newAlpha;
        }

        public void End()
        {
            Flush();
            Drawing = false;
        }

        public void Flush()
        {
            Device.SetRenderTarget(RenderTarget);

            if (VertexIndex <= 0) return;

            Device.SamplerStates[0] = SamplerState.PointWrap;
            Device.RasterizerState = RasterizerState.CullNone;
            
            Device.DepthStencilState = DefaultDepthStencilState;
            Device.BlendState = DefaultBlendState;

            var basicEffect = CurrentEffect as BasicEffect;
            var alphaTest = CurrentEffect as AlphaTestEffect;

            if (basicEffect != null)
            {
                basicEffect.View = ProjectionMatrix;
                basicEffect.World = Matrix;
                basicEffect.Texture = Textures[0];
                basicEffect.TextureEnabled = true;
                basicEffect.VertexColorEnabled = true;
                basicEffect.Alpha = Alpha;
            }
            else if (alphaTest != null)
            {
                alphaTest.View = ProjectionMatrix;
                alphaTest.World = Matrix;
                alphaTest.Texture = Textures[0];
                alphaTest.VertexColorEnabled = true;
                alphaTest.Alpha = Alpha;
            }
            else
            {
                for(int i = 0; i < Textures.Length; i++)
                {
                    if (Textures[i] != null && CurrentEffect.Parameters[$"Texture{i}"] != null)
                        CurrentEffect.Parameters[$"Texture{i}"].SetValue(Textures[i]);

                    else if (Textures[i] != null && CurrentEffect.Parameters[$"Texture{i}Sampler"] != null)
                        CurrentEffect.Parameters[$"Texture{i}Sampler"].SetValue(Textures[i]);

                    else if (Textures[i] != null && CurrentEffect.Parameters[$"Texture{i}Sampler+Texture{i}"] != null)
                        CurrentEffect.Parameters[$"Texture{i}Sampler+Texture{i}"].SetValue(Textures[i]);
                }

                CurrentEffect.Parameters["WorldViewMatrix"].SetValue(Matrix);
                CurrentEffect.Parameters["ProjectionMatrix"].SetValue(ProjectionMatrix);
            }

            foreach (var pass in CurrentEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                Device.DrawUserPrimitives(
                    PrimitiveType.TriangleList,
                    Vertices,
                    0,
                    VertexIndex / 3);
            }

            VertexIndex = 0;
        }

        public void SetRenderTarget(RenderTarget2D target)
        {
            if(RenderTarget != target)
            {
                Flush();
            }

            RenderTarget = target;
        }

        public void ResetRenderTarget()
        {
            SetRenderTarget(null);
        }

        private void Vertex(Vector3 pos, Vector2 uv, Color blend)
        {
            Vertices[VertexIndex].Position = pos;
            Vertices[VertexIndex].Color = blend;
            Vertices[VertexIndex].TextureCoordinate = uv;

            VertexIndex++;
        }

        private void SetPixelTexture(int index = 0)
        {
            SetTexture(Pixel, index);
        }

        private void SetTexture(Texture2D texture, int index = 0)
        {
            // Idk if getting texture does stuff with video memory, if so i need to cache this
            if (Textures[index] == texture) return;

            Flush();

            Textures[index] = texture;
        }
        
        public void SetShader(Effect effect)
        {
            if (effect == null) effect = DefaultEffect;
            if (CurrentEffect == effect) return;

            Flush();
            CurrentEffect = effect;
        }
        public void ResetShader()
        {
            SetShader(null);
        }

        private int VerticesLeftUntilFlush()
        {
            return Vertices.Length - VertexIndex - 1;
        }

        public void Dispose()
        {
            DefaultEffect?.Dispose();
            Pixel.Dispose();
        }
    }
}
