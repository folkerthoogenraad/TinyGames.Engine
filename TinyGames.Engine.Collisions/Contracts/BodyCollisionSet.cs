using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using System.Diagnostics;

namespace TinyGames.Engine.Collisions.Contracts
{
    public class BodyCollisionSet
    {
        public List<BodyCollisionIndices> CollisionIndices { get; set; }
        public List<BodyBounds> Bounds { get; set; }

        public IEnumerable<BodyCollision> Collisions => GetCollisions();

        private IEnumerable<BodyCollision> GetCollisions()
        {
            foreach(var collision in CollisionIndices)
            {
                var from = collision.BodyA;
                var to = collision.BodyB;

                var velocity = from.Velocity + to.Velocity;

                yield return new BodyCollision(from.Body, to.Body, velocity);
            }
        }
    }
}
