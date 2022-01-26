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

namespace TinyGames
{
    public class CollisionTestGame : Game
    {
        private GraphicsDeviceManager _graphics;

        public Graphics2D Graphics;
        public Camera Camera;

        public PhysicsWorld World;

        public PhysicsBody MouseBody;

        Vector2 MousePosition;
        Vector2 OriginPosition;

        private BodyCollisionSet _collisions;

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
            Graphics = new Graphics2D(GraphicsDevice);
            World = new PhysicsWorld();

            Camera = new Camera(720, 16f / 9f);

            MouseBody = new PhysicsBody(new Vector2(0, 0), new BoxCollider(AABB.Create(-16, -16, 32, 32))) { Solid = false };

            World.AddBody(MouseBody);

            //Random random = new Random(12);
            Random random = new Random(4);

            for (int i = 0; i < 20; i++)
            {
                World.AddBody(new PhysicsBody(new Vector2((random.NextFloat() * 2 - 1) * 180, (random.NextFloat() * 2 - 1) * 180), new BoxCollider(AABB.CreateCentered(0, 0, random.NextFloat() * 64 + 8, random.NextFloat() * 64 + 8)))
                {
                    Solid = true
                });
            }

            for (int i = 0; i < 200; i++)
            {
                World.AddBody(new PhysicsBody(new Vector2((random.NextFloat() * 2 - 1) * 180, (random.NextFloat() * 2 - 1) * 180), 
                    new CircleCollider(new Circle(random.RandomPointInCircle() * 128, random.NextFloatRange(8, 32))))
                {
                    Solid = true
                });
            }

            for (int i = 0; i < 0; i++)
            {
                World.AddBody(new PhysicsBody(new Vector2(i * 16 - 128, 0), new BoxCollider(AABB.CreateCentered(0, 0, 16, 16)))
                {
                    Solid = true,
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

            _collisions = World.Update(delta);

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

        public void DrawWorld(PhysicsWorld world)
        {
            foreach (var body in world.Bodies)
            {
                DrawBody(body, Color.Green);
            }

            if (_collisions != null && false)
            {
                foreach (var collision in _collisions.CollisionIndices)
                {
                    var a = collision.BodyA;
                    var b = collision.BodyB;

                    //DrawAABB(a.Bounds, Color.Red);
                    DrawAABB(a.Bounds.Translated(a.UnstuckMotion), Color.Red);
                    //DrawAABB(b.Bounds, Color.Red);
                    DrawAABB(b.Bounds.Translated(b.UnstuckMotion), Color.Red);
                }
            }
        }

        public void DrawBody(PhysicsBody body, Color color)
        {
            if(body.Collider is BoxCollider)
            {
                AABB bounds = body.Collider.Bounds.Translated(body.Position);

                Graphics.DrawRectangle(bounds, color);
            }
            if(body.Collider is CircleCollider)
            {
                var circleCollider = body.Collider as CircleCollider;
                var circle = circleCollider.Circle.Translated(body.Position);

                Graphics.DrawCircle(circle.Position, circle.Radius, color);
            }
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
