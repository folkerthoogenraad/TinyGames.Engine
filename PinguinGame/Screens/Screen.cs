﻿using Microsoft.Xna.Framework.Content;
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

        public GraphicsDevice Device;
        public ContentManager Content;

        public virtual void Init(GraphicsDevice device, ContentManager content)
        {
            Graphics = new Graphics2D(device);
            
            float aspect = device.PresentationParameters.BackBufferWidth / (float)device.PresentationParameters.BackBufferHeight;
            Camera = new Camera(GetApplicableHeight(device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight), aspect);

            Content = content;
            Device = device;
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
