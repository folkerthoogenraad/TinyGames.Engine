using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TinyGames.Engine.Maths;

namespace StudentBikeGame.Games.Bike
{
    public class Bike
    {
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }
        public Vector2 Heading { get; private set; }

        public BikeHandling Handling { get; }

        public Bike(BikeHandling handling)
        {
            Handling = handling;
            Heading = new Vector2(1, 0);
        }


        public Bike Update(float delta, float acceleration, float rotationSpeed, SurfaceHandling surface)
        {
            Bike bike = new Bike(Handling);

            // Set the new heading
            bike.Heading = Heading.Rotated(rotationSpeed * delta);

            // Set the position
            bike.Position = Position;
            bike.Velocity = Velocity;

            float speed = bike.Velocity.Length();

            if (speed > 0)
            {
                Vector2 dragDirection = bike.Velocity / speed;

                bike.Velocity -= dragDirection * speed * speed * delta * Handling.AirResistance * surface.ResistanceMultiplier;

            }

            float latheralVelocity = LatheralVelocity;

            float latheralForce = -latheralVelocity;

            if (ForwardVelocity > 1)
            {
                float slipAngle = MathF.Atan2(latheralVelocity, ForwardVelocity) * Tools.RadToDeg;

                latheralForce = -slipAngle / 20 * surface.SteepnessMuliplier;
            }

            Vector2 force = new Vector2(latheralForce, acceleration * delta);

            if (force.Length() > Handling.Grip * surface.GripMultiplier)
            {
                force.Normalize();
                force *= Handling.Grip * surface.GripMultiplier;
            }

            bike.Velocity += Right * force.X;
            bike.Velocity += Forward * force.Y;

            // Update the position with the new velocity
            bike.Position += Velocity * delta;

            return bike;
        }

        public Vector2 Forward => Heading;
        public Vector2 Right => Heading.Perpendicular();
        public float LatheralVelocity => Vector2.Dot(Velocity, Right);
        public float ForwardVelocity => Vector2.Dot(Velocity, Forward);
    }
}
