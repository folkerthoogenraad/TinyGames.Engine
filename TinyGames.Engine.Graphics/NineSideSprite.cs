using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using TinyGames.Engine.Maths;

namespace TinyGames.Engine.Graphics
{
    public class NineSideSprite
    {
        public Texture2D Texture { get; set; }
        public Rectangle SourceRectangle { get; set; }

        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }

        public int HorizontalMiddle => SourceRectangle.Width - Left - Right;
        public int VerticalMiddle => SourceRectangle.Height - Top - Bottom;

        public NineSideSprite(Texture2D texture, Rectangle sourceRectangle, int left = 0, int right = 0, int top = 0, int bottom = 0)
        {
            Texture = texture;
            SourceRectangle = sourceRectangle;
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        public void Draw(Graphics2D graphics, Vector2 position, Vector2 size)
        {
            Draw(graphics, position, size, Color.White);
        }

        public void Draw(Graphics2D graphics, Vector2 position, Vector2 size, Color color)
        {
            var horizontal = GetHorizontalSegmentLengths(size.X).ToArray();
            var vertical = GetVerticalSegmentLengths(size.Y).ToArray();

            Vector2 offset = position;
            Point textureOffset = SourceRectangle.Location;

            for (int i = 0; i < horizontal.Length; i++)
            {
                var h = horizontal[i];

                offset.Y = position.Y;
                textureOffset.Y = SourceRectangle.Location.Y;

                for (int j = 0; j < vertical.Length; j++)
                {
                    var v = vertical[j];

                    graphics.DrawTextureRegion(Texture, AABB.Create(textureOffset.X, textureOffset.Y, h.TextureWidth, v.TextureHeight), offset, new Vector2(h.SegmentWidth, v.SegmentHeight), color);

                    offset.Y += v.SegmentHeight;
                    textureOffset.Y += v.TextureHeight;
                }

                offset.X += h.SegmentWidth;
                textureOffset.X += h.TextureWidth;
            }
        }

        private IEnumerable<(float SegmentWidth, int TextureWidth)> GetHorizontalSegmentLengths(float width)
        {
            yield return (Left, Left);
            yield return (width - Left - Right, HorizontalMiddle);
            yield return (Right, Right);
        }
        private IEnumerable<(float SegmentHeight, int TextureHeight)> GetVerticalSegmentLengths(float height)
        {
            yield return (Top, Top);
            yield return (height - Top - Bottom, VerticalMiddle);
            yield return (Bottom, Bottom);
        }
    }
}
