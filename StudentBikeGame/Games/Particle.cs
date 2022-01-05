using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace StudentBikeGame.Games
{
    public class Particle
    {
        public Vector2 Position;
        public Color Color;

        public Animation Animation;

        public float Angle = 0;

        public float Timer = 0;
        
        public bool Update(float delta)
        {
            Timer += delta;

            if (Timer > Animation.Duration)
            {
                return false;
            }

            return true;
        }
    }

}
