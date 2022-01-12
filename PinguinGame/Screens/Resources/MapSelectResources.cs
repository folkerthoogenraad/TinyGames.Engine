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
    public class MapSelectResources
    {
        public Color BackgroundColor { get; private set; } = new Color(88, 141, 52);
        public Color BackgroundOverlayColor { get; private set; } = new Color(113, 165, 77);
        public Texture2D BackgroundOverlay { get; private set; }

        public Sprite TitleMap { get; private set; }
        public Sprite LevelSelected { get; private set; }
        public Sprite LevelUnselected { get; private set; }

        public Font Font { get; private set; }

        public NineSideSprite ButtonUnselected { get; private set; }
        public NineSideSprite ButtonSelected { get; private set; }
        public NineSideSprite ButtonPressed { get; private set; }

        public MapSelectResources(ContentManager content)
        {
            BackgroundOverlay = content.Load<Texture2D>("Sprites/UI/BackgroundPenguins");

            TitleMap = new Sprite(content.Load<Texture2D>("Sprites/UI/TitleMap")).CenterOrigin();
            LevelSelected = new Sprite(content.Load<Texture2D>("Sprites/UI/ButtonLevelSelect"), new Rectangle(0, 0, 32, 32)).CenterOrigin();
            LevelUnselected = new Sprite(content.Load<Texture2D>("Sprites/UI/ButtonLevelSelect"), new Rectangle(0, 32, 32, 32)).CenterOrigin();

            Font = content.LoadFont("Fonts/SansSerifFont");

            var buttonTexture = content.Load<Texture2D>("Sprites/UI/Buttons");

            ButtonSelected = new NineSideSprite(buttonTexture, new Rectangle(0, 0, 16, 16), 6, 6, 6, 6);
            ButtonUnselected = new NineSideSprite(buttonTexture, new Rectangle(0, 16, 16, 16), 6, 6, 6, 6);
            ButtonPressed = new NineSideSprite(buttonTexture, new Rectangle(32, 16, 16, 16), 6, 6, 6, 6);

        }
    }
}
