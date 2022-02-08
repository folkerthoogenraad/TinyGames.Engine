using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Extensions;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Maths;

namespace PinguinGame.Screens.Fades
{
    public class RandomBlockFade : Fade
    {
        public Color Color { get; set; }
        public float[] Blocks { get; set; }

        public bool Forward { get; set; } = true;

        public RandomBlockFade(Color color, int blocks = 8)
        {
            Color = color;
            Blocks = new float[blocks];

            Random random = new Random();

            for(int i = 0; i < blocks; i++)
            {
                Blocks[i] = random.NextFloatRange(0.2f, 0.8f);
            }
        }

        public override void Draw(Graphics2D graphics, float amount, AABB bounds)
        {
            float widthPerSegment = bounds.Width / Blocks.Length;
            float height = bounds.Height;

            for(int i = 0; i < Blocks.Length; i++)
            {
                float iOff = i / (float)Blocks.Length;

                float am;

                if (Forward) am = Math.Clamp(amount * 2 - iOff, 0, 1);
                else am = Math.Clamp(amount * 2 - (1 - iOff), 0, 1);

                float top = bounds.Y;
                float bottom = bounds.Y + height;
                float f = Blocks[i];

                float left = bounds.X + widthPerSegment * i;
                float right = bounds.X + widthPerSegment * (i + 1);

                // Draw top block
                graphics.DrawRectangle(new AABB()
                {
                    Left = left,
                    Right = right,
                    Top = top,
                    Bottom = top + f * height * am,
                }, Color);

                // Draw bottom block
                graphics.DrawRectangle(new AABB()
                {
                    Left = left,
                    Right = right,
                    Top = bottom - (1 - f) * height * am,
                    Bottom = bottom,
                }, Color);
            }
        }
    }
}
