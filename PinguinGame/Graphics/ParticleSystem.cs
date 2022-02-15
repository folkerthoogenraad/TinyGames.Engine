using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Scenes;
using TinyGames.Engine.Scenes.Extensions;

namespace PinguinGame.Graphics
{
    public class ParticleSystem : ISceneBehaviour, IDrawable2D
    {
        public List<Particle> Particles;

        public ParticleSystem()
        {
            Particles = new List<Particle>();
        }


        public void Draw(Graphics2D graphics)
        {
            foreach (var particle in Particles)
            {
                // graphics.DrawCircle(particle.Position - new Vector2(0, particle.Height + 8), 1, Color.Red, GraphicsHelper.YToDepth(particle.Position.Y));
                graphics.DrawSprite(particle.Animation.GetSpriteForTime(particle.Timer), particle.Position - new Vector2(0, particle.Height), particle.Angle, GraphicsHelper.YToDepth(particle.Position.Y), particle.Color);
            }
        }

        public void Add(Particle particle)
        {
            Particles.Add(particle);
        }

        // ======================================== // 
        // Scene component stuff
        // ======================================== // 
        public void Init(Scene scene)
        {
            var graphics = scene.GetBehaviour<SceneGraphics>();
            graphics.AddDrawable(this);
        }

        public void BeforeUpdate(float delta)
        {
            // Do nothing
        }

        public void AfterUpdate(float delta)
        {
            Particles.RemoveAll(x => !x.Update(delta));
        }

        public void Destroy()
        {
            // Do nothing
        }
    }
}
