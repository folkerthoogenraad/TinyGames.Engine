using System;
using System.Collections.Generic;
using System.Text;

namespace TinyGames.Engine.Animations
{
    public class SpringAnimation
    {
        public float Extension { get; set; } = 0;
        public float SpringConstant { get; set; } = 100;
        public float DamperConstant { get; set; } = 8;
        public float Velocity { get; set; } = 0;

        public void Update(float delta)
        {
            Velocity -= Extension * SpringConstant * delta;
            Velocity -= Velocity * DamperConstant * delta;

            Extension += Velocity * delta;
        }
    }
}
