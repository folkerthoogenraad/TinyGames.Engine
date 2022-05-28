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
        public Vector2 Normal;

        public BodyCollision(PhysicsBody from, PhysicsBody to, Vector2 velocity, Vector2 normal)
        {
            From = from;
            To = to;
            Velocity = velocity;
            Normal = normal;
        }
    }
}
