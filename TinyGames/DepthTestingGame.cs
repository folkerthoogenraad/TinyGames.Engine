using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TinyGames.Engine.Extensions;
using TinyGames.Engine.Graphics;

using System.Linq;
using TinyGames.Engine.Maths;
using TinyGames.Engine.Collisions;
using TinyGames.Engine.Collisions.Contracts;
using System.Collections.Generic;

namespace TinyGames
{
    public class Coin
    {
        public float Phase = 0;
        public float Depth = 0;
        public float Angle = 0;

        public Vector2 Position;
        public Vector2 Velocity;

        public Color Color;
    }

    public class DepthTestingGame : Game
    {
        private GraphicsDeviceManager _graphics;

        public Graphics2D Graphics;
        public Camera Camera;

        public Sprite Sprite;

        public World World;

        public List<Coin> Coins;

        public DepthTestingGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            // graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

            Coins = new List<Coin>();

            var random = new Random();

            const float size = 80;


            for (int i = 0; i < 100; i++)
            {
                Coins.Add(new Coin() { 
                    Phase = random.NextFloat(),
                    Depth = 0,
                    Position = new Vector2(random.NextFloatNormalized(), random.NextFloatNormalized()) * size,
                    Velocity = new Vector2(random.NextFloatNormalized(), random.NextFloatNormalized()) * size * 0.3f,
                    Color = new Color(random.NextFloat(), random.NextFloat(), random.NextFloat()),
                    Angle = random.NextFloat() * 360,
                });
            }

        }

        protected override void LoadContent()
        {
            var effect = Content.Load<Effect>("Effects/StandardEffect");
            var texture = Content.Load<Texture2D>("Sprites/Sheet");

            Graphics = new Graphics2D(GraphicsDevice, new AlphaTestEffect(GraphicsDevice));
            World = new World();

            Camera = new Camera(180, 16f / 9f);

            Sprite = new Sprite(texture, new Rectangle(48, 0, 16, 16));

            Sprite.CenterOrigin();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }


        protected override void Update(GameTime gameTime)
        {
            float delta = gameTime.GetDeltaInSeconds();
            var keyState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            const float size = 80;

            foreach(var coin in Coins)
            {
                coin.Position += coin.Velocity * delta;

                coin.Phase += delta * 0.2f;
                coin.Depth = (float)Math.Sin(coin.Phase * Math.PI * 2);

                coin.Angle += (coin.Velocity.X * 10 + (1 - (coin.Depth * 0.5f + 0.5f)) * coin.Velocity.X * 10) * delta;

                // Keep within bounds
                if (coin.Position.X < -size && coin.Velocity.X < 0) coin.Velocity.X = -coin.Velocity.X;
                if (coin.Position.X > size && coin.Velocity.X > 0) coin.Velocity.X = -coin.Velocity.X;

                if (coin.Position.Y < -size && coin.Velocity.Y < 0) coin.Velocity.Y = -coin.Velocity.Y;
                if (coin.Position.Y > size && coin.Velocity.Y > 0) coin.Velocity.Y = -coin.Velocity.Y;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Graphics.Clear(Color.White);

            Graphics.Begin(Camera.GetMatrix());

            foreach(var coin in Coins)
            {
                Graphics.DrawSprite(Sprite, coin.Position, new Vector2(coin.Depth + 1, coin.Depth + 1), coin.Angle, coin.Depth, coin.Color);
            }

            Graphics.End();

            base.Draw(gameTime);
        }
    }
}
