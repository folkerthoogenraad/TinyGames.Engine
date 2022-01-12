using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TinyGames.Engine.Extensions;
using TinyGames.Engine.Graphics;

using System.Linq;
using TinyGames.Engine.Maths;
using TinyGames.Engine.Collisions;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using TinyGames.Engine.Animations;

namespace TinyGames
{
    public class AnimationTestGame : Game
    {
        private GraphicsDeviceManager _graphics;

        public Graphics2D Graphics;
        public Camera Camera;

        private float Timer = 0;
        private float EaseTimer = 0;

        private BounceAnimation Bounce;
        private SpringAnimation Spring;

        public AnimationTestGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            // graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

        }

        protected override void LoadContent()
        {
            Graphics = new Graphics2D(GraphicsDevice);
            Camera = new Camera(360, 16.0f / 9.0f);

            Bounce = new BounceAnimation();
            Spring = new SpringAnimation();
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

            EaseTimer += 2 * delta;
            Timer += 0.5f * delta;

            Bounce.Update(delta);
            Spring.Update(delta);

            if(EaseTimer > 1)
            {
                EaseTimer = 1;
            }

            if (keyState.IsKeyDown(Keys.Space))
            {
                Bounce.Height = Bounce.MaxHeight;
                Spring.Extension = 1;
                EaseTimer = 0;
            }

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            Graphics.Clear(Color.White);

            Graphics.Begin(Camera.GetMatrix());
            
            var up = new Vector2(0, -64);
            var position1 = new Vector2(-64, 0);
            var position2 = new Vector2(-32, 0);
            var position3 = new Vector2(32, 0);
            var position4 = new Vector2(64, 0);

            Graphics.DrawLine(position1, position4, 1, 0, Color.Green);

            Graphics.DrawCircle(position1 + up * Wave.Sine(Timer), 4, Color.Red);
            Graphics.DrawCircle(position2 + up * Wave.Triangle(Timer), 4, Color.Red);
            Graphics.DrawCircle(position3 + up * Wave.Sawtooth(Timer), 4, Color.Red);
            Graphics.DrawCircle(position4 + up * Wave.Parabola(Timer), 4, Color.Red);

            Graphics.DrawCircle(position1 + up * Bounce.Height, 4, Color.Blue);
            Graphics.DrawCircle(position2 + up * Spring.Extension, 4, Color.Blue);

            Graphics.DrawCircle(position1 + up * Ease.Linear(EaseTimer), 4, Color.Orange);
            Graphics.DrawCircle(position2 + up * Ease.EaseInOut(EaseTimer), 4, Color.Orange);
            Graphics.DrawCircle(position3 + up * Ease.EaseIn(EaseTimer), 4, Color.Orange);
            Graphics.DrawCircle(position4 + up * Ease.EaseOut(EaseTimer), 4, Color.Orange);

            Graphics.End();

            base.Draw(gameTime);
        }
    }
}
