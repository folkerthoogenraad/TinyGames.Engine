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
    public class CharacterSelectResources
    {
        public Color BackgroundColor { get; private set; } = new Color(52, 86, 141);
        public Color BackgroundOverlayColor { get; private set; } = new Color(87, 117, 167);
        public Texture2D BackgroundOverlay { get; private set; }

        public Sprite TitleCharacterSelect { get; private set; }

        public Font Font { get; private set; }

        public NineSideSprite ButtonOutline { get; private set; }
        public NineSideSprite ButtonUnselected { get; private set; }
        public NineSideSprite ButtonSelected { get; private set; }
        public NineSideSprite ButtonPressed { get; private set; }



        public CharacterSelectResources(ContentManager content)
        {
            BackgroundOverlay = content.Load<Texture2D>("Sprites/UI/BackgroundPenguins");
            TitleCharacterSelect = new Sprite(content.Load<Texture2D>("Sprites/UI/TitleCharacterSelect")).CenterOrigin();

            Font = content.LoadFont("Fonts/SansSerifFont");

            var buttonTexture = content.Load<Texture2D>("Sprites/UI/Buttons");

            ButtonOutline = new NineSideSprite(buttonTexture, new Rectangle(48, 0, 16, 16), 6, 6, 6, 6);

            ButtonSelected = new NineSideSprite(buttonTexture, new Rectangle(0, 0, 16, 16), 6, 6, 6, 6);
            ButtonUnselected = new NineSideSprite(buttonTexture, new Rectangle(0, 16, 16, 16), 6, 6, 6, 6);
            ButtonPressed = new NineSideSprite(buttonTexture, new Rectangle(32, 16, 16, 16), 6, 6, 6, 6);
        }
    }
}
