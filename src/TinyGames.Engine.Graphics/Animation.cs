﻿using System;
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

        public Sprite GetSpriteNormalized(float index)
        {
            return GetSpriteByIndex((int)(index * Sprites.Length));
        }
        public Sprite GetSpriteForTime(float time)
        {
            return GetSpriteByIndex((int)(time * FrameRate));
        }

        public Sprite GetSpriteByIndex(int index)
        {
            int idx = index % Sprites.Length;
            if (idx < 0) idx += Sprites.Length;
            return Sprites[idx];
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
