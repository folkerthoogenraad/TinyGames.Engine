using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace PinguinGame.Graphics
{
    public class Particle
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public Color Color = Color.White;

        public Animation Animation;

        public float Angle = 0;

        public float Timer = 0;

        public float Height = 0;
        public float HeightVelocity = 0;

        public bool Update(float delta)
        {
            Position += Velocity * delta;
            Height += HeightVelocity * delta;
            Timer += delta;

            if (Timer > Animation.Duration)
            {
                return false;
            }

            return true;
        }
    }
}
