﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Input;
using PinguinGame.Player;
using PinguinGame.Screens.Resources;
using PinguinGame.Screens.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Graphics.Fonts;
using TinyGames.Engine.Graphics.Fonts.LoadersAndGenerators;

namespace PinguinGame.Screens
{
    public class SplashScreen : Screen
    {
        private readonly IScreenService _screens;

        private UISplashScreen _ui;

        public float Timer = 0;

        public SplashScreen(IScreenService screens)
        {
            _screens = screens;
        }

        public override void Init(GraphicsDevice device, ContentManager content)
        {
            base.Init(device, content);

            _ui = new UISplashScreen(new SplashResources(content));
            _ui.UpdateLayout(Camera.Bounds);
        }

        public override void UpdateSelf(float delta)
        {
            base.UpdateSelf(delta);

            Timer += delta;

            if(Timer > 1.5f)
            {
                _screens.ShowTitleScreen();
            }

        }

        public override void UpdateAnimation(float delta)
        {
            base.UpdateAnimation(delta);
            _ui.Update(delta);
        }

        public override void Draw()
        {
            base.Draw();

            Graphics.Clear(Color.MistyRose);

            Graphics.Begin(Camera.GetMatrix());

            _ui.Draw(Graphics);

            Graphics.End();
        }
    }
}