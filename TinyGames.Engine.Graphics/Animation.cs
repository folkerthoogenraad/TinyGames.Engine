using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Graphics
{
    public class Animation
    {
        public Sprite[] Sprites;

        public float FrameRate { get; set; } = 12;

        public float Duration => Sprites.Length / FrameRate;

        public Animation(params Sprite[] sprites)
        {
            Sprites = sprites;
        }

        public Sprite GetSpriteForTime(float time)
        {
            return GetSpriteByIndex((int)(time * FrameRate));
        }

        public Sprite GetSpriteByIndex(int index)
        {
            return Sprites[index % Sprites.Length];
        }

        public Animation SetFrameRate(float rate)
        {
            FrameRate = rate;

            return this;
        }

        public static Animation FromSprites(float frameRate, params Sprite[] sprites)
        {
            return new Animation(sprites) { FrameRate = frameRate };
        }
    }
}
