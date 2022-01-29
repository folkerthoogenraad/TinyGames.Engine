using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.MiniGames.Ice
{
    public class CharacterBounce
    {
        public float Bouncyness { get; set; } = 0.5f;
        public float Height { get; set; }
        public float Velocity { get; set; }
        public float Gravity { get; set; } = 128;

        public Vector2 Offset => new Vector2(0, -Height);

        public void Update(float delta)
        {
            Velocity += Gravity * delta;
            Height -= Velocity * delta;

            if(Height < 0)
            {
                Height = 0;
                Velocity = -Velocity * Bouncyness;
            }
        }
    }
}
