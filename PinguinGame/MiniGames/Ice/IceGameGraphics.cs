using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Scenes;

namespace PinguinGame.MiniGames.Ice
{
    public class IceGameGraphics : ISceneBehaviour
    {
        public Sprite Bridge { get; set; }
        public Sprite[] Grass { get; set; }

        public Sprite Tree { get; set; }
        public Sprite TreeShadow { get; set; }


        public Sprite Snowball { get; set; }
        public Sprite SnowballShadow { get; set; }
        public Animation SnowballSplashAnimation { get; set; }

        public IceGameGraphics(ContentManager content)
        {
            var texture = content.Load<Texture2D>("Sprites/Ice/IceGameplayElements");

            Bridge = new Sprite(texture, new Rectangle(32, 48, 16, 16));

            Grass = new Sprite[] {
                new Sprite(texture, new Rectangle(16, 64, 16, 16)).CenterOrigin(),
                new Sprite(texture, new Rectangle(32, 64, 16, 16)).CenterOrigin(),
                new Sprite(texture, new Rectangle(48, 64, 16, 16)).CenterOrigin(),
                new Sprite(texture, new Rectangle(64, 64, 16, 16)).CenterOrigin()
            };

            Tree = new Sprite(texture, new Rectangle(0, 48, 16, 32)).SetOrigin(8, 32);
            TreeShadow = new Sprite(texture, new Rectangle(0, 80, 16, 16)).CenterOrigin();

            Snowball = new Sprite(texture, new Rectangle(8, 0, 8, 8)).CenterOrigin();
            SnowballShadow = new Sprite(texture, new Rectangle(8, 8, 8, 8)).CenterOrigin();

            SnowballSplashAnimation = new Animation(
                new Sprite(texture, new Rectangle(64 + 0, 0, 16, 16)).CenterOrigin(),
                new Sprite(texture, new Rectangle(64 + 16, 0, 16, 16)).CenterOrigin(),
                new Sprite(texture, new Rectangle(64 + 32, 0, 16, 16)).CenterOrigin(),
                new Sprite(texture, new Rectangle(64 + 48, 0, 16, 16)).CenterOrigin());

        }

        // =========================================== //
        // Scene component
        // =========================================== //
        public void Init(Scene scene)
        {
        }

        public void BeforeUpdate(float delta)
        {
        }

        public void AfterUpdate(float delta)
        {
        }

        public void Destroy()
        {
        }
    }
    public static class IceGameGraphicsExtensions
    {
        public static IceGameGraphics GetIceGameGraphics(this Scene scene) => scene.GetBehaviour<IceGameGraphics>();
    }
}
