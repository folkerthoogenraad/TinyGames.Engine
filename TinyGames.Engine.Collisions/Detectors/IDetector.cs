using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Collisions.Contracts;

namespace TinyGames.Engine.Collisions.Detectors
{
    public interface IDetector
    {
        public BodyCollisionSet Solve(IEnumerable<Body> bodies);
    }
}
