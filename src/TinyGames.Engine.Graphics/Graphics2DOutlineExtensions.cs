using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyGames.Engine.Maths;

namespace TinyGames.Engine.Graphics
{
    public enum OutlineAlignment
    {
        Inside,
        Center,
        Outside,
    }

    public static class Graphics2DOutlineExtensions
    {
        public static void DrawCircleOutline(this Graphics2D graphics, Vector2 pos, float radius, Color color, float width = 1, OutlineAlignment alignment = OutlineAlignment.Center, float depth = 0, int subdivisions = 32)
        {
            graphics.DrawDonut(pos, radius - width * InsideAlignment(alignment), radius + width * OutsideAlignment(alignment), color, depth, subdivisions);
        }

        public static void DrawRectangleOutline(this Graphics2D graphics, Vector2 position, Vector2 size, Color color, float width = 1, OutlineAlignment alignment = OutlineAlignment.Center, float depth = 0)
        {
            float outside = width * OutsideAlignment(alignment);

            size += new Vector2(outside * 2, outside * 2);
            position -= new Vector2(outside, outside);

            Vector2 tl = new Vector2(position.X, position.Y);
            Vector2 tr = new Vector2(position.X + size.X, position.Y);
            Vector2 br = new Vector2(position.X + size.X, position.Y + size.Y);
            Vector2 bl = new Vector2(position.X, position.Y + size.Y);

            Vector2 itl = tl + new Vector2(width, width);
            Vector2 itr = tr + new Vector2(-width, width);
            Vector2 ibr = br + new Vector2(-width, -width);
            Vector2 ibl = bl + new Vector2(width, -width);

            graphics.DrawQuad(tl, tr, itr, itl, color);
            graphics.DrawQuad(tr, br, ibr, itr, color);
            graphics.DrawQuad(br, bl, ibl, ibr, color);
            graphics.DrawQuad(bl, tl, itl, ibl, color);
        }

        public static void DrawRectangleOutline(this Graphics2D graphics, Rectangle rect, Color color, float width = 1, OutlineAlignment alignment = OutlineAlignment.Center, float depth = 0)
        {
            graphics.DrawRectangleOutline(new Vector2(rect.X, rect.Y), new Vector2(rect.Width, rect.Height), color, width, alignment, depth);
        }

        public static void DrawRectangleOutline(this Graphics2D graphics, float x, float y, float w, float h, Color color, float width = 1, OutlineAlignment alignment = OutlineAlignment.Center, float depth = 0)
        {
            graphics.DrawRectangleOutline(new Vector2(x, y), new Vector2(w, h), color, width, alignment, depth);
        }

        public static void DrawRectangleOutline(this Graphics2D graphics, AABB bounds, Color color, float width = 1, OutlineAlignment alignment = OutlineAlignment.Center, float depth = 0)
        {
            graphics.DrawRectangleOutline(bounds.TopLeft, bounds.Size, color, width, alignment, depth);
        }

        private static float InsideAlignment(OutlineAlignment alignment)
        {
            return alignment switch
            {
                OutlineAlignment.Inside => 1,
                OutlineAlignment.Center => 0.5f,
                OutlineAlignment.Outside => 0,
                _ => 0
            };
        }

        private static float OutsideAlignment(OutlineAlignment alignment)
        {
            return alignment switch
            {
                OutlineAlignment.Inside => 0,
                OutlineAlignment.Center => 0.5f,
                OutlineAlignment.Outside => 1,
                _ => 0
            };
        }
    }
}
