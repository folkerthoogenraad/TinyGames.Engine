using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Input;
using PinguinGame.MiniGames.Generic;
using PinguinGame.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using TinyGames.Engine.Graphics;

namespace PinguinGame.MiniGames.Minecarts
{
    public class MinecartGame
    {
        public MinecartLevel Level { get; }
        public MinecartLevelGraphics LevelGraphics { get; }
        public Camera Camera { get; }

        public MinecartGraphics MinecartGraphics { get; }

        public PlayerInfo[] Players { get; }
        public List<Minecart> Minecarts { get; set; }

        public ContentManager Content { get; }
        public GraphicsDevice Device { get; }
        public Graphics2D Graphics { get; }
        public IMiniGameInputService<MinecartInput> InputService { get; set; }

        private float Timer = 0;

        public MinecartGame(ContentManager content, GraphicsDevice device, MinecartLevel level, MinecartLevelGraphics graphics, PlayerInfo[] players, IMiniGameInputService<MinecartInput> input)
        {
            Level = level;
            LevelGraphics = graphics;
            Players = players;
            Device = device;
            Graphics = new Graphics2D(device);
            Content = content;
            InputService = input;

            Camera = new Camera();
            Camera.Height = 180;

            // Avarage position of spawns
            Spawn();

            var texture = content.Load<Texture2D>("Sprites/MinecartSheet");
            var effectsTexture = content.Load<Texture2D>("Sprites/Effects");

            MinecartGraphics = new MinecartGraphics()
            {
                Background = new Sprite(texture, new Rectangle(0, 96, 16, 32)).SetOrigin(8, 8),
                Foreground = new Sprite(texture, new Rectangle(16, 96, 16, 32)).SetOrigin(8, 8),
                Shadow = new Sprite(texture, new Rectangle(32, 96, 16, 32)).SetOrigin(8, 8),
                Sparks = new Animation(
                    new Sprite(effectsTexture, new Rectangle(0, 16, 16, 16)).CenterOrigin(),
                    new Sprite(effectsTexture, new Rectangle(16, 16, 16, 16)).CenterOrigin(),
                    new Sprite(effectsTexture, new Rectangle(32, 16, 16, 16)).CenterOrigin()
                    ),
                Dust = new Animation(
                    new Sprite(effectsTexture, new Rectangle(0, 32, 16, 16)).CenterOrigin(),
                    new Sprite(effectsTexture, new Rectangle(16, 32, 16, 16)).CenterOrigin(),
                    new Sprite(effectsTexture, new Rectangle(32, 32, 16, 16)).CenterOrigin()
                    )
            };
        }

        public void Spawn()
        {
            Camera.Position = Level.Spawns.Select(x => x.Position).Aggregate((a, b) => a + b) / Level.Spawns.Length;
            Camera.ClampInBounds(Level.Bounds);

            Minecarts = Players
                .Select(x => (x, Level.Spawns.Where(y => y.Index == x.Index).First()))
                .Select(x => new Minecart()
                {
                    Player = x.Item1,
                    Position = x.Item2.Position,
                    XPosition = x.Item2.Position.X,
                })
                .ToList();
        }

        public void UpdateMinecartsWithInput(float delta)
        {
            foreach (var (minecart, input) in Minecarts.Select(x => (x, InputService.GetInputForInputDevice(x.Player.InputDevice))))
            {
                minecart.Update(delta, input);
            }
        }

        public void UpdateCamera(float delta)
        {
            Timer += delta;

            float minY = Minecarts.Select(x => x.Position.Y).Min() + Camera.Size.Y / 4;

            var carts = Minecarts.Where(x => !x.OffRoad);

            Vector2 targetPosition = Camera.Position;

            if(carts.Count() > 0)
            {
                targetPosition = carts.Select(x => x.Position).Aggregate((a, b) => a + b) / carts.Count();
            }

            targetPosition = new Vector2(targetPosition.X, MathF.Min(minY, targetPosition.Y));

            Camera.Position = Vector2.Lerp(Camera.Position, targetPosition, delta * 10);
            Camera.ClampInBounds(Level.Bounds);
        }

        public void UpdateMinecartOffRoad()
        {
            var layer = Level.Tilemap.Layers.Where(x => x.Name == "Tracks").FirstOrDefault();

            foreach(var minecart in Minecarts.Where(x => x.Grounded && !x.OffRoad))
            {
                var point = Level.Tilemap.GetTilemapPosition(minecart.Position);

                var sprite = layer.GetSprite(point.X, point.Y);

                if(sprite == null)
                {
                    minecart.OffRoad = true;
                }
            }
        }

        public void Draw()
        {
            LevelGraphics.Draw(Camera, Level);

            Graphics.Begin(Camera.GetMatrix());

            foreach(var minecart in Minecarts)
            {
                Graphics.DrawSprite(MinecartGraphics.Shadow, minecart.Position, 0, 0, new Color(Color.White, 0.3f));
                Graphics.DrawSprite(MinecartGraphics.Background, minecart.Position + new Vector2(0, -minecart.Height));

                if (minecart.Grounded && minecart.RailVelocity > 4)
                {
                    var animation = MinecartGraphics.Sparks;

                    if (minecart.OffRoad)
                    {
                        animation = MinecartGraphics.Dust;
                    }

                    Graphics.DrawSprite(animation.GetSpriteForTime(Timer), minecart.Position + new Vector2(0, 8));
                }
            }

            Graphics.End();
        }
    }
}
