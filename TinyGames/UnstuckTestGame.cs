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

    public class UnstuckTestGame : Game
    {
        private GraphicsDeviceManager _graphics;

        public Graphics2D Graphics;
        public Camera Camera;

        public Vector2 MousePosition;

        public UnstuckTestGame()
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
            Graphics = new Graphics2D(GraphicsDevice);
            Camera = new Camera(180, 16f / 9f);
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }


        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float delta = gameTime.GetDeltaInSeconds();
            var keyState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            MousePosition = Camera.TransformMousePosition(
                new Vector2(((float)mouseState.X / _graphics.PreferredBackBufferWidth), ((float)mouseState.Y / _graphics.PreferredBackBufferHeight)));
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Graphics.Clear(Color.White);

            Graphics.Begin(Camera.GetMatrix());

            //DrawDebugCircles();
            //DrawDebugAABB();
            //DrawDebugBlockCircle();
            DrawDebugCircleBlock();

            Graphics.End();
        }

        public void DrawDebugCircles()
        {
            Circle circle = new Circle(new Vector2(16, 32), 48);
            Circle mouseCircle = new Circle(MousePosition, 24);

            Graphics.DrawCircle(circle.Position, circle.Radius, Color.Red);

            Graphics.DrawCircle(mouseCircle.Position - Collision.Unstuck(circle, mouseCircle), mouseCircle.Radius, Color.Green);
            Graphics.DrawCircle(mouseCircle.Position, mouseCircle.Radius, Color.Blue);
        }

        public void DrawDebugAABB()
        {
            AABB block = AABB.CreateCentered(new Vector2(16, 32), new Vector2(48, 32));
            AABB mouseBlock = AABB.CreateCentered(MousePosition, new Vector2(16, 24));

            Graphics.DrawRectangle(block, Color.Red);

            Graphics.DrawRectangle(mouseBlock.Translated(-Collision.Unstuck(block, mouseBlock)), Color.Green);
            Graphics.DrawRectangle(mouseBlock, Color.Blue);
        }

        public void DrawDebugBlockCircle()
        {
            AABB block = AABB.CreateCentered(new Vector2(16, 32), new Vector2(48, 32));
            Circle mouseCircle = new Circle(MousePosition, 24);

            Graphics.DrawRectangle(block, Color.Red);

            Graphics.DrawCircle(mouseCircle.Position - Collision.Unstuck(block, mouseCircle), mouseCircle.Radius, Color.Green);
            Graphics.DrawCircle(mouseCircle.Position, mouseCircle.Radius, Color.Blue);
        }

        public void DrawDebugCircleBlock()
        {
            Circle circle = new Circle(new Vector2(16, 32), 48);
            AABB mouseBlock = AABB.CreateCentered(MousePosition, new Vector2(16, 24));

            Graphics.DrawCircle(circle.Position, circle.Radius, Color.Red);

            Graphics.DrawRectangle(mouseBlock.Translated(-Collision.Unstuck(circle, mouseBlock)), Color.Green);
            Graphics.DrawRectangle(mouseBlock, Color.Blue);
        }
    }
}

