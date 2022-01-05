using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TinyGames.Engine.Extensions;
using TinyGames.Engine.Graphics;

using System.Linq;
using TinyGames.Engine.Maths;
using TinyGames.Engine.Collisions;
using System.Collections.Generic;
using TinyGames.Engine.Util;
using StudentBikeGame.Screens;

namespace StudentBikeGame
{

    public class StudentBikeGame : Game
    {
        private GraphicsDeviceManager _graphics;

        public Graphics2D Graphics;
        public Camera Camera;
        
        private Screen _screen;

        public Screen Screen
        {
            get => _screen;
            set {
                if(_screen != value)
                {
                    _screen?.Destroy();
                    _screen = value;
                    _screen?.Init(GraphicsDevice, Content);
                }
            }
        }

        public StudentBikeGame()
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
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

        }

        protected override void LoadContent()
        {
            Graphics = new Graphics2D(GraphicsDevice);

            Camera = new Camera(360, 16f / 9f);

            Screen = new InGameScreen();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            float delta = gameTime.GetDeltaInSeconds();

            Screen.Update(delta);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Screen.Draw();
        }
    }
#if false
    public class StudentBikeGame : Game
    {
        private GraphicsDeviceManager _graphics;

        public Graphics2D Graphics;
        public Camera Camera;

        public float AngularVelocity = 0;
        public float Angle = -MathF.PI / 2;
        public float TargetAngle = -MathF.PI / 2;

        private RingList<(Vector2 position, SurfaceType surface)> _backWheel;
        private RingList<(Vector2 position, SurfaceType surface)> _frontWheel;

        private Bike _bike;

        private Sprite _background;

        private Sprite _bikeSprite;
        private Sprite _bikeTurnSprite;
        private Animation _bikerAnimation;
        private Animation _bikerTurnAnimation;

        private Animation _smokeAnimation;

        private float PedallingTimer = 0;

        public TextureSampler CollisionSampler;
        public DataMapping<Color, SurfaceType> SurfaceMapping;

        public DataMapping<SurfaceType, SurfaceHandling> HandlingMapping;

        public float SmokeParticleTimer = 0;
        public List<Particle> Particles;

        private int SteerDirection = 1;

        public StudentBikeGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _backWheel = new RingList<(Vector2 position, SurfaceType surface)>(100);
            _frontWheel = new RingList<(Vector2 position, SurfaceType surface)>(100);

            Particles = new List<Particle>();

            _bike = new Bike(new BikeHandling());

            var grassSurface = new SurfaceHandling()
            {
                SteepnessMuliplier = 1f,
                GripMultiplier = 0.2f,
                ResistanceMultiplier = 1.0f,
            };

            var dirtSurface = new SurfaceHandling()
            {
                SteepnessMuliplier = 1,
                GripMultiplier = 1,
                ResistanceMultiplier = 1f,
            };
            var asphaltSurface = new SurfaceHandling()
            {
                SteepnessMuliplier = 40,
                GripMultiplier = 1.2f,
                ResistanceMultiplier = 1.0f,
            };

            SurfaceMapping = new DataMapping<Color, SurfaceType>(SurfaceType.Grass);
            SurfaceMapping.Register(Color.Red, SurfaceType.Dirt);
            SurfaceMapping.Register(Color.Blue, SurfaceType.Asphalt);

            HandlingMapping = new DataMapping<SurfaceType, SurfaceHandling>(grassSurface);
            HandlingMapping.Register(SurfaceType.Dirt, dirtSurface);
            HandlingMapping.Register(SurfaceType.Asphalt, asphaltSurface);
        }

        protected override void Initialize()
        {
            base.Initialize();

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

        }

        protected override void LoadContent()
        {
            var sheet = Content.Load<Texture2D>("Sprites/Sheet");
            var background = Content.Load<Texture2D>("Sprites/Background");

            var backgroundCollision = Content.Load<Texture2D>("Sprites/Background_Collision");

            CollisionSampler = TextureSampler.FromTexture(backgroundCollision);

            _background = new Sprite(background);

            _bikeSprite = new Sprite(sheet, new Rectangle(0, 0, 32, 16));
            _bikeTurnSprite = new Sprite(sheet, new Rectangle(32, 0, 32, 16));
            _bikeSprite.CenterOrigin();
            _bikeTurnSprite.CenterOrigin();

            _bikerAnimation = new Animation(
                new Sprite(sheet, new Rectangle(0, 16, 32, 16)).CenterOrigin(),
                new Sprite(sheet, new Rectangle(0, 32, 32, 16)).CenterOrigin())
            {
                FrameRate = 1,
            };
            _bikerTurnAnimation = new Animation(
                new Sprite(sheet, new Rectangle(32, 16, 32, 16)).CenterOrigin()
                )
            { FrameRate = 1, };

            _smokeAnimation = new Animation(
                new Sprite(sheet, new Rectangle(64, 0, 16, 16)).CenterOrigin(),
                new Sprite(sheet, new Rectangle(64 + 16, 0, 16, 16)).CenterOrigin(),
                new Sprite(sheet, new Rectangle(64 + 32, 0, 16, 16)).CenterOrigin(),
                new Sprite(sheet, new Rectangle(64 + 48, 0, 16, 16)).CenterOrigin()
                )
            {
                FrameRate = 6,
            };

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


            // Simulate the angle of the bike
            float wantedAngle = -MathF.PI / 2;
            float multiplier = 3;

            SteerDirection = 0;

            if (keyState.IsKeyDown(Keys.Left))
            {
                wantedAngle -= MathF.PI / 4;
                multiplier = 1;
                SteerDirection = -1;
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                wantedAngle += MathF.PI / 4;
                multiplier = 1;
                SteerDirection = 1;
            }

            TargetAngle = Tools.Lerp(TargetAngle, wantedAngle, delta * 10);

            float wantedVelocity = -(Angle - TargetAngle) * 10;

            AngularVelocity = Tools.Lerp(AngularVelocity, wantedVelocity, delta * 5 * multiplier);

            Angle += AngularVelocity * delta;



            // Simulate the actual bike stuff
            var bikeSurface = SurfaceMapping.Map(CollisionSampler.GetColor(_bike.Position));
            var angled = Tools.AngleVector(Angle);

            _bike = _bike.Update(delta, 300, angled.X * 5, HandlingMapping.Map(bikeSurface));

            // Visual stuff
            PedallingTimer += delta * _bike.ForwardVelocity * 0.05f;

            // Front and back wheel animations
            var frontWheelPosition = _bike.Position + _bike.Forward * 10;
            var backWheelPosition = _bike.Position - _bike.Forward * 10;

            _frontWheel.Add((frontWheelPosition, SurfaceMapping.Map(CollisionSampler.GetColor(frontWheelPosition))));
            _backWheel.Add((backWheelPosition, SurfaceMapping.Map(CollisionSampler.GetColor(backWheelPosition))));

            Particles = Particles.Where(x => x.Update(delta)).ToList();

            // Move the camera
            var targetPosition = _bike.Position + _bike.Heading * _bike.ForwardVelocity;
            Camera.Position = Vector2.Lerp(Camera.Position, targetPosition, delta * 2);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Graphics.Clear(Color.FloralWhite);

            Graphics.Begin(Camera.GetMatrix());

            Graphics.DrawSprite(_background, Vector2.Zero, 0, -2);
            Graphics.DrawSprite(_background, Vector2.Zero + _background.Size * new Vector2(-1, 0), 0, -2);
            Graphics.DrawSprite(_background, Vector2.Zero + _background.Size * new Vector2(1, 0), 0, -2);
            Graphics.DrawSprite(_background, Vector2.Zero + _background.Size * new Vector2(0, -1), 0, -2);
            Graphics.DrawSprite(_background, Vector2.Zero + _background.Size * new Vector2(0, 1), 0, -2);

            DrawParticles();


            DrawTrail(Graphics, _backWheel.Select(x => (x.position, GetSurfaceTrailColor(x.surface))), 4);
            DrawTrail(Graphics, _frontWheel.Select(x => (x.position, GetSurfaceTrailColor(x.surface))), 4);

            // DrawDebug();

            Graphics.End();

            base.Draw(gameTime);
        }

        public void DrawParticles()
        {
            foreach(var particle in Particles)
            {
                Graphics.DrawSprite(particle.Animation.GetSpriteForTime(particle.Timer), particle.Position, particle.Angle, 0, particle.Color);
            }
        }

        public void DrawDebug()
        {
            Vector2 startPos = Camera.Position;
            Vector2 endPos = startPos + Tools.AngleVector(Angle) * 50;

            Vector2 targetDir = Tools.AngleVector(TargetAngle) * 50;

            Vector2 groundPosition = new Vector2(endPos.X, startPos.Y);

            Graphics.DrawLine(startPos, startPos + targetDir, 1, 0, Color.Green);
            Graphics.DrawLine(endPos, groundPosition, 1, 0, Color.Blue);
            Graphics.DrawLine(startPos, endPos, 1, 0, Color.Black);

            Graphics.DrawCircle(startPos, 2, Color.Red);
            Graphics.DrawCircle(endPos, 2, Color.Red);

            Graphics.DrawCircle(groundPosition, 2, Color.Red);
        }

        public Color GetSurfaceTrailColor(SurfaceType surface)
        {
            if (surface == SurfaceType.Grass) return new Color(142, 158, 108);
            if (surface == SurfaceType.Dirt) return new Color(82, 74, 65);

            return new Color(53, 53, 53);
        }

        public void DrawTrail(Graphics2D graphics, IEnumerable<(Vector2 position, Color color)> trail, float width)
        {
            var trailPairs = trail.Reverse().Pairs();

            int index = 0;
            int count = trailPairs.Count();

            float widthPerIndex = width / count;

            foreach (var (from, to) in trailPairs)
            {
                graphics.DrawLine(from.position, to.position, width - index * widthPerIndex, -1, from.color);

                index++;
            }
        }
    }
#endif
}
