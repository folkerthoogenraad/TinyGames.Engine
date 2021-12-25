﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Collisions
{
    public class Body
    {
        public Vector2 Position;
        public Vector2 Velocity;

        public Collider Collider;
        public bool Static;

        public Body(Vector2 position, Collider collider)
        {
            Position = position;
            Velocity = new Vector2();
            Collider = collider;
        }
    }
}
