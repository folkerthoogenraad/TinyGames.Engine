using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Graphics.Fonts.LoadersAndGenerators.Contracts
{
    public class FontLoaderSettings
    {
        public Texture2D Texture { get; set; }

        public string[] GlyphRows { get; set; }

        public int GlyphOffsetX { get; set; }
        public int GlyphOffsetY { get; set; }

        public int GlyphSeperationX { get; set; }
        public int GlyphSeperationY { get; set; }

        public int GlyphWidth { get; set; }
        public int GlyphHeight { get; set; }

        public int GlyphBaseline { get; set; }

        public int LetterSpacing { get; set; }
    }
}
