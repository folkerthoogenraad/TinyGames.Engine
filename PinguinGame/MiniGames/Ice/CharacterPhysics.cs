using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Maths;

namespace PinguinGame.MiniGames.Ice
{

    public class CharacterPhysics
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; private set; }
        public Vector2 Facing { get; private set; }

        public CharacterPhysics(Vector2 position)
        {
            Position = position;
        }

        public CharacterPhysics Move(float delta, Vector2 speed, float acceleration)
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

        public CharacterPhysics StartSlide(Vector2 newSpeed)
        {
            var state = Clone();

            state.Velocity = newSpeed;

            if (newSpeed.LengthSquared() > 0)
            {
                state.Facing = newSpeed;
            }

            return state;
        }
        public CharacterPhysics StartBonk(Vector2 newSpeed)
        {
            var state = Clone();

            state.Velocity = newSpeed;

            if (newSpeed.LengthSquared() > 0)
            {
                state.Facing = -newSpeed;
            }

            return state;
        }

        public CharacterPhysics Slide(float delta, Vector2 direction)
        {
            var state = Clone();

            if(direction.LengthSquared() > 0)
            {
                var wantedAngle = direction.GetAngle();
                var angle = state.Velocity.GetAngle();

                var speed = state.Velocity.Length();

                var newAngle = Tools.AngleLerp(angle, wantedAngle, delta);

                state.Velocity = Tools.AngleVector(newAngle) * speed;
            }


            state.Facing = state.Velocity;
            state.Position += state.Velocity * delta;

            return state;
        }

        public CharacterPhysics SetFacing(Vector2 facing)
        {
            var state = Clone();

            state.Facing = facing;

            return state;
        }

        private CharacterPhysics Clone()
        {
            return MemberwiseClone() as CharacterPhysics;
        }
    }
}
