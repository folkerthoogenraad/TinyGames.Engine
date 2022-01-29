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
    public class InGameResources
    {
        public Sprite Three { get; private set; }
        public Sprite Two { get; private set; }
        public Sprite One { get; private set; }
        public Sprite Go { get; private set; }

        public Font Font { get; private set; }


        public InGameResources(ContentManager content)
        {
            var texture = content.Load<Texture2D>("Sprites/UI/InGameCountdown");

            One = new Sprite(texture, new Rectangle(0,0, 64,64)).CenterOrigin();
            Two = new Sprite(texture, new Rectangle(64,0, 64,64)).CenterOrigin();
            Three = new Sprite(texture, new Rectangle(128,0, 64,64)).CenterOrigin();
            Go = new Sprite(texture, new Rectangle(0,64, 128,64)).CenterOrigin();

            Font = content.LoadFont("Fonts/SansSerifFont");
        }
    }
}
