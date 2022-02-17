using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;

namespace PinguinGame.MiniGames.Ice
{
    public class IceGameEffects
    {
        public Animation StunEffect { get; set; }
        public Animation GeyserParticles { get; set; }

        public IceGameEffects(ContentManager content)
        {
            var texture = content.Load<Texture2D>("Sprites/Effects");

            StunEffect = new Animation(
                new Sprite(texture, new Rectangle(64 + 0, 0, 16, 16)).CenterOrigin(),
                new Sprite(texture, new Rectangle(64 + 16, 0, 16, 16)).CenterOrigin(),
                new Sprite(texture, new Rectangle(64 + 32, 0, 16, 16)).CenterOrigin(),
                new Sprite(texture, new Rectangle(64 + 48, 0, 16, 16)).CenterOrigin()
                );

            GeyserParticles = new Animation(
                new Sprite(texture, new Rectangle(0, 48, 16, 32)).SetOrigin(8, 32),
                new Sprite(texture, new Rectangle(16, 48, 16, 32)).SetOrigin(8, 32),
                new Sprite(texture, new Rectangle(32, 48, 16, 32)).SetOrigin(8, 32),
                new Sprite(texture, new Rectangle(48, 48, 16, 32)).SetOrigin(8, 32)
                ).SetFrameRate(2);
        }
    }
}
