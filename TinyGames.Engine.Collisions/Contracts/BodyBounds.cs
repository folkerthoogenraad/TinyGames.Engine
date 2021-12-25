using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Maths;

namespace TinyGames.Engine.Collisions.Contracts
{
    public class BodyBounds
    {
        public Body Body;
        public Vector2 Position;
        public Vector2 Velocity;
        
        public AABB Bounds;
        public float Mass;

        public bool Static;
        public bool Solid;

        public Vector2 UnstuckMotion;
    }
}
