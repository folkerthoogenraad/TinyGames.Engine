using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PinguinGame.Pinguins
{

    public class PenguinPhysics
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; private set; }
        public Vector2 Facing { get; private set; }

        public PenguinPhysics(Vector2 position)
        {
            Position = position;
        }

        public PenguinPhysics Move(float delta, Vector2 speed, float acceleration)
        {
            var state = Clone();

            state.Velocity = Vector2.Lerp(Velocity, speed, delta * acceleration);
            state.Position += state.Velocity * delta;

            if(speed.LengthSquared() > 0)
            {
                state.Facing = speed;
            }

            return state;
        }

        public PenguinPhysics StartSlide(Vector2 newSpeed)
        {
            var state = Clone();

            state.Velocity = newSpeed;

            if (newSpeed.LengthSquared() > 0)
            {
                state.Facing = newSpeed;
            }

            return state;
        }
        public PenguinPhysics StartBonk(Vector2 newSpeed)
        {
            var state = Clone();

            state.Velocity = newSpeed;

            if (newSpeed.LengthSquared() > 0)
            {
                state.Facing = -newSpeed;
            }

            return state;
        }

        public PenguinPhysics Slide(float delta)
        {
            var state = Clone();

            state.Position += state.Velocity * delta;

            return state;
        }

        public PenguinPhysics SetFacing(Vector2 facing)
        {
            var state = Clone();

            state.Facing = facing;

            return state;
        }

        private PenguinPhysics Clone()
        {
            return MemberwiseClone() as PenguinPhysics;
        }
    }
}
