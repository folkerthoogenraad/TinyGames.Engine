using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TinyGames.Engine.Graphics.Fonts.LoadersAndGenerators
{
    public class TextureReadWriter
    {
        public Color[] Colors { get; }
        public int Width { get; }
        public int Height { get; }

        public TextureReadWriter(Color[] colors, int width, int height)
        {
            Colors = colors;
            Width = width;
            Height = height;
        }

        public Color GetColor(int x, int y)
        {
            return Colors[x + y * Width];
        }

        public void SetColor(int x, int y, Color color)
        {
            Colors[x + y * Width] = color;
        }

        public void Commit(Texture2D texture)
        {
            texture.SetData(Colors);
        }
    }

    public class FontOutlineSampler
    {
        private Rectangle _readRectangle;

        private TextureReadWriter _reader;

        public FontOutlineSampler(Rectangle readRectangle, TextureReadWriter reader)
        {
            _readRectangle = readRectangle;
            _reader = reader;
        }

        public bool IsSolid(int x, int y)
        {
            if (x < 0) return false;
            if (y < 0) return false;

            if (x >= _readRectangle.Width) return false;
            if (y >= _readRectangle.Height) return false;

            return _reader.GetColor(_readRectangle.X + x, _readRectangle.Y + y).A > 0;
        }

        public bool ShouldWrite(int x, int y)
        {
            // Something means no write
            if (IsSolid(x, y)) return false;

            // Sample all eight spots around
            if (IsSolid(x - 1, y)) return true;
            if (IsSolid(x + 1, y)) return true;
            if (IsSolid(x, y + 1)) return true;
            if (IsSolid(x, y - 1)) return true;

            if (IsSolid(x - 1, y - 1)) return true;
            if (IsSolid(x - 1, y + 1)) return true;
            if (IsSolid(x + 1, y - 1)) return true;
            if (IsSolid(x + 1, y + 1)) return true;

            // Nothing means no write
            return false;
        }

        public void Write(TextureReadWriter target, int xOffset, int yOffset)
        {
            int width = _readRectangle.Width;
            int height = _readRectangle.Height;

            for (int i = 0; i < width + 2; i++)
            {
                for(int j = 0; j < height + 2; j++)
                {
                    if(ShouldWrite(i - 1, j - 1))
                    {
                        target.SetColor(xOffset + i, yOffset + j, Color.White);
                    }
                    else
                    {
                        target.SetColor(xOffset + i, yOffset + j, Color.Transparent);
                    }
                }
            }
        }
    }

    // TODO make this into something that reads two non color but bool arrays so it can more easily be tested.
    public class FontOutline
    {
        private Font _sourceFont;
        private GraphicsDevice _device;
        
        private Texture2D _texture;
        private Color[] _colors;
        private Color[] _sourceColors;

        private TextureReadWriter _reader;
        private TextureReadWriter _writer;

        private int _glyphWidth;
        private int _glyphHeight;
        private int _columnCount;

        public FontOutline(GraphicsDevice device, Font font)
        {
            _device = device;
            _sourceFont = font;

            _sourceColors = new Color[font.Texture.Width * font.Texture.Height];
            font.Texture.GetData(_sourceColors);

            _reader = new TextureReadWriter(_sourceColors, font.Texture.Width, font.Texture.Height);
            CreateTexture();
        }

        public Font Create()
        {
            Font font = new Font(_texture);

            font.Height = _sourceFont.Height;
            font.LetterSpacing = _sourceFont.LetterSpacing;
            font.SpaceSize = _sourceFont.SpaceSize;
            font.Baseline = _sourceFont.Baseline;

            int index = 0;

            int xOffset = 0;
            int yOffset = 0;

            foreach(var glyph in _sourceFont.Glyphs)
            {
                FontOutlineSampler sampler = new FontOutlineSampler(glyph.Value.Rectangle, _reader);
                sampler.Write(_writer, xOffset, yOffset);

                font.SetGlyphForCharacter(glyph.Key, new Glyph() {
                    OffsetX = glyph.Value.OffsetX - 1,
                    OffsetY = glyph.Value.OffsetY - 1,
                    Advance = glyph.Value.Advance,
                    Rectangle = new Rectangle(xOffset, yOffset, _glyphWidth, _glyphHeight),
                });

                xOffset += _glyphWidth;
                index++;

                if(index >= _columnCount)
                {
                    index = 0;
                    xOffset = 0;
                    yOffset += _glyphHeight;
                }
            }

            _writer.Commit(_texture);

            return font;
        }

        private void CreateTexture()
        {
            int width = _sourceFont.Glyphs.Max(x => x.Value.Width);
            int height = _sourceFont.Height;

            int newWidth = width + 2;
            int newHeight = height + 2;

            int glyphCount = _sourceFont.Glyphs.Count;

            // Inefficient but w/e
            int rows = (int)Math.Ceiling(Math.Sqrt(glyphCount));
            int columns = glyphCount / rows;

            _columnCount = columns;

            _glyphWidth = newWidth;
            _glyphHeight = newHeight;

            int textureWidth = RoundToNearestPowerOfTwo((rows) * newWidth * 2);
            int textureHeight = RoundToNearestPowerOfTwo((columns) * newHeight * 2);

            _colors = new Color[textureWidth * textureHeight];
            _texture = new Texture2D(_device, textureWidth, textureHeight);
            _writer = new TextureReadWriter(_colors, textureWidth, textureHeight);
        }

        public int RoundToNearestPowerOfTwo(int input)
        {
            return (int)Math.Pow(2, Math.Ceiling(Math.Log2(input)));
        }

        public static Font Create(GraphicsDevice device, Font source)
        {
            return new FontOutline(device, source).Create();
        }
    }
}
