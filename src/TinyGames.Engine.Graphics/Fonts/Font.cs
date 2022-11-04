using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TinyGames.Engine.Graphics;

namespace TinyGames.Engine.Graphics.Fonts
{
    public class Glyph
    {
        public Rectangle Rectangle { get; set; }

        public int Width => Rectangle.Width;
        public int Height => Rectangle.Height;

        public int OffsetX = 0;
        public int OffsetY = 0;
        public int Advance { get; set; }
    }

    public class Font : IFont
    {
        private Texture2D _texture;
        private Dictionary<char, Glyph> _glyphs;

        // TODO make this into something readonly or something
        public Dictionary<char, Glyph> Glyphs => _glyphs;
        public Texture2D Texture => _texture;

        public float LetterSpacing { get; set; } = 1;
        public float Baseline { get; set; } = 0;
        public int Height { get; set; } = 0;

        public float SpaceSize { get; set; } = 4;

        public Font(Texture2D texture)
        {
            _glyphs = new Dictionary<char, Glyph>();
            _texture = texture; 
        }

        public void DrawString(Graphics2D graphics, string t, Vector2 position, Vector2 scale, Color color, FontHAlign halign, FontVAlign valign)
        {
            position -= scale * Measure(t) * GetAlignVector(halign, valign);

            foreach (var ch in t)
            {
                if(ch == ' ')
                {
                    position.X += SpaceSize * scale.X;
                    continue;
                }

                var glyph = GetGlyphForCharacter(ch);

                if (glyph == null) continue;

                graphics.DrawTextureRegion(_texture, glyph.Rectangle, position + new Vector2(glyph.OffsetX, glyph.OffsetY) * scale, scale * new Vector2(glyph.Width, glyph.Height), color);

                position.X += glyph.Advance * scale.X;
                position.X += LetterSpacing * scale.X;
            }
        }

        public Vector2 Measure(string t)
        {
            Vector2 size = new Vector2();
            size.Y = Height;

            int offsetX = 0;

            for (int i = 0; i < t.Length; i++)
            {
                bool last = i == t.Length - 1;

                char ch = t[i];

                if(ch == ' ')
                {
                    size.X += SpaceSize;
                    continue;
                }

                Glyph glyph = GetGlyphForCharacter(ch);

                if (glyph == null) continue; // TODO default glyph

                size.X += glyph.Advance;

                // TODO figure out if we need this.
                // This doesn't quite work if we put the offset here.
                offsetX = 0;// glyph.Width - glyph.Advance;

                if (!last)
                {
                    size += new Vector2(LetterSpacing, 0);
                }
            }

            return new Vector2(size.X + offsetX, size.Y);
        }

        public float OffsetOf(string t, int index)
        {
            float amount = 0;
            for (int i = 0; i < index; i++)
            {
                bool last = i == index - 1;

                char ch = t[i];

                if (ch == ' ')
                {
                    amount += SpaceSize;
                    continue;
                }

                Glyph glyph = GetGlyphForCharacter(ch);

                if (glyph == null) continue; // TODO default glyph

                amount += glyph.Advance;

                if (!last)
                {
                    amount += LetterSpacing;
                }
            }

            return amount;
        }

        public Glyph GetGlyphForCharacter(char c)
        {
            if (_glyphs.ContainsKey(c))
            {
                return _glyphs[c];
            }
            return null;
        }

        public void SetGlyphForCharacter(char c, Glyph g)
        {
            _glyphs[c] = g;
        }

        private Vector2 GetAlignVector(FontHAlign hAlign, FontVAlign vAlign)
        {
            float x = 0;
            float y = 0;

            if (hAlign == FontHAlign.Left) x = 0;
            if (hAlign == FontHAlign.Center) x = 0.5f;
            if (hAlign == FontHAlign.Right) x = 1;

            if (vAlign == FontVAlign.Top) y = 0;
            if (vAlign == FontVAlign.Center) y = 0.5f;
            if (vAlign == FontVAlign.Bottom) y = 1;

            return new Vector2(x, y);
        }
    }
}
