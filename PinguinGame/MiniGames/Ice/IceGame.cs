﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Input;
using PinguinGame.MiniGames.Generic;
using PinguinGame.Player;
using PinguinGame.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Util;

namespace PinguinGame.MiniGames.Ice
{
    public class IceGame
    {
        public Camera Camera { get; set; }
        public IEnumerable<Character> Penguins => _penguins;
        public IEnumerable<PlayerInfo> Players => Fight.Players;
        public Fight Fight { get; private set; }

        public IceLevel Level { get; private set; }
        public Camera LevelCamera { get; private set; }
        public IceLevelGraphics LevelGraphics { get; private set; }
        public SnowballGraphics SnowballGraphics { get; set; }

        public IMiniGameInputService<CharacterInput> InputService { get; set; }
        
        private List<Character> _penguins;
        private List<Snowball> _snowballs;

        public IceGame(ContentManager content, GraphicsDevice device, IceLevel level, PlayerInfo[] players, IMiniGameInputService<CharacterInput> inputService) // Probably should have a levelservice or something
        {
            _penguins = new List<Character>();
            _snowballs = new List<Snowball>();

            InputService = inputService;

            Fight = new Fight(players);

            Level = level;
            LevelCamera = new Camera(256, 1);
            LevelGraphics = new IceLevelGraphics(device, 256, 256, new IceLevelGraphicsSettings());
            SnowballGraphics = new SnowballGraphics() { 
                Indicator = new Sprite(content.Load<Texture2D>("Sprites/Ice/IceGameplayElements"), new Rectangle(0, 0, 8, 8)).CenterOrigin(),
                Sprite = new Sprite(content.Load<Texture2D>("Sprites/Ice/IceGameplayElements"), new Rectangle(8, 0, 8, 8)).CenterOrigin(),
                Shadow = new Sprite(content.Load<Texture2D>("Sprites/Ice/IceGameplayElements"), new Rectangle(8, 8, 8, 8)).CenterOrigin(),
            };
        }

        public void AddSnowball(Snowball ball)
        {
            _snowballs.Add(ball);
        }

        public void AddPenguin(Character penguin)
        {
            _penguins.Add(penguin);
        }

        public void RemovePenguin(Character penguin)
        {
            _penguins.Remove(penguin);
        }

        public void RemoveAllPenguins()
        {
            _penguins.Clear();
        }

        public void TryBonkCharacters()
        {
            // Bonks!
            foreach (var (a, b) in Penguins.Combinations())
            {
                var p1 = a.Position;
                var p2 = b.Position;

                var dir = p2 - p1;
                var dist = dir.Length();

                if (dist > 8) continue;
                if (dist == 0) continue;

                dir /= dist;

                var totalVelocity = (a.Physics.Velocity - b.Physics.Velocity).Length();
                var bonkVelocity = Math.Max(1, totalVelocity);

                var bonkA = BonkCharacters(a, b, bonkVelocity, -dir);
                var bonkB = BonkCharacters(b, a, bonkVelocity, dir);

                a.Bonk(bonkA);
                b.Bonk(bonkB);
            }

            foreach(var penguin in Penguins)
            {
                foreach(var snowball in _snowballs)
                {
                    if (penguin.Player == snowball.Player) continue;

                    var p1 = penguin.Position;
                    var p2 = snowball.Position;

                    var dir = p2 - p1;
                    var dist = dir.Length();

                    if (dist > 8) continue;

                    snowball.Collided = true;

                    penguin.Bonk(snowball.Velocity * 0.5f);
                }
            }
        }

        private Vector2 BonkCharacters(Character self, Character other, float bonkVelocity, Vector2 direction)
        {
            // Advantage
            if (self.IsSliding && other.IsGathering)
            {
                return bonkVelocity * 0 * direction;
            }

            // Disadvantage
            else if (self.IsGathering && other.IsSliding)
            {
                return bonkVelocity * 1.2f * direction;
            }

            // No advantage
            else
            {
                // Divide equally
                return bonkVelocity * 0.5f * direction;
            }
        }

        public List<Character> TryDrownCharacters()
        {
            var result = new List<Character>();

            foreach(var p in _penguins.Where(x => !x.IsDrowning))
            {
                var block = Level.GetIceBlockForPosition(p.Position);
                
                if(block == null || !block.Solid)
                {
                    p.Drown();
                    result.Add(p);
                }
            }

            return result;
        }

        public void PlaceCharactersOnGround()
        {
            foreach (var penguin in _penguins)
            {
                IceBlock block = Level.GetIceBlockForPosition(penguin.Position);
                float height = 0;

                if (block != null && block.Solid)
                {
                    height = block.Height;
                }

                if (penguin.GroundHeight > height)
                {
                    penguin.Bounce.Height += penguin.GroundHeight - height;
                }

                penguin.GroundHeight = height;
                penguin.Grounded = block != null && block.Solid;
            }
        }

        public void Update(float delta)
        {
            UpdateLevel(delta);
            UpdateSnowballs(delta);
            UpdatePenguins(delta);
        }

        private float SinkTimer = 3;

        public void UpdateLevel(float delta)
        {
            foreach (var block in Level.Blocks)
            {
                block.Update(delta);
            }

            SinkTimer -= delta;

            if (SinkTimer < 0) {
                SinkTimer += 1 + (float)(new Random().NextDouble()) * 3;

                var avail = Level.Blocks.Where(x => x.State is IceBlockIdleState && x.Sinkable);
                if(avail.Count() > 0)
                {
                    var block = avail.Random();
                    block.State = new IceBlockSinkingState(block);
                }
            }
        }

        public void UpdateSnowballs(float delta)
        {
            _snowballs = _snowballs.Where(x => x.Update(delta)).ToList();
        }

        public void UpdatePenguins(float delta)
        {
            foreach (var (penguin, input) in _penguins.Select(x => (x, InputService.GetInputForPlayer(x.Player))))
            {
                penguin.Update(input, delta);
            }

            PlaceCharactersOnGround();
        }
        
        public void DrawWorld(Graphics2D graphics)
        {
            LevelGraphics.Draw(LevelCamera, Level);

            graphics.Clear(LevelGraphics.Settings.WaterColor);
            
            graphics.DrawTexture(LevelGraphics.RenderTarget, new Vector2(-128, -128), new Vector2(256, 256));

            var penguins = _penguins.OrderBy(x => x.Physics.Position.Y);

            foreach (var snowball in _snowballs)
            {
                IceBlock block = Level.GetIceBlockForPosition(snowball.Position);

                float height = 0;

                if (block != null && block.Solid)
                {
                    height = block.Height;
                }

                graphics.DrawSprite(SnowballGraphics.Shadow, snowball.Position - new Vector2(0, height), snowball.Angle);
            }

            foreach (var penguin in penguins)
            {
                penguin.Draw(graphics, penguin.Graphics);
            }

            foreach(var snowball in _snowballs)
            {
                graphics.DrawSprite(SnowballGraphics.Sprite, snowball.Position - new Vector2(0, snowball.Height), snowball.Angle);
            }

            foreach (var penguin in penguins)
            {
                if (!penguin.SnowballGathering.HasSnowball) continue;

                graphics.DrawSprite(SnowballGraphics.Indicator, penguin.Position - new Vector2(0, 24 + penguin.Bounce.Height));
            }
        }
    }
}