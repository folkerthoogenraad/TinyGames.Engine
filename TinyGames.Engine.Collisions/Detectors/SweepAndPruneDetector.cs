using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TinyGames.Engine.Collisions.Contracts;

namespace TinyGames.Engine.Collisions.Detectors
{
    public class SweepAndPruneDetector : IDetector
    {
        public CollisionSet Solve(IEnumerable<Body> bodies)
        {
            var bodyBounds = bodies.Select(x => new BodyBounds()
            {
                Body = x,
                Position = x.Position,
                Velocity = x.Velocity,
                Bounds = x.Collider.Bounds.Translated(x.Position),
                Mass = 1,
                Static = x.Static
            }).OrderBy(x => x.Bounds.Left).ToList();

            var collisions = new List<Collision>();
            // TODO actually do sweep and prune instead of this shit but ok

            CollisionSet result = new CollisionSet();
            result.Bounds = bodyBounds;
            result.Collisions = collisions;
            
            for(int i = 0; i < bodyBounds.Count; i++)
            {
                var boundsA = bodyBounds[i];

                for(int j = i + 1; j < bodyBounds.Count; j++)
                {
                    var boundsB = bodyBounds[j];

                    // No static static collisions. Its kinda useless.
                    if (boundsA.Static && boundsB.Static) continue;

                    if (boundsA.Bounds.Overlaps(boundsB.Bounds))
                    {
                        collisions.Add(new Collision(i, j));
                    }
                }
            }

            return result;
        }
    }
}
