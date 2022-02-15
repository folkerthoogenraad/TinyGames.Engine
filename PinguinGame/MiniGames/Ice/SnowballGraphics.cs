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
    public class SnowballGraphics : ISceneBehaviour
    {
        public Sprite Sprite;
        public Sprite Shadow;

        public Animation SplashAnimation;

        public SnowballGraphics(ContentManager content)
        {
            var texture = content.Load<Texture2D>("Sprites/Ice/IceGameplayElements");

            Sprite = new Sprite(texture, new Rectangle(8, 0, 8, 8)).CenterOrigin();
            Shadow = new Sprite(texture, new Rectangle(8, 8, 8, 8)).CenterOrigin();

            SplashAnimation = new Animation(
                new Sprite(texture, new Rectangle(64 + 0, 0, 16, 16)).CenterOrigin(),
                new Sprite(texture, new Rectangle(64 + 16, 0, 16, 16)).CenterOrigin(),
                new Sprite(texture, new Rectangle(64 + 32, 0, 16, 16)).CenterOrigin(),
                new Sprite(texture, new Rectangle(64 + 48, 0, 16, 16)).CenterOrigin());
        }

        // ======================================== // 
        // Scene component stuff
        // ======================================== // 
        public void AfterUpdate(float delta)
        {
        }

        public void BeforeUpdate(float delta)
        {
        }

        public void Destroy()
        {
        }

        public void Init(Scene scene)
        {
        }
    }
}
