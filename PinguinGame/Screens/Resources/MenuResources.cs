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
    public class MenuResources
    {
        public Color BackgroundColor { get; private set; } = new Color(52, 86, 141);
        public Color BackgroundOverlayColor { get; private set; } = new Color(87, 117, 167);

        public Texture2D BackgroundOverlay { get; private set; }
        public Sprite Title { get; private set; }

        public Font Font { get; private set; }

        public NineSideSprite ButtonUnselected { get; private set; }
        public NineSideSprite ButtonSelected { get; private set; }
        public NineSideSprite ButtonPressed { get; private set; }

        public Sprite PlayIcon { get; private set; }
        public Sprite SettingsIcon { get; private set; }
        public Sprite ExitIcon { get; private set; }


        public MenuResources(ContentManager content)
        {
            BackgroundOverlay = content.Load<Texture2D>("Sprites/UI/BackgroundPenguins");
            Title = new Sprite(content.Load<Texture2D>("Sprites/UI/Title")).CenterOrigin();

            Font = content.LoadFont("Fonts/SansSerifFont");

            var buttonTexture = content.Load<Texture2D>("Sprites/UI/Buttons");

            ButtonSelected = new NineSideSprite(buttonTexture, new Rectangle(0, 0, 16, 16), 6, 6, 6, 6);
            ButtonUnselected = new NineSideSprite(buttonTexture, new Rectangle(0, 16, 16, 16), 6, 6, 6, 6);
            ButtonPressed = new NineSideSprite(buttonTexture, new Rectangle(32, 16, 16, 16), 6, 6, 6, 6);


            PlayIcon = new Sprite(content.Load<Texture2D>("Sprites/UI/MenuIcons"), new Rectangle(0, 0, 16, 16)).CenterOrigin();
            SettingsIcon = new Sprite(content.Load<Texture2D>("Sprites/UI/MenuIcons"), new Rectangle(16, 0, 16, 16)).CenterOrigin();
            ExitIcon = new Sprite(content.Load<Texture2D>("Sprites/UI/MenuIcons"), new Rectangle(0, 16, 16, 16)).CenterOrigin();
        }
    }
}
