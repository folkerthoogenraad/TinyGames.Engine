﻿using System;
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
        public void Solve(BodyCollisionSet set)
        {
            foreach(var collision in set.CollisionIndices)
            {
                var boundsA = collision.BodyA;
                var boundsB = collision.BodyB;

                if (!boundsA.Solid || !boundsB.Solid) continue;

                var relativePosition = (boundsB.Position + boundsB.UnstuckMotion) - (boundsA.Position + boundsA.UnstuckMotion);

                if (!ColliderOverlaps.Overlaps(boundsA.Collider, boundsB.Collider, relativePosition)) continue;

                Vector2 unstuck = ColliderPenetration.Penetration(boundsA.Collider, boundsB.Collider, relativePosition);

                float length = unstuck.LengthSquared();
                if (length <= 0) continue;

                float totalMass = boundsA.Mass + boundsB.Mass;

                if (totalMass == 0) continue;

                float balance = boundsB.Mass / totalMass;

                if (boundsA.Static) balance = 0;
                if (boundsB.Static) balance = 1;

                boundsA.UnstuckMotion += unstuck * balance;
                boundsB.UnstuckMotion += -unstuck * (1 - balance);
            }
        }
    }
}
