using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Collisions.Contracts;

namespace TinyGames.Engine.Collisions.Solvers
{
    public interface ISolver
    {
        public void Solve(CollisionSet set);
    }
}
