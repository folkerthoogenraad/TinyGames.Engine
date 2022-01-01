using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Collisions.Contracts
{
    public struct BodyCollisionIndices
    {
        public int BodyA;
        public int BodyB;

        public BodyCollisionIndices(int a, int b)
        {
            BodyA = a;
            BodyB = b;
        }
    }
}
