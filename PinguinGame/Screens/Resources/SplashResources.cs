using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Graphics.Fonts;
using TinyGames.Engine.Graphics.Fonts.LoadersAndGenerators;

namespace PinguinGame.Screens.Resources
{
    public class SplashResources
    {
        public Color BackgroundColor { get; private set; } = Color.Black;
        public Sprite JustFGames { get; private set; }

        public SplashResources(ContentManager content)
        {
            JustFGames = new Sprite(content.Load<Texture2D>("Sprites/UI/JustFGames")).CenterOrigin();
        }
    }
}
