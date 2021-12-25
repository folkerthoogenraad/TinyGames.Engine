using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Collisions.Contracts
{
    public class CollisionSet
    {
        public List<Collision> Collisions { get; set; }
        public List<BodyBounds> Bounds { get; set; }
    }
}
