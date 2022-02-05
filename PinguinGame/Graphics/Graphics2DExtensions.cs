using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace PinguinGame.Graphics
{
    public static class Graphics2DExtensions
    {
        public static void DrawSpriteWithDepths(this Graphics2D graphics, Sprite sprite, Vector2 position, float depthTop, float depthBottom, Color color)
        {
            var vertices = new VertexPositionColorTexture[6];

            // a - b
            // | \ |
            // d - c

            Vector2 a = position - sprite.Origin;
            Vector2 b = position - sprite.Origin + new Vector2(sprite.Width, 0);
            Vector2 c = position - sprite.Origin + new Vector2(sprite.Width, sprite.Height);
            Vector2 d = position - sprite.Origin + new Vector2(0, sprite.Height);

            float tWidth = sprite.Texture.Width;
            float tHeight = sprite.Texture.Height;

            Rectangle rect = sprite.SourceRectangle;

            Vector2 uvA = new Vector2(rect.Left / tWidth, rect.Top / tHeight);
            Vector2 uvB = new Vector2(rect.Right / tWidth, rect.Top / tHeight);
            Vector2 uvC = new Vector2(rect.Right / tWidth, rect.Bottom / tHeight);
            Vector2 uvD = new Vector2(rect.Left / tWidth, rect.Bottom / tHeight);

            vertices[0] = new VertexPositionColorTexture(new Vector3(a.X, a.Y, depthTop), color, uvA);
            vertices[1] = new VertexPositionColorTexture(new Vector3(b.X, b.Y, depthTop), color, uvB);
            vertices[2] = new VertexPositionColorTexture(new Vector3(c.X, c.Y, depthBottom), color, uvC);

            vertices[3] = new VertexPositionColorTexture(new Vector3(a.X, a.Y, depthTop), color, uvA);
            vertices[4] = new VertexPositionColorTexture(new Vector3(c.X, c.Y, depthBottom), color, uvC);
            vertices[5] = new VertexPositionColorTexture(new Vector3(d.X, d.Y, depthBottom), color, uvD);

            graphics.DrawRaw(vertices, sprite.Texture);
        }

        public static void DrawSpriteWithColors(this Graphics2D graphics, Sprite sprite, Vector2 position, float depth, Color colorTop, Color colorBottom)
        {
            var vertices = new VertexPositionColorTexture[6];

            // a - b
            // | \ |
            // d - c

            Vector2 a = position - sprite.Origin;
            Vector2 b = position - sprite.Origin + new Vector2(sprite.Width, 0);
            Vector2 c = position - sprite.Origin + new Vector2(sprite.Width, sprite.Height);
            Vector2 d = position - sprite.Origin + new Vector2(0, sprite.Height);

            float tWidth = sprite.Texture.Width;
            float tHeight = sprite.Texture.Height;

            Rectangle rect = sprite.SourceRectangle;

            Vector2 uvA = new Vector2(rect.Left / tWidth, rect.Top / tHeight);
            Vector2 uvB = new Vector2(rect.Right / tWidth, rect.Top / tHeight);
            Vector2 uvC = new Vector2(rect.Right / tWidth, rect.Bottom / tHeight);
            Vector2 uvD = new Vector2(rect.Left / tWidth, rect.Bottom / tHeight);

            vertices[0] = new VertexPositionColorTexture(new Vector3(a.X, a.Y, depth), colorTop, uvA);
            vertices[1] = new VertexPositionColorTexture(new Vector3(b.X, b.Y, depth), colorTop, uvB);
            vertices[2] = new VertexPositionColorTexture(new Vector3(c.X, c.Y, depth), colorBottom, uvC);

            vertices[3] = new VertexPositionColorTexture(new Vector3(a.X, a.Y, depth), colorTop, uvA);
            vertices[4] = new VertexPositionColorTexture(new Vector3(c.X, c.Y, depth), colorBottom, uvC);
            vertices[5] = new VertexPositionColorTexture(new Vector3(d.X, d.Y, depth), colorBottom, uvD);

            graphics.DrawRaw(vertices, sprite.Texture);
        }
    }
}
