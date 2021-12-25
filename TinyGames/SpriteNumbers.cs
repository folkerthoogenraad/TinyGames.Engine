using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TinyGames.Engine.Graphics;

namespace TinyGames
{
    public class SpriteNumbers
    {
        private Sprite[] Sprites;

        public float LetterSpacing { get; set; } = 1;

        public SpriteNumbers(Texture2D texture, Point offset, Point size, Point stride)
        {
            Sprites = new Sprite[10];

            for (int i = 0; i < 10; i++)
            {
                Sprites[i] = new Sprite(texture, new Rectangle(offset, size));

                offset += stride;
            }
        }

        public void DrawNumber(Graphics2D graphics, int number, Vector2 position)
        {
            string t = "" + number;

            foreach(var sprite in t.Select(c => GetSpriteForCharacter(c)))
            {
                graphics.DrawSprite(sprite, position);

                position.X += sprite.Width;
                position.X += LetterSpacing;
            }
        }

        public Vector2 Measure(int number)
        {
            string t = "" + number;

            Vector2 size = new Vector2();
            

            for(int i = 0; i < t.Length; i++)
            {
                bool last = i == t.Length - 1;

                Sprite sprite = GetSpriteForCharacter(t[i]);

                size.X += sprite.Width;
                size.Y = MathF.Max(size.Y, sprite.Height);

                if(!last)
                {
                    size += new Vector2(LetterSpacing, 0);
                }
            }

            return size;
        }

        public Sprite GetSpriteForCharacter(char c)
        {
            return GetSpriteForNumber(GetNumberForCharacter(c));
        }

        public Sprite GetSpriteForNumber(int n)
        {
            return Sprites[n];
        }

        public int GetNumberForCharacter(char c)
        {
            return c - '0';
        }
    }
}
