using Microsoft.Xna.Framework;
using PinguinGame.Player;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Maths;

namespace PinguinGame.MiniGames.Minecarts
{
    public struct MinecartInput
    {
        public bool JumpLeft;
        public bool JumpRight;

        public bool Boost;
        public bool Brake;
    }

    public class Minecart
    {
        public PlayerInfo Player { get; set; }
        public MinecartGraphics Graphics { get; set; }

        public bool Grounded = true;
        public float Height { get; set; }
        public float Bouncyness { get; set; }
        public float Gravity { get; set; } = 300;
        public float UpVelocity { get; set; }

        public float RailVelocity { get; set; }
        
        public Vector2 Position;

        public float BoostDuration { get; set; } = 0.5f;
        public float BoostDelayBetween { get; set; } = 0.4f;
        public float BoostTimeout { get; set; } = 0;
        public bool IsBoosting => BoostTimeout > 0;
        public bool CanBoost => BoostTimeout < -BoostDelayBetween;

        public float Acceleration { get; set; } = 64;
        public float Friction { get; set; } = 16;

        public float MinSpeed { get; set; } = 16;
        public float MaxSpeed { get; set; } = 96;

        public float XPosition { get; set; }

        public bool OffRoad { get; set; } = false;

        public void Update(float delta, MinecartInput input)
        {
            if (OffRoad)
            {
                UpdateOffRoad(delta);
            }
            else
            {
                if (!Grounded)
                {
                    UpdateJump(delta, input);
                }
                else
                {
                    UpdateGrounded(delta, input);
                }
            }

            UpVelocity -= Gravity * delta;
            Height += UpVelocity * delta;

            if (Height < 0)
            {
                Height = 0;
                UpVelocity = 0;
                Grounded = true;
            }

            Position += new Vector2(0, -RailVelocity * delta);
            Position.X = Tools.Lerp(Position.X, XPosition, delta * 15);
        }

        public void UpdateOffRoad(float delta)
        {
            RailVelocity -= Acceleration * delta;

            if(RailVelocity < 0)
            {
                RailVelocity = 0;
            }
        }

        public void UpdateGrounded(float delta, MinecartInput input)
        {
            if (input.JumpLeft)
            {
                XPosition -= 32;
                UpVelocity = 48;
                Grounded = false;
                return;
            }
            if (input.JumpRight)
            {
                XPosition += 32;
                UpVelocity = 48;
                Grounded = false;
                return;
            }

            float acceleration = 0;

            if (input.Boost)
            {
                acceleration += 1;
            }
            if (input.Brake)
            {
                acceleration -= 1;
            }

            RailVelocity += acceleration * Acceleration * delta;
            RailVelocity -= Friction * delta;

            if(RailVelocity < MinSpeed) RailVelocity = MinSpeed;
            if(RailVelocity > MaxSpeed) RailVelocity = MaxSpeed;
        }

        public void UpdateJump(float delta, MinecartInput input)
        {
        }
    }
}
