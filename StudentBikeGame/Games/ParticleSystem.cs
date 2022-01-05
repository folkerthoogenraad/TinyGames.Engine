using System;
using System.Collections.Generic;
using System.Linq;
using TinyGames.Engine.Graphics;

namespace StudentBikeGame.Games
{
    public class ParticleSystem
    {
        public List<Particle> Particles;

        public ParticleSystem()
        {
            Particles = new List<Particle>();
        }
        public void Update(float delta)
        {
            Particles = Particles.Where(x => x.Update(delta)).ToList();
        }

        public void Draw(Graphics2D graphics)
        {
            foreach(var particle in Particles)
            {
                graphics.DrawSprite(particle.Animation.GetSpriteForTime(particle.Timer), particle.Position, particle.Angle, 0, particle.Color);
            }
        }

        public void Add(Particle particle)
        {
            Particles.Add(particle);
        }
    }
}
