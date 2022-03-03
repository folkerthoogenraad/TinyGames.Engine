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

        public IGraphicsService GraphicsService { get; private set; }
        public GraphicsDevice Device => GraphicsService.Device;

        public ContentManager Content;

        public virtual void Init(IGraphicsService graphicsService, ContentManager content)
        {
            GraphicsService = graphicsService;
            Content = content;

            Graphics = new Graphics2D(Device);
            Camera = new Camera(graphicsService.Height, graphicsService.AspectRatio);

        }

        public virtual void Update(float delta)
        {
            UpdateSelf(delta);
            UpdateAnimation(delta);
        }

        public virtual void UpdateSelf(float delta)
        {

        }
        public virtual void UpdateAnimation(float delta)
        {

        }

        public virtual void Draw()
        {

        }

        public virtual void Destroy()
        {

        }

        private int GetApplicableHeight(int width, int height)
        {
            return 180;
        }
    }
}
