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
    public class TitleResources
    {
        public Color BackgroundColor { get; private set; } = new Color(52, 86, 141);
        public Color BackgroundOverlayColor { get; private set; } = new Color(87, 117, 167);
        public Texture2D BackgroundOverlay { get; private set; }

        public Sprite Title { get; private set; }
        public Sprite Penguin { get; private set; }
        public Sprite InsertCoin{ get; private set; }
        public Sprite IceBlock { get; private set; }

        public TitleResources(ContentManager content)
        {
            BackgroundOverlay = content.Load<Texture2D>("Sprites/UI/BackgroundPenguins");
            Title = new Sprite(content.Load<Texture2D>("Sprites/UI/Title")).CenterOrigin();
            Penguin = new Sprite(content.Load<Texture2D>("Sprites/UI/Penguin")).CenterOrigin();
            InsertCoin = new Sprite(content.Load<Texture2D>("Sprites/UI/InsertCoin")).CenterOrigin();
            IceBlock = new Sprite(content.Load<Texture2D>("Sprites/UI/IceBlock")).CenterOrigin();

        }
    }
}
