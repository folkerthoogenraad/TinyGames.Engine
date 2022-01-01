using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Collisions.Contracts
{
    public struct BodyCollision
    {
        public Body From;
        public Body To;

        public Vector2 Velocity;

        public BodyCollision(Body from, Body to, Vector2 velocity)
        {
            From = from;
            To = to;
            Velocity = velocity;
        }
    }
}
