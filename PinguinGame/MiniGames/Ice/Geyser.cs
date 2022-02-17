using Microsoft.Xna.Framework;
using PinguinGame.Graphics;
using PinguinGame.Player;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Extensions;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Scenes;

namespace PinguinGame.MiniGames.Ice
{
    [RequireSceneBehaviour(typeof(IceGameEffects))]
    public class Geyser : GameObject
    {
        public Vector2 Position { get; set; }
        public ParticleSystem System { get; set; }
        public Animation Particle { get; set; }

        public Geyser(Vector2 position)
        {
            Position = position;
        }

        private float Timer = 0;
        private float ParticleTimer = 0;

        public bool Erupting => Timer % 8 > 4;

        public override void Init()
        {
            base.Init();

            System = Scene.GetBehaviour<ParticleSystem>();

            Particle = Scene.GetBehaviour<IceGameEffects>().GeyserParticles;
        }

        public override void Update(float delta)
        {
            Timer += delta;

            if (Erupting)
            {
                ParticleTimer += delta;

                if(ParticleTimer > 0.05f)
                {
                    ParticleTimer -= 0.05f;

                    System.Add(new Particle()
                    {
                        Animation = Particle.SetFrameRate(12),
                        Position = Position,
                        Height = 8,
                        HeightVelocity = 64,
                        Velocity = new Random().NextPointInCircle() * 16
                    });
                }
            }
        }
    }
}
