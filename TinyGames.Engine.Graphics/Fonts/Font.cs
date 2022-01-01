using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TinyGames.Engine.Graphics;

namespace TinyGames.Engine.Graphics.Fonts
{
    public class Font
    {
        private Dictionary<char, Sprite> Sprites;

        public float LetterSpacing { get; set; } = 1;

        public Font()
        {

        }

        public void DrawString(Graphics2D graphics, string t, Vector2 position)
        {
            foreach (var sprite in t.Select(c => GetSpriteForCharacter(c)))
            {
                graphics.DrawSprite(sprite, position);

                position.X += sprite.Width;
                position.X += LetterSpacing;
            }
        }

        public Vector2 Measure(string t)
        {
            Vector2 size = new Vector2();


            for (int i = 0; i < t.Length; i++)
            {
                bool last = i == t.Length - 1;

                Sprite sprite = GetSpriteForCharacter(t[i]);

                size.X += sprite.Width;
                size.Y = MathF.Max(size.Y, sprite.Height);

                if (!last)
                {
                    size += new Vector2(LetterSpacing, 0);
                }
            }

            return size;
        }

        public Sprite GetSpriteForCharacter(char c)
        {
            return Sprites[c];
        }

        public void SetSpriteForCharacter(char c, Sprite s)
        {
            Sprites[c] = s;
        }

    }
}
