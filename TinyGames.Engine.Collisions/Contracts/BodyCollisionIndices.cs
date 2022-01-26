using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Collisions.Contracts
{
    public struct BodyCollisionIndices
    {
        public BodyBounds BodyA;
        public BodyBounds BodyB;

        public BodyCollisionIndices(BodyBounds a, BodyBounds b)
        {
            BodyA = a;
            BodyB = b;
        }
    }
}
