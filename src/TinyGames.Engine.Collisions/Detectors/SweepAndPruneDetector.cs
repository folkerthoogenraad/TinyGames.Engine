using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TinyGames.Engine.Collisions.Contracts;
using TinyGames.Engine.Maths;
using System.Diagnostics;

namespace TinyGames.Engine.Collisions.Detectors
{
    public class SweepAndPruneDetector : IDetector
    {
        public BodyCollisionSet Detect(IEnumerable<PhysicsBody> bodies)
        {
            var bodyBounds = bodies.Where(x => !x.IgnoreCollisions).Select(x => new BodyBounds()
            {
                Body = x,
                Position = x.Position,
                Velocity = x.Velocity,
                Collider = x.Collider,
                Bounds = x.Collider.Bounds.Translated(x.Position),
                Mass = x.Mass,
                Static = x.Static,
                Solid = x.Solid,
            }).OrderBy(x => x.Bounds.Left).ToList();

            var collisions = new List<BodyCollisionIndices>();

            BodyCollisionSet result = new BodyCollisionSet();
            result.Bounds = bodyBounds;
            result.CollisionIndices = collisions;

            var possibleCollisions = new HashSet<BodyBounds>();
            
            for(int i = 0; i < bodyBounds.Count; i++)
            {
                var self = bodyBounds[i];
                
                // Remove everything to the left of current body
                possibleCollisions.RemoveWhere(x => AABB.IsOnLeft(self.Bounds, x.Bounds));

                // This can be one linq expression probably
                foreach (var other in possibleCollisions
                    .Where(x => !(x.Static && self.Static))
                    .Where(x => AABB.Overlaps(x.Bounds, self.Bounds))
                    .Where(x => self.Collider.Overlaps(x.Collider, x.Position - self.Position)) // TODO Save the normal in a select or something
                    )
                {
                    collisions.Add(new BodyCollisionIndices(self, other));
                }

                possibleCollisions.Add(self);
            }

            return result;
        }
    }
}
