using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TinyGames.Engine.Extensions;
using TinyGames.Engine.Graphics;

using System.Linq;
using TinyGames.Engine.Maths;

namespace TinyGames
{
    public class TinyGames : Game
    {
        private GraphicsDeviceManager _graphics;

        public Graphics2D Graphics;
        public Camera Camera;

        public Sprite Ground;

        public Sprite Lander;
        public Sprite BrokenLander;

        public Sprite UIGameOver;
        public Sprite UIDone;

        public Vector2 Position;
        public Vector2 Velocity;
        public float GroundHeight = 300;

        public float Gravity = 125;
        public float Force = 250;
        public float Radius = 8;
        public float Bouncyness = 0.5f;

        public float MaxFuel = 200;
        public float Fuel = 200;
        public float FuelBurnedPerSecond = 60;

        public bool Broken = false;
        public bool Done = false;

        public int Level = 1;

        public SpriteNumbers SpriteNumbers;

        private Vector2[] Stars;

        public Color ForegroundColor = Color.White;
        public Color BackgroundColor = new Color(15, 24, 32);
        public Color AccentColor = Color.Red;

        public float AngularVelocity = 0;
        public float Angle = 0;

        public TinyGames()
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

        }

        protected override void LoadContent()
        {
            var effect = Content.Load<Effect>("Effects/StandardEffect");
            var sheet = Content.Load<Texture2D>("Sprites/Sheet");

            Graphics = new Graphics2D(GraphicsDevice, effect);

            Camera = new Camera(360, 16f / 9f);
            Camera.Position = new Vector2(0, GroundHeight - Camera.Height / 2 + 32);

            Lander = new Sprite(sheet, new Rectangle(0, 0, 16, 16));
            Ground = new Sprite(sheet, new Rectangle(0, 16, 16, 16));
            BrokenLander = new Sprite(sheet, new Rectangle(16, 0, 16, 16));

            Lander.CenterOrigin();
            BrokenLander.CenterOrigin();
            
            UIDone = new Sprite(sheet, new Rectangle(0, 48, 64, 16));
            UIGameOver = new Sprite(sheet, new Rectangle(0, 32, 64, 16));

            UIDone.CenterOrigin();
            UIGameOver.CenterOrigin();

            SpriteNumbers = new SpriteNumbers(sheet, new Point(0, 112), new Point(5, 11), new Point(6, 0));

            var bounds = Camera.Bounds;
            var random = new Random();

            Stars = new Vector2[64].Select(x => bounds.TopLeft + new Vector2(bounds.Width * (float)random.NextDouble(), bounds.Height * (float)random.NextDouble())).ToArray();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            float delta = gameTime.GetDeltaInSeconds();
            var keyState = Keyboard.GetState();

            Velocity += new Vector2(0, Gravity) * delta;

            // Angular drag
            Angle = Tools.Lerp(Angle, 0, delta);
            AngularVelocity = Tools.Lerp(AngularVelocity, 0, delta * 5);

            if (!Broken && Fuel > 0)
            {
                int xDir = 0;
                bool bursting = false;
                float magnitude = 0;

                if (keyState.IsKeyDown(Keys.Left))
                {
                    bursting = true;
                    xDir -= 1;
                    magnitude += 0.5f;
                }
                if (keyState.IsKeyDown(Keys.Right))
                {
                    bursting = true;
                    xDir += 1;
                    magnitude += 0.5f;
                }

                if (bursting)
                {
                    var direction = new Vector2(0, -1).Rotated(Angle);


                    AngularVelocity += xDir * 10 * delta;
                    Velocity += direction * magnitude * Force * delta;
                    Fuel -= delta * FuelBurnedPerSecond * magnitude;
                }
            }
            if (keyState.IsKeyDown(Keys.Enter))
            {
                NextLevel();
            }

            Position += Velocity * delta;
            Angle += AngularVelocity * delta;

            Camera.Position.X = Tools.Lerp(Camera.Position.X, Position.X, delta * 20);

            if(Position.Y + Radius > GroundHeight)
            {
                AngularVelocity = 0;

                if (MathF.Abs(Angle) < MathF.PI / 8)
                {
                    Angle = 0;
                }
                else
                {
                    Broken = true;
                }

                if(Velocity.Y > 80)
                {
                    Broken = true;
                }

                if (Velocity.Y < 10)
                {
                    Done = true;
                }

                Position.Y = GroundHeight - Radius;
                Velocity.Y = -Velocity.Y * Bouncyness;

                Velocity.X = Velocity.X * Bouncyness;
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Graphics.Clear(BackgroundColor);

            Graphics.Begin(Camera.GetMatrix());

            // Stars
            foreach (var star in Stars)
            {
                Graphics.DrawRectangle(star, new Vector2(1, 1), Color.White);
            }

            // Draw sprite
            Graphics.DrawSprite(Broken ? BrokenLander : Lander, Position, Angle * Tools.RadToDeg);

            // Draw ground
            for(int i = -10; i < 10; i++)
            {
                Graphics.DrawSprite(Ground, new Vector2(i * 16, GroundHeight));
            }

            // UI shit
            var fuelBounds = new AABB()
            {
                Left = Position.X - 4,
                Right = Position.X + 4,
                Top = Position.Y - 32,
                Bottom = Position.Y - 16,
            };
            var barBounds = fuelBounds.Clone().Shrink(1);

            Graphics.DrawRectangle(fuelBounds, ForegroundColor);
            Graphics.DrawRectangle(barBounds, BackgroundColor);

            barBounds.Shrink(1);

            var fuelPercentage = Fuel / MaxFuel;

            var fillBounds = new AABB()
            {
                Left = barBounds.Left,
                Right = barBounds.Right,

                Top = barBounds.Top + barBounds.Height - barBounds.Height * fuelPercentage,
                Bottom = barBounds.Bottom
            };
            Graphics.DrawRectangle(fillBounds, fuelPercentage < 0.3f ? AccentColor : ForegroundColor);


            // Numbers and shit
            var bounds = Camera.Bounds;

            Vector2 size = SpriteNumbers.Measure(Level);
            SpriteNumbers.DrawNumber(Graphics, Level, bounds.TopCenter - size / 2 + new Vector2(0, 16));

            if (Done)
            {
                Graphics.DrawSprite(Broken ? UIGameOver : UIDone, bounds.Center, new Vector2(2, 2));
            }

            Graphics.End();

            base.Draw(gameTime);
        }

        public void NextLevel()
        {
            if (!Done) return;

            if (Broken)
            {
                Level = 1;
            }
            else
            {
                Level++;
            }

            Fuel = MaxFuel;
            Broken = false;
            Position = new Vector2();
            Velocity = new Vector2();
            Done = false;
        }
    }
}
