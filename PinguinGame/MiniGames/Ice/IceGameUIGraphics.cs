using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace PinguinGame.MiniGames.Ice
{
    public class IceGameUIGraphics
    {
        public Sprite Heart { get; set; }
        public Sprite HeartOutline { get; set; }
        public Sprite Indicator { get; set; }
        public Sprite IndicatorOutline { get; set; }

        public Animation SnowballCharge { get; set; }
        public Sprite SnowballChargeOutline { get; set; }

        public IceGameUIGraphics(ContentManager content)
        {
            var texture = content.Load<Texture2D>("Sprites/UI/InGameUI");
            Heart = new Sprite(texture, new Rectangle(0, 0, 16, 16)).CenterOrigin();
            HeartOutline = new Sprite(texture, new Rectangle(16, 0, 16, 16)).CenterOrigin();
            Indicator = new Sprite(texture, new Rectangle(0, 16, 16, 16)).CenterOrigin();
            IndicatorOutline = new Sprite(texture, new Rectangle(16, 16, 16, 16)).CenterOrigin();

            SnowballCharge = new Animation(
                new Sprite(texture, new Rectangle(0, 32, 16, 16)).CenterOrigin(),
                new Sprite(texture, new Rectangle(16, 32, 16, 16)).CenterOrigin(),
                new Sprite(texture, new Rectangle(32, 32, 16, 16)).CenterOrigin(),
                new Sprite(texture, new Rectangle(48, 32, 16, 16)).CenterOrigin()
                );
            SnowballChargeOutline = new Sprite(texture, new Rectangle(0, 48, 16, 16)).CenterOrigin();
        }
    }
}
