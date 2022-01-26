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

namespace TinyGames
{
    public interface IGrid<T>
    {
        public int Width { get; }
        public int Height { get; }

        public T GetValue(int x, int y);
        public void SetValue(int x, int y, T value);
    }

    public class ReadWriteGrid<T> : IGrid<T>
    {
        private Grid<T> _readGrid;
        private Grid<T> _writeGrid;

        public int Width => _readGrid.Width;
        public int Height => _readGrid.Height;

        public ReadWriteGrid(int width, int height)
        {
            _readGrid = new Grid<T>(width, height);
            _writeGrid = new Grid<T>(width, height);
        }

        public T GetValue(int x, int y)
        {
            return _readGrid.GetValue(x, y);
        }

        public void SetValue(int x, int y, T value)
        {
            _writeGrid.SetValue(x, y, value);
        }

        public void Flip()
        {
            var temp = _readGrid;

            _readGrid = _writeGrid;
            _writeGrid = temp;
        }
    }

    public class Grid<T> : IGrid<T>
    {
        private T[] _values;

        public int Width { get; }
        public int Height { get; }

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;

            _values = new T[Width * Height];
        }

        public T GetValue(int x, int y)
        {
            return _values[GetIndex(x, y)];
        }

        public void SetValue(int x, int y, T value)
        {
            _values[GetIndex(x, y)] = value;
        }

        private int GetIndex(int x, int y)
        {
            x %= Width;
            y %= Height;

            if (x < 0) x += Width;
            if (y < 0) y += Height;

            return x + y * Width;
        }
    }

    public struct Particle
    {
        public double Velocity;
        public double Height;
    }

    public class GridTestGame : Game
    {
        private GraphicsDeviceManager _graphics;

        public Graphics2D Graphics;
        public Camera Camera;

        public PhysicsWorld World;

        public PhysicsBody MouseBody;

        public Vector2 GridSize = new Vector2(4, 4);

        public int Width = 128;
        public int Height = 128;

        public Color[] Colors;
        public Texture2D Texture;
        public ReadWriteGrid<Particle> Grid;

        public float Frequency = 8;
        public float Phase = 0;

        public GridTestGame()
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
            var effect = Content.Load<Effect>("Effects/StandardEffect");

            Graphics = new Graphics2D(GraphicsDevice, effect);
            World = new PhysicsWorld();

            Camera = new Camera(1080, 16f / 9f);

            Grid = new ReadWriteGrid<Particle>(Width, Height);
            Texture = new Texture2D(GraphicsDevice, Width, Height);
            Colors = new Color[Width * Height];
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        private double GetDiff(double self, double other)
        {
            return (other - self);
        }

        private double UpdateVelocity(Particle self, Particle other)
        {
            return GetDiff(self.Height, other.Height);
        }

        Vector2 TargetPosition;

        protected override void Update(GameTime gameTime)
        {
            float delta = gameTime.GetDeltaInSeconds();
            var keyState = Keyboard.GetState();
            var mouseState = Mouse.GetState();


            Phase += Frequency * delta;

            if (keyState.IsKeyDown(Keys.Up))
            {
                Frequency += 1 * delta;
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                Frequency -= 1 * delta;
            }

            for (int i = 0; i < 4; i++)
            {
                ProcessAllSync();

                if(mouseState.LeftButton == ButtonState.Pressed)
                {
                    TargetPosition = Camera.TransformMousePosition(new Vector2(((float)mouseState.X / _graphics.PreferredBackBufferWidth), ((float)mouseState.Y / _graphics.PreferredBackBufferHeight)));

                }
                if (true)
                {
                    double value = Math.Sin(Phase * (Math.PI * 2)) * 1;

                    var t = (TargetPosition / GridSize) - new Vector2(Width, Height) / 2;

                    for(int x = -2; x < 2; x++)
                    {
                        for(int y = -2; y < 2; y++)
                        {
                            Grid.SetValue((int)t.X + x, (int)t.Y + y, new Particle() { Height = value, Velocity = -value });
                        }
                    }
                }

                Grid.Flip();
                
            }

            Texture.SetData(Colors);

            base.Update(gameTime);
        }

        public void ProcessAllSync()
        {
            var process = ProcessAll();
            process.Wait();
        }

        public async Task ProcessAll()
        {
            List<Task> tasks = new List<Task>();

            int chunkSize = Width / 4;

            for(int x = 0; x < Width; x += chunkSize)
            {
                for(int y = 0; y < Height; y += chunkSize)
                {
                    tasks.Add(ProcessAsync(x, y, chunkSize, chunkSize));
                }
            }

            await Task.WhenAll(tasks);
        }


        public Task ProcessAsync(int xoffset, int yoffset, int width, int height)
        {
            return Task.Run(() => Process(xoffset, yoffset, width, height));
        }

        public void Process(int xoffset, int yoffset, int width, int height)
        {
            for (int i = xoffset; i < xoffset + width; i++)
            {
                for (int j = yoffset; j < yoffset + height; j++)
                {
                    var selfValue = Grid.GetValue(i, j);

                    var value = selfValue;

                    double velocityDelta = 0;

                    double digWeight = 1 / Math.Sqrt(2);
                    double totalWeight = 5 + digWeight * 4;

                    // Straights
                    velocityDelta += GetDiff(selfValue.Height, 0);

                    velocityDelta += GetDiff(selfValue.Height, Grid.GetValue(i - 1, j).Height);
                    velocityDelta += GetDiff(selfValue.Height, Grid.GetValue(i + 1, j).Height);
                    velocityDelta += GetDiff(selfValue.Height, Grid.GetValue(i, j - 1).Height);
                    velocityDelta += GetDiff(selfValue.Height, Grid.GetValue(i, j + 1).Height);

                    velocityDelta += GetDiff(selfValue.Height, Grid.GetValue(i - 1, j - 1).Height) * digWeight;
                    velocityDelta += GetDiff(selfValue.Height, Grid.GetValue(i + 1, j - 1).Height) * digWeight;
                    velocityDelta += GetDiff(selfValue.Height, Grid.GetValue(i - 1, j + 1).Height) * digWeight;
                    velocityDelta += GetDiff(selfValue.Height, Grid.GetValue(i + 1, j + 1).Height) * digWeight;

                    velocityDelta /= totalWeight;

                    value.Velocity = (selfValue.Velocity + velocityDelta * 0.1) * .999;
                    value.Height += value.Velocity;

                    Grid.SetValue(i, j, value);

                    Colors[i + j * Height] = GetColorForParticle(value);
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            Graphics.Clear(Color.White);

            Graphics.Begin(Camera.GetMatrix());

            Vector2 offset = -GridSize * new Vector2(Width, Height) / 2;
            Vector2 size = GridSize * new Vector2(Width, Height);
            Vector2 sizeHorizontal = GridSize * new Vector2(Width, 0);

            Graphics.DrawTexture(Texture, offset, size);
            Graphics.DrawTexture(Texture, offset - sizeHorizontal, size);
            Graphics.DrawTexture(Texture, offset + sizeHorizontal, size);

            Graphics.End();

            base.Draw(gameTime);
        }

        public Color GetColorForParticle(Particle value)
        {
            float h = (float)Math.Clamp(value.Height, -1, 1);
            float v = (float)Math.Clamp(value.Velocity, -1, 1);

            float t = (float)Math.Clamp(h + v, -1, 1);

            Color color;

            if (t > 0)
            {
                color = new Color(t, 0, 0, 1);
            }
            else
            {
                color = new Color(-t, 0, 0, 1);
            }

            return color;
        }
    }
}
