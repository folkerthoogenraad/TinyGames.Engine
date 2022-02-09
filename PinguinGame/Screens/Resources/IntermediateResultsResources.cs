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
    public class IntermediateResultsResources
    {
        public Sprite TitleResults { get; private set; }

        public Font Font { get; private set; }

        public NineSideSprite ButtonSelected { get; private set; }
        public NineSideSprite ButtonPressed { get; private set; }

        public NineSideSprite ResultLine { get; private set; }
        public NineSideSprite ResultLineOutline { get; private set; }

        public Color BackgroundColor { get; set; } = new Color(0, 0, 0, 128);


        public IntermediateResultsResources(ContentManager content)
        {
            Font = content.LoadFont("Fonts/SansSerifFont");

            var buttonTexture = content.Load<Texture2D>("Sprites/UI/Buttons");

            ButtonSelected = new NineSideSprite(buttonTexture, 
                new Rectangle(0, 0, 16, 16), 6, 6, 6, 6);
            ButtonPressed = new NineSideSprite(buttonTexture, 
                new Rectangle(32, 16, 16, 16), 6, 6, 6, 6);

            ResultLine = new NineSideSprite(buttonTexture,
                new Rectangle(0, 32, 16, 16), 6, 6, 6, 6);
            ResultLineOutline = new NineSideSprite(buttonTexture,
                new Rectangle(16, 32, 16, 16), 6, 6, 6, 6);

            TitleResults = new Sprite(content.Load<Texture2D>("Sprites/UI/TitleResults")).CenterOrigin();

        }
    }
}
