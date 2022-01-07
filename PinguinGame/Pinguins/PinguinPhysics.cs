using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Pinguins
{
    public class PinguinSettings
    {
        public float Acceleration = 3;
        public float MoveSpeed = 60;
    }

    public class PinguinPhysics
    {
        public PinguinSettings Settings { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }

        public bool IsWalking { get; private set; }
        public bool IsSliding { get; private set; }
        public Vector2 Facing { get; private set; }

        public PinguinPhysics(PinguinSettings settings, Vector2 position)
        {
            Settings = settings;
            Position = position;
        }

        public PinguinPhysics Update(float delta, Vector2 moveDirection)
        {
            var state = Clone();

            state.Velocity = Vector2.Lerp(Velocity, moveDirection * Settings.MoveSpeed, delta * Settings.Acceleration);
            state.Position += state.Velocity * delta;

            if(moveDirection.LengthSquared() > 0)
            {
                state.Facing = moveDirection;
                state.IsWalking = true;
            }
            else { state.IsWalking = false; }


            return state;
        }

        private PinguinPhysics Clone()
        {
            return MemberwiseClone() as PinguinPhysics;
        }
    }
}
