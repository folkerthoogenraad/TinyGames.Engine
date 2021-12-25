using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TinyGames.Engine.Collisions.Contracts;
using TinyGames.Engine.Maths;
using Microsoft.Xna.Framework;

namespace TinyGames.Engine.Collisions.Solvers
{
    public class DefaultSolver : ISolver
    {
        public void Solve(CollisionSet set)
        {
            foreach(var collision in set.Collisions)
            {
                var boundsA = set.Bounds[collision.BodyA];
                var boundsB = set.Bounds[collision.BodyB];

                if (!boundsA.Solid && !boundsB.Solid) continue;

                var aabbA = boundsA.Bounds.Translated(boundsA.UnstuckMotion);
                var aabbB = boundsB.Bounds.Translated(boundsB.UnstuckMotion);

                var minkow = AABB.MinkowskiDifference(aabbA, aabbB);

                Vector2 unstuck = -minkow.Unstuck(Vector2.Zero);

                float length = unstuck.LengthSquared();
                if (length <= 0) continue;

                Vector2 normal = -unstuck / length;

                float totalMass = boundsA.Mass + boundsB.Mass;

                if (totalMass == 0) continue;

                float balance = boundsB.Mass / totalMass;

                if (boundsA.Static) balance = 0;
                if (boundsB.Static) balance = 1;

                boundsA.UnstuckMotion += unstuck * balance;
                boundsB.UnstuckMotion += -unstuck * (1 - balance);

                //boundsA.Body.Velocity -= normal * Vector2.Dot(boundsA.Body.Velocity, normal);
                //boundsB.Body.Velocity -= -normal * Vector2.Dot(boundsB.Body.Velocity, -normal);
            }
        }
    }
}
