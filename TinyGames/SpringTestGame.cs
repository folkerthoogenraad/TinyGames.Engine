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
    public class Point
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public bool Pinned { get; set; }
        public List<Point> Connections { get; set; }

        public Point(Vector2 position, bool pinned = false)
        {
            Position = position;
            Connections = new List<Point>();
            Pinned = pinned;
        }

        public Point AddConnection(Point other, bool addToOther = true)
        {
            Connections.Add(other);

            if (addToOther)
            {
                other.AddConnection(this, false);
            }

            return this;
        }
    }

    public class SpringTestGame : Game
    {
        private GraphicsDeviceManager _graphics;

        public Graphics2D Graphics;
        public Camera Camera;

        Vector2 MousePosition;
        Point SelectedPoint = null;

        public List<Point> Points;

        public SpringTestGame()
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

            var p1 = new Point(new Vector2(0, -64));
            var p2 = new Point(new Vector2(0, 0));
            var p3 = new Point(new Vector2(16, 16));

            p1.AddConnection(p2);
            p2.AddConnection(p3);
        }

        protected override void LoadContent()
        {
            Graphics = new Graphics2D(GraphicsDevice);
            Camera = new Camera(360, 16f / 9f);
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

            Vector2 targetPosition = Camera.TransformMousePosition(
                new Vector2(((float)mouseState.X / _graphics.PreferredBackBufferWidth), ((float)mouseState.Y / _graphics.PreferredBackBufferHeight)));

            MousePosition = targetPosition;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Graphics.Clear(Color.White);

            Graphics.Begin(Camera.GetMatrix());

            Points.ForEach(p => Graphics.DrawCircle(p.Position, 4, Color.Red));

            Graphics.End();
        }

        public Point GetClosestPoint(Vector2 p, float maxDistance = 16)
        {
            return Points
                .Select(x => (x, Vector2.Distance(x.Position, p)))
                .OrderBy(x => x.Item2)
                .Where(x => x.Item2 < maxDistance)
                .Select(x => x.Item1)
                .FirstOrDefault();
        }
    }
}

