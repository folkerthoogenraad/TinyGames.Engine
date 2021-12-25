using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TinyGames.Engine.Extensions;
using TinyGames.Engine.Graphics;

using System.Linq;
using TinyGames.Engine.Maths;
using TinyGames.Engine.Collisions;

namespace TinyGames
{
    public class CollisionTestGame : Game
    {
        private GraphicsDeviceManager _graphics;

        public Graphics2D Graphics;
        public Camera Camera;

        public World World;

        public Body MouseBody;

        Vector2 MousePosition;
        Vector2 OriginPosition;

        public CollisionTestGame()
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

            Graphics = new Graphics2D(GraphicsDevice, effect);
            World = new World();

            Camera = new Camera(360, 16f / 9f);

            MouseBody = new Body(new Vector2(0, 0), new Collider(AABB.Create(-16, -16, 32, 32)));

            World.AddBody(MouseBody);

            Random random = new Random(12);

            for(int i = 0; i < 0; i++)
            {
                World.AddBody(new Body(new Vector2((random.NextFloat() * 2 - 1) * 180, (random.NextFloat() * 2 - 1) * 180), new Collider(AABB.CreateCentered(0, 0, random.NextFloat() * 64 + 8, random.NextFloat() * 64 + 8))) { 
                    Static = false
                });
            }

            for(int i = 0; i < 10; i++)
            {
                World.AddBody(new Body(new Vector2(i * 16 - 128, 0), new Collider(AABB.CreateCentered(0, 0, 16, 16)))
                {
                    Static = true
                });
            }
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

            if(mouseState.LeftButton == ButtonState.Pressed)
            {
                OriginPosition = MousePosition;
            }

            var wantedPosition = targetPosition;
            MouseBody.Velocity = Vector2.Lerp(MouseBody.Velocity, (wantedPosition - MouseBody.Position) * 10, delta * 10);

            World.Update(delta);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Graphics.Clear(Color.White);

            Graphics.Begin(Camera.GetMatrix());

            // DrawDebugMinkowski();
            DrawWorld(World);

            Graphics.End();

            base.Draw(gameTime);
        }

        public void DrawDebugMinkowski()
        {
            var mouseBounds = AABB.CreateCentered(MousePosition, new Vector2(32, 16));
            var colliderBounds = AABB.CreateCentered(new Vector2(32, 52), new Vector2(64, 16));
            var minkow = AABB.MinkowskiDifference(mouseBounds, colliderBounds);


            var velocity = OriginPosition - MousePosition;
            
            Graphics.DrawLine(MousePosition, OriginPosition, 1, 0, Color.Green);
            Graphics.DrawLine(Vector2.Zero, -velocity, 1, 0, Color.Yellow);

            float distance;
            Vector2 normal;
            if(minkow.RayIntersection(-velocity, velocity, out distance, out normal))
            {
                DrawPoint(-velocity + velocity * distance, Color.Green);
                DrawAABB(mouseBounds.Translated(velocity * (1 - distance)), Color.Green);
                DrawAABB(mouseBounds.Translated(mouseBounds.UnstuckVector(colliderBounds, -normal)), Color.Aqua);
            }

            DrawAABB(mouseBounds, Color.Black);
            DrawAABB(colliderBounds, Color.Black);

            DrawAABB(minkow, Color.Red);

            DrawPoint(Vector2.Zero, Color.Red);


        }

        public void DrawWorld(World world)
        {
            foreach (var body in world.Bodies)
            {
                DrawBody(body, Color.Green);
            }

            if (world.CollisionSet != null)
            {
                foreach (var collision in world.CollisionSet.Collisions)
                {
                    var a = world.CollisionSet.Bounds[collision.BodyA];
                    var b = world.CollisionSet.Bounds[collision.BodyB];

                    // DrawAABB(a.Bounds, Color.Red);
                    // DrawAABB(a.Bounds.Translated(a.UnstuckMotion), Color.Red);
                    // DrawAABB(b.Bounds, Color.Red);
                    // DrawAABB(b.Bounds.Translated(b.UnstuckMotion), Color.Red);
                }
            }
        }

        public void DrawBody(Body body, Color color)
        {
            AABB bounds = body.Collider.Bounds.Translated(body.Position);

            DrawAABB(bounds, color);
        }
        public void DrawAABB(AABB bounds, Color color)
        {
            Graphics.DrawLine(bounds.TopLeft, bounds.TopRight, 1, 0, color);
            Graphics.DrawLine(bounds.TopRight, bounds.BottomRight, 1, 0, color);
            Graphics.DrawLine(bounds.BottomRight, bounds.BottomLeft, 1, 0, color);
            Graphics.DrawLine(bounds.BottomLeft, bounds.TopLeft, 1, 0, color);
        }

        public void DrawPoint(Vector2 point, Color color)
        {
            Graphics.DrawCircle(point, 2, color);
        }
    }
}
