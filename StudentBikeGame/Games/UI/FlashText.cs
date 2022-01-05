using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Graphics.Fonts;

namespace StudentBikeGame.Games.UI
{
    public class FlashText
    {
        public Font Font { get; set; }
        public Vector2 Position { get; set; }
        public string Text { get; set; }

        private FlashTextAnimation _animation;
        private Vector2 _offset;
        private float _timer = 0;

        public FlashText(Font font, string text, Vector2 position)
        {
            Font = font;
            Text = text;
            Position = position;

            _offset = new Vector2(0, -16);
            _animation = new FlashTextAnimation();
        }

        public bool Update(float delta)
        {
            _animation.Update(delta);
            _timer += delta;

            _offset.Y = _animation.CurrentValue * -16;

            if(_timer > 2)
            {
                return false;
            }

            return true;
        }

        public void Draw(Graphics2D graphics)
        {
            graphics.DrawString(Font, Text, Position + _offset, Color.White, FontHAlign.Center, FontVAlign.Center);
        }


        private class FlashTextAnimation
        {
            public float CurrentValue { get; set; } = 1;
            public float Velocity { get; set; } = 0;

            public void Update(float delta)
            {
                Velocity -= 16 * delta;
                CurrentValue += Velocity * delta;

                if(CurrentValue < 0)
                {
                    CurrentValue = 0;
                    Velocity = -Velocity * 0.5f;
                }

                if (CurrentValue > 1)
                {
                    CurrentValue = 1;
                }
            }
        }
    }
}
