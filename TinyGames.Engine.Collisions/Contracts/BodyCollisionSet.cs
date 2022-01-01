using System;
using System.Collections.Generic;
using System.Text;

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
                var from = Bounds[collision.BodyA];
                var to = Bounds[collision.BodyB];

                var velocity = from.Velocity + to.Velocity;

                yield return new BodyCollision(from.Body, to.Body, velocity);
            }
        }
    }
}
