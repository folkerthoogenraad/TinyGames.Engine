using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Collisions.Contracts
{
    public struct BodyCollision
    {
        public PhysicsBody From;
        public PhysicsBody To;

        public Vector2 Velocity;

        public BodyCollision(PhysicsBody from, PhysicsBody to, Vector2 velocity)
        {
            From = from;
            To = to;
            Velocity = velocity;
        }
    }
}
