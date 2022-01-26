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
    public class SpringPoint
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public bool Pinned { get; set; }
        public List<SpringPoint> Connections { get; set; }

        public SpringPoint(Vector2 position, bool pinned = false)
        {
            Position = position;
            Connections = new List<SpringPoint>();
            Pinned = pinned;
        }

        public SpringPoint AddConnection(SpringPoint other, bool addToOther = true)
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
        SpringPoint SelectedPoint = null;

        public List<SpringPoint> Points;

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

            Points = new List<SpringPoint>();

            float s = 64;

            var p11 = new SpringPoint(new Vector2(-s, -s), true);
            var p12 = new SpringPoint(new Vector2(-s, 0));
            var p13 = new SpringPoint(new Vector2(-s, s));

            var p21 = new SpringPoint(new Vector2(0, -s), true);
            var p22 = new SpringPoint(new Vector2(0, 0));
            var p23 = new SpringPoint(new Vector2(0, s));

            var p31 = new SpringPoint(new Vector2(s, -s), true);
            var p32 = new SpringPoint(new Vector2(s, 0));
            var p33 = new SpringPoint(new Vector2(s, s));

            // Vertical connections
            p11.AddConnection(p12);
            p12.AddConnection(p13);

            p21.AddConnection(p22);
            p22.AddConnection(p23);
            
            p31.AddConnection(p32);
            p32.AddConnection(p33);

            // Horizontal connections
            //p11.AddConnection(p21);
            //p21.AddConnection(p31);

            //p12.AddConnection(p22);
            //p22.AddConnection(p32);
            
            //p13.AddConnection(p23);
            //p23.AddConnection(p33);

            // Add the points
            Points.Add(p11);
            Points.Add(p12);
            Points.Add(p13);

            Points.Add(p21);
            Points.Add(p22);
            Points.Add(p23);

            Points.Add(p31);
            Points.Add(p32);
            Points.Add(p33);
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
            base.Update(gameTime);
            
            float delta = gameTime.GetDeltaInSeconds();
            var keyState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            MousePosition = Camera.TransformMousePosition(
                new Vector2(((float)mouseState.X / _graphics.PreferredBackBufferWidth), ((float)mouseState.Y / _graphics.PreferredBackBufferHeight)));

            if(mouseState.LeftButton == ButtonState.Pressed)
            {
                if(SelectedPoint == null)
                {
                    SelectedPoint = GetClosestPoint(MousePosition);
                }

                if(SelectedPoint != null)
                {
                    SelectedPoint.Position = MousePosition;
                    SelectedPoint.Velocity = Vector2.Zero;
                }
            }
            else
            {
                if(SelectedPoint != null)
                {
                    SelectedPoint.Velocity = Vector2.Zero;
                }
                SelectedPoint = null;
            }

            int iterations = 40;
            delta /= iterations;

            for(int i = 0; i < iterations; i++)
            {
                foreach (var point in Points.Where(x => !x.Pinned))
                {
                    point.Velocity += new Vector2(0, 128) * delta;

                    foreach(var connection in point.Connections)
                    {
                        Vector2 direction = connection.Position - point.Position;

                        float springLength = 64;
                        float length = direction.Length();
                        float stretch = length - springLength;
                        float k = 10f;
                        float d = 0.1f;

                        direction /= length;

                        point.Velocity += direction * stretch * k;
                        point.Velocity -= direction * Vector2.Dot(direction, point.Velocity) * d;
                    }
                }


                foreach (var point in Points.Where(x => !x.Pinned))
                {
                    point.Position += point.Velocity * delta;
                }

            }
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Graphics.Clear(Color.White);

            Graphics.Begin(Camera.GetMatrix());

            foreach (var point in Points)
            {
                foreach (var connection in point.Connections)
                {
                    Graphics.DrawLine(point.Position, connection.Position, 2, 0, Color.Black);
                }
            }

            foreach (var point in Points)
            {
                Graphics.DrawCircle(point.Position, 4, Color.Red);
            }

            Graphics.End();
        }

        public SpringPoint GetClosestPoint(Vector2 p, float maxDistance = 16)
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

