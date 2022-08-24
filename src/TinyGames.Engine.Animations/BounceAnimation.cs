using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Animations
{
    public class BounceAnimation
    {
        public float Gravity { get; set; } = 10;
        public float Height { get; set; } = 0;
        public float Velocity { get; set; } = 0;
        public float Bouncyness { get; set; } = 0.5f;


        public float MinHeight { get; set; } = 0;
        public float MaxHeight { get; set; } = 1;

        public void Update(float delta)
        {
            Velocity -= Gravity * delta;
            Height += Velocity * delta;

            if (Height < MinHeight)
            {
                Height = MinHeight;
                Velocity = -Velocity * Bouncyness;
            }
            if (Height > MaxHeight)
            {
                Height = MaxHeight;
                Velocity = -Velocity * Bouncyness;
            }
        }
    }
}
