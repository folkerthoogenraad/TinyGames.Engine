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
        public Sprite Grass0 { get; set; }
        public Sprite Grass1 { get; set; }
        public Sprite Grass2 { get; set; }
        public Sprite Grass3 { get; set; }
        public Sprite GrassShadow { get; set; }
        public Sprite Tree { get; set; }
        public Sprite TreeShadow { get; set; }

        public IceGameGraphics(ContentManager content)
        {
            var texture = content.Load<Texture2D>("Sprites/Ice/IceGameplayElements");

            Bridge = new Sprite(texture, new Rectangle(32, 48, 16, 16));

            Grass0 = new Sprite(texture, new Rectangle(16, 64, 16, 16)).CenterOrigin();
            Grass1 = new Sprite(texture, new Rectangle(32, 64, 16, 16)).CenterOrigin();
            Grass2 = new Sprite(texture, new Rectangle(48, 64, 16, 16)).CenterOrigin();
            Grass3 = new Sprite(texture, new Rectangle(64, 64, 16, 16)).CenterOrigin();
            GrassShadow = new Sprite(texture, new Rectangle(16, 80, 16, 16)).CenterOrigin();

            Tree = new Sprite(texture, new Rectangle(0, 48, 16, 32)).SetOrigin(8, 32);
            TreeShadow = new Sprite(texture, new Rectangle(0, 80, 16, 16)).CenterOrigin();
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
