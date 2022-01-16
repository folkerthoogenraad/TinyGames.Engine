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
using System.Diagnostics;

namespace TinyGames
{
    public class ShapesTestGame : Game
    {
        private GraphicsDeviceManager _graphics;

        public Graphics2D Graphics;
        public Camera Camera;

        Vector2 MousePosition;

        public ShapesTestGame()
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
            Graphics.Clear(Color.White);

            Graphics.Begin(Camera.GetMatrix());

            DrawPolygonTest();

            Graphics.End();

            base.Draw(gameTime);
        }

        public void DrawPolygonTest()
        {
            Polygon polygon = new Polygon(new Vector2[] {
                new Vector2(0.93f,-7.67f) * 16,
                new Vector2(5.77f,-5.3f) * 16,
                new Vector2(7.5f,2.27f) * 16,
                new Vector2(-1.53f,2.84f) * 16,
                new Vector2(-3.2f,-4.57f) * 16,
            });

            DrawPolygon(polygon, polygon.Inside(MousePosition) ? Color.Green : Color.Black);

            Graphics.DrawCircle(MousePosition, 2, Color.Blue, 0);
            Graphics.DrawCircle(polygon.ClosestPoint(MousePosition), 2, Color.Green, 0);
        }

        public void DrawPolygon(Polygon polygon, Color color)
        {
            foreach(var line in polygon.Lines)
            {
                Graphics.DrawLine(line.From, line.To, 1, 0, color);
            }
        }

        public void DrawLineTest()
        {
            Line line = new Line(new Vector2(-1, 2), new Vector2(36, 124));

            Vector2 projectedPoint = line.GetProjectedPoint(MousePosition);
            Vector2 projectedPointClamped = line.GetProjectedPointClamped(MousePosition);

            float distance = Vector2.Distance(projectedPoint, MousePosition);
            float distance2 = line.SignedDistance(MousePosition);

            Debug.WriteLine("Distance: " + distance + " -> " + distance2);

            Graphics.DrawLine(line.From, line.To, 1, 0, Color.Black);
            Graphics.DrawCircle(line.To, 2, Color.Blue);

            Graphics.DrawLine(projectedPoint, MousePosition, 1, 0, Color.Green);
            Graphics.DrawLine(projectedPointClamped, MousePosition, 1, 0, Color.Green);

            Graphics.DrawCircle(MousePosition, 2, line.IsOnRight(MousePosition) ? Color.Green : Color.Red);

        }
    }
}
