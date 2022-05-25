using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics.Fonts.LoadersAndGenerators.Contracts;

// TODO this should probably be in a seperate thing but ok
namespace TinyGames.Engine.Graphics.Fonts.LoadersAndGenerators
{

    public class FontLoader
    {
        private FontLoaderSettings Settings;
        private Color[] Colors;
        
        public FontLoader(FontLoaderSettings settings)
        {
            Settings = settings;

            Colors = new Color[settings.Texture.Width * settings.Texture.Height];
            settings.Texture.GetData(Colors);
        }

        public Font Load()
        {
            Font font = new Font(Settings.Texture);

            font.LetterSpacing = Settings.LetterSpacing;
            font.Height = Settings.GlyphHeight;
            font.Baseline = Settings.GlyphBaseline;
            font.SpaceSize = Settings.GlyphWidth;

            int x = Settings.GlyphOffsetX;
            int y = Settings.GlyphOffsetY;

            foreach(var row in Settings.GlyphRows)
            {
                x = Settings.GlyphOffsetX;

                foreach (var ch in row)
                {
                    int width = GetCharacterWidth(new Rectangle(x, y, Settings.GlyphWidth, Settings.GlyphHeight));
                    int height = Settings.GlyphHeight;

                    Glyph glyph = new Glyph() {
                        Rectangle = new Rectangle(x, y, width, height),
                        Advance = width,
                        OffsetX = 0,
                        OffsetY = 0,
                    };

                    font.SetGlyphForCharacter(ch, glyph);

                    x += Settings.GlyphSeperationY + Settings.GlyphWidth;
                }

                y += Settings.GlyphSeperationY + Settings.GlyphHeight;
            }

            return font;
        }

        private int GetCharacterWidth(Rectangle scanRect)
        {
            if (Settings.Monospaced) return scanRect.Width;

            int width = 0;

            for(int i = 0; i < scanRect.Width; i++)
            {
                for(int j = 0; j < scanRect.Height; j++)
                {
                    if (IsPixelSolid(i + scanRect.X, j + scanRect.Y))
                    {
                        width = i;
                        break;
                    }
                }
            }

            return width + 1;
        }

        private bool IsPixelSolid(int x, int y)
        {
            return Colors[x + y * Settings.Texture.Width].A > 128;
        }


        public static Font Load(FontLoaderSettings settings)
        {
            return new FontLoader(settings).Load();
        }
    }
}
