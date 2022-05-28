using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using System.Diagnostics;
using TinyGames.Engine.Maths;
using Microsoft.Xna.Framework;

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

                var relativeVelocity = to.Velocity - from.Velocity;

                yield return new BodyCollision(from.Body, to.Body, -relativeVelocity, from.UnstuckMotion.NormalizedOrDefault(new Vector2(1, 0)));
                yield return new BodyCollision(to.Body, from.Body, relativeVelocity, to.UnstuckMotion.NormalizedOrDefault(new Vector2(1, 0)));
            }
        }
    }
}
