
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using TinyGames.Engine.Extensions;
using TinyGames.Engine.Graphics;

namespace EmmaGame
{
    public class Light
    {
        public Vector2 Position { get; set; }
        public Color Color { get; set; }
    }

    public class EmmaGameTest : Game
    {
        private GraphicsDeviceManager _graphics;
        public Graphics2D Graphics { get; set; }
        public Camera Camera { get; set; }
        public Camera LightmapCamera { get; set; }
        public Texture2D TerminalBackground { get; set; }
        public Texture2D TerminalLightsBackground { get; set; }

        public RenderTarget2D LightingRenderTarget { get; set; }
        public Sprite LightOne { get; set; }

        public float PlayerAnimationTimer = 0;
        public Animation PlayerIdle;
        public Animation PlayerWalking;

        public List<Light> Lights { get; set; }

        public float PlayerFacing = 1;
        public Vector2 MousePosition;

        public Vector2 PlayerVelocity;
        public Vector2 PlayerPosition;

        public EmmaGameTest()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Lights = new List<Light>();
        }

        protected override void Initialize()
        {
            base.Initialize();

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();

            Graphics = new Graphics2D(GraphicsDevice);
            Camera = new Camera(180);
            Camera.Position = new Vector2(192, 128) / 2;

            TerminalBackground = Content.Load<Texture2D>("Terminal");
            TerminalLightsBackground = Content.Load<Texture2D>("TerminalLights2");
            LightingRenderTarget = new RenderTarget2D(GraphicsDevice, 192, 128);

            LightmapCamera = new Camera(128, 192.0f / 128.0f);
            LightmapCamera.Position = LightmapCamera.Size / 2;

            LightOne = new Sprite(Content.Load<Texture2D>("LightOne")).CenterOrigin();

            var playerTexture = Content.Load<Texture2D>("JackSheet");
            PlayerIdle = new Animation(
                new Sprite(playerTexture, new Rectangle(0, 0, 16, 32)).SetOrigin(new Vector2(8, 32))
                );
            PlayerWalking = new Animation(
                new Sprite(playerTexture, new Rectangle(0, 32, 16, 32)).SetOrigin(new Vector2(8, 32)),
                new Sprite(playerTexture, new Rectangle(16, 32, 16, 32)).SetOrigin(new Vector2(8, 32)),
                new Sprite(playerTexture, new Rectangle(32, 32, 16, 32)).SetOrigin(new Vector2(8, 32)),
                new Sprite(playerTexture, new Rectangle(48, 32, 16, 32)).SetOrigin(new Vector2(8, 32))
                );

            PlayerPosition = new Vector2(10, 100);
        }

        protected override void LoadContent()
        {
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
        bool pressed = false;
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float delta = gameTime.GetDeltaInSeconds();
            var keyState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            MousePosition = Camera.TransformMousePosition(
                new Vector2(((float)mouseState.X / _graphics.PreferredBackBufferWidth), ((float)mouseState.Y / _graphics.PreferredBackBufferHeight)));


            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (!pressed)
                {
                    var random = new Random();

                    Lights.Add(new Light() { 
                        Position = MousePosition,
                        Color = new Color(random.NextFloat(), random.NextFloat(), random.NextFloat()),
                    });
                }
                pressed = true;
            }
            else
            {
                pressed = false;
            }

            if(mouseState.RightButton == ButtonState.Pressed)
            {
                Lights.Clear();
            }

            PlayerAnimationTimer += delta;

            // Delta stuff
            PlayerVelocity = new Vector2(0, 0);

            if (keyState.IsKeyDown(Keys.Left))
            {
                PlayerVelocity.X += -32;
                PlayerFacing = -1;
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                PlayerVelocity.X += 32;
                PlayerFacing = 1;
            }

            PlayerPosition += PlayerVelocity * delta;
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);


            Graphics.Begin(LightmapCamera.GetMatrix());
            Graphics.Clear(Color.Black);

            Graphics.Push();
            Graphics.SetRenderTarget(LightingRenderTarget);
            Graphics.SetBlendMode(BlendMode.Additive);

            Graphics.DrawRectangle(new Rectangle(0, 0, 192, 128), new Color(0.2f, 0.2f, 0.3f));
            Graphics.DrawTexture(TerminalLightsBackground, new Vector2(0, 0), new Vector2(192, 128));

            foreach (var light in Lights)
            {
                Graphics.DrawSprite(LightOne, light.Position, 0, 0, light.Color);
            }

            Graphics.DrawSprite(LightOne, MousePosition, 0, 0, Color.Red);

            Graphics.SetBlendMode(BlendMode.Normal);
            Graphics.ResetRenderTarget();
            Graphics.Pop();

            Graphics.End();



            Graphics.Begin(Camera.GetMatrix());
            Graphics.Clear(Color.Black);

            Graphics.DrawTexture(TerminalBackground, new Vector2(0, 0), new Vector2(192, 128));

            Vector2 playerScale = new Vector2(PlayerFacing, 1);
            Sprite sprite = PlayerIdle.GetSpriteForTime(PlayerAnimationTimer);

            if(PlayerVelocity.X != 0)
            {
                sprite = PlayerWalking.GetSpriteForTime(PlayerAnimationTimer);
            }

            Graphics.DrawSprite(sprite, PlayerPosition, playerScale, 0, 0, Color.White);

            Graphics.SetBlendMode(BlendMode.Multiply);
            Graphics.DrawTexture(LightingRenderTarget, new Vector2(0, 0), new Vector2(192, 128));
            Graphics.SetBlendMode(BlendMode.Normal);

            Graphics.End();
        }
    }
}
