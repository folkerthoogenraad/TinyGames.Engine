using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Scenes;

namespace PinguinGame.Gameplay.Components
{
    public class HeightComponent : Component
    {
        public float Bouncyness { get; set; } = 0.5f;
        public float GroundHeight { get; set; }
        public float Height { get; set; }
        public float Velocity { get; set; }
        public float Gravity { get; set; } = 128;

        public Vector2 Offset => new Vector2(0, -Height);

        public override void Update(float delta)
        {
            Velocity += Gravity * delta;
            Height -= Velocity * delta;

            if(Height < GroundHeight)
            {
                Height = GroundHeight;
                Velocity = -Velocity * Bouncyness;
            }
        }

        public void ClampPosition()
        {
            if (Height < GroundHeight)
            {
                Height = GroundHeight;
                Velocity = -Velocity * Bouncyness;
            }
        }
    }
}
