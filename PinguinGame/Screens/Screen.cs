using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace PinguinGame.Screens
{
    public class Screen
    {
        public Camera Camera { get; set; }
        public Graphics2D Graphics { get; set; }

        public virtual void Init(GraphicsDevice device, ContentManager content)
        {
            Graphics = new Graphics2D(device);
            Camera = new Camera(360, 16.0f / 9.0f);
        }

        public virtual void Update(float delta)
        {

        }

        public virtual void Draw()
        {

        }

        public virtual void Destroy()
        {

        }
    }
}
