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
    public class PlayerSelectResources
    {
        public Color BackgroundColor { get; private set; } = new Color(88, 141, 52);
        public Color BackgroundOverlayColor { get; private set; } = new Color(113, 165, 77);
        public Color[] PlayerColors { get; private set; } = new Color[] { Color.Red, Color.Blue, Color.Yellow, Color.Green }; // TODO load this from somewhere

        public Texture2D BackgroundOverlay { get; private set; }
        public Sprite TitlePlay { get; private set; }
        public Sprite JoinIcon { get; private set; }
        public Sprite PenguinIcon { get; private set; }
        public Sprite PenguinIconOverlay { get; private set; }

        public Font Font { get; private set; }

        public NineSideSprite ButtonUnJoined { get; private set; }
        public NineSideSprite ButtonJoined { get; private set; }
        public NineSideSprite ButtonReady { get; private set; }
        public NineSideSprite ButtonSelected { get; private set; }

        public PlayerSelectResources(ContentManager content)
        {
            BackgroundOverlay = content.Load<Texture2D>("Sprites/UI/BackgroundPenguins");
            TitlePlay = new Sprite(content.Load<Texture2D>("Sprites/UI/TitlePlay")).CenterOrigin();
            JoinIcon = new Sprite(content.Load<Texture2D>("Sprites/UI/JoinIcon")).CenterOrigin();
            PenguinIcon = new Sprite(content.Load<Texture2D>("Sprites/UI/PenguinIcon"), new Rectangle(0, 0, 16, 16)).CenterOrigin();
            PenguinIconOverlay = new Sprite(content.Load<Texture2D>("Sprites/UI/PenguinIcon"), new Rectangle(0, 16, 16, 16)).CenterOrigin();

            Font = content.LoadFont("Fonts/SansSerifFont");

            var buttonTexture = content.Load<Texture2D>("Sprites/UI/Buttons");

            ButtonUnJoined = new NineSideSprite(buttonTexture, new Rectangle(16, 16, 16, 16), 6, 6, 6, 6);
            ButtonJoined = new NineSideSprite(buttonTexture, new Rectangle(32, 0, 16, 16), 6, 6, 6, 6);
            ButtonReady = new NineSideSprite(buttonTexture, new Rectangle(16, 0, 16, 16), 6, 6, 6, 6);

            ButtonSelected = new NineSideSprite(buttonTexture, new Rectangle(0, 0, 16, 16), 6, 6, 6, 6);
        }
    }
}
