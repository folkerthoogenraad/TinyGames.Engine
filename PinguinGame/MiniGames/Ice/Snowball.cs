using Microsoft.Xna.Framework;
using PinguinGame.Player;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.MiniGames.Ice
{
    public class Snowball
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Lifetime { get; set; }

        public float Angle => 0;
        public float Height { get; set; }
        public bool Collided { get; set; } = false;
        public PlayerInfo Player { get; set; }

        public bool Update(float delta)
        {
            if (Collided) return false;
            
            Position += Velocity * delta;

            Lifetime -= delta;

            if(Lifetime < 0)
            {
                return false;
            }

            return true;
        }
    }
}
