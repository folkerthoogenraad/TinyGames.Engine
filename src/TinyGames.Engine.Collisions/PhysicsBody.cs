﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Collisions
{
    public class PhysicsBody
    {
        public Vector2 Position;
        public Vector2 Velocity;

        public Collider Collider;
        public bool Static;
        public bool Solid;
        public bool IgnoreCollisions;
        public float Mass;

        public PhysicsBody(Vector2 position, Collider collider, bool isStatic = false, bool isSolid = false, bool ignoreCollisions = false)
        {
            Position = position;
            Velocity = new Vector2();
            Collider = collider;
            Static = isStatic;
            Solid = isSolid;
            IgnoreCollisions = ignoreCollisions;
            Mass = 1;
        }
    }
}
