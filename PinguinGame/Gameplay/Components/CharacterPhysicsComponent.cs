using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Maths;
using TinyGames.Engine.Scenes;

namespace PinguinGame.Gameplay.Components
{

    public class CharacterPhysicsComponent : Component
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; private set; }
        public Vector2 Facing { get; private set; }

        public CharacterPhysicsComponent(Vector2 position)
        {
            Position = position;
        }

        public CharacterPhysicsComponent Move(float delta, Vector2 speed, float acceleration)
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

        public CharacterPhysicsComponent StartSlide(Vector2 newSpeed)
        {
            var state = Clone();

            state.Velocity = newSpeed;

            if (newSpeed.LengthSquared() > 0)
            {
                state.Facing = newSpeed;
            }

            return state;
        }
        public CharacterPhysicsComponent StartBonk(Vector2 newSpeed)
        {
            var state = Clone();

            state.Velocity = newSpeed;

            if (newSpeed.LengthSquared() > 0)
            {
                state.Facing = -newSpeed;
            }

            return state;
        }

        public CharacterPhysicsComponent Slide(float delta, Vector2 direction)
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

        public CharacterPhysicsComponent SetFacing(Vector2 facing)
        {
            var state = Clone();

            state.Facing = facing;

            return state;
        }

        private CharacterPhysicsComponent Clone()
        {
            return MemberwiseClone() as CharacterPhysicsComponent;
        }
    }
}
