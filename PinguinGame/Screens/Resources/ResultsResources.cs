﻿using Microsoft.Xna.Framework;
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
    public class ResultsResources
    {
        public Color BackgroundColor { get; private set; } = new Color(87,17,17);
        public Color BackgroundOverlayColor { get; private set; } = new Color(68, 14, 14);

        public Texture2D BackgroundOverlay { get; private set; }

        public Sprite TitleResults { get; private set; }
        public Sprite Goblet { get; private set; }
        public Sprite Banner { get; private set; }
        public Sprite BannerOverlay { get; private set; }

        public Font Font { get; private set; }


        public NineSideSprite ButtonSelected { get; private set; }
        public NineSideSprite ButtonPressed { get; private set; }

        public NineSideSprite ResultLine { get; private set; }
        public NineSideSprite ResultLineOutline { get; private set; }


        public ResultsResources(ContentManager content)
        {
            BackgroundOverlay = content.Load<Texture2D>("Sprites/UI/BackgroundPenguins");
            TitleResults = new Sprite(content.Load<Texture2D>("Sprites/UI/TitleResults")).CenterOrigin();

            Goblet = new Sprite(content.Load<Texture2D>("Sprites/UI/WinGoblet")).CenterOrigin();
            Banner = new Sprite(content.Load<Texture2D>("Sprites/UI/WinBanner"), new Rectangle(0, 0, 256, 64)).SetOrigin(128, 29);
            BannerOverlay = new Sprite(content.Load<Texture2D>("Sprites/UI/WinBanner"), new Rectangle(0, 64, 256, 64)).SetOrigin(128, 29);

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

        }
    }
}
