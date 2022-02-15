using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Audio;
using PinguinGame.Graphics;
using PinguinGame.Input;
using PinguinGame.MiniGames.Generic;
using PinguinGame.Player;
using PinguinGame.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using TinyGames.Engine.Extensions;
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Scenes;
using TinyGames.Engine.Scenes.Extensions;
using TinyGames.Engine.Util;

namespace PinguinGame.MiniGames.Ice
{
    public class IceGame
    {
        public Camera Camera { get; set; }
        public IEnumerable<Character> Characters => Scene.FindGameObjectsOfType<Character>();
        public IEnumerable<PlayerInfo> Players => Fight.Players;
        public Fight Fight { get; private set; }

        public IceLevel Level { get; private set; }
        public IceLevelGraphics LevelGraphics { get; private set; }
        public IceGameUIGraphics UIGraphics { get; set; }
        public IceGameEffects Effects { get; set; }

        public IMiniGameInputService<CharacterInput> InputService { get; set; }
        public IUISoundService UISoundService { get; set; }
        public IScreenService ScreenService { get; set; }


        private IEnumerable<Snowball> _snowballs => Scene.FindGameObjectsOfType<Snowball>();
        private IEnumerable<Geyser> _geysers => Scene.FindGameObjectsOfType<Geyser>();

        private List<IceBlockBehaviour> _iceBlockBehaviour;

        public ContentManager Content { get; set; }

        public Random Random { get; set; }

        public Scene Scene { get; set; }
        public SceneGraphics SceneGraphics { get; set; }
        public ParticleSystem ParticleSystem { get; set; }

        public IceGame(IServiceProvider services, ContentManager content, GraphicsDevice device, IceLevel level, PlayerInfo[] players, IMiniGameInputService<CharacterInput> inputService, IUISoundService uiSoundService, IScreenService screenservice) // Probably should have a levelservice or something
        {
            Content = content;

            UISoundService = uiSoundService;
            ScreenService = screenservice;
            UIGraphics = new IceGameUIGraphics(content);
            Effects = new IceGameEffects(content);

            InputService = inputService;

            Fight = new Fight(players);

            Level = level;
            LevelGraphics = new IceLevelGraphics(content, device, level, new IceLevelGraphicsSettings());

            _iceBlockBehaviour = new List<IceBlockBehaviour>() { 
                new RandomSinkIceBlockBehaviour(),
                new TimedSinkIceBlockBehaviour(),
                new TimedDriftIceBlockBehaviour(),
            };


            Random = new Random();

            Scene = new Scene(services);

            SceneGraphics = Scene.AddBehaviour(new SceneGraphics());
            ParticleSystem  = Scene.AddBehaviour(new ParticleSystem());
            Scene.AddBehaviour(new Walkables(Level));
            Scene.AddBehaviour(new SnowballGraphics(content));
            Scene.AddBehaviour(new IceGameUIGraphics(content));

            Scene.Init();


            // This is loading the Geysers, not really needed.
            var effectsTexture = content.Load<Texture2D>("Sprites/Effects");
            var geyserAnimation = new Animation(
                new Sprite(effectsTexture, new Rectangle(0, 48, 16, 32)).SetOrigin(8, 32),
                new Sprite(effectsTexture, new Rectangle(16, 48, 16, 32)).SetOrigin(8, 32),
                new Sprite(effectsTexture, new Rectangle(32, 48, 16, 32)).SetOrigin(8, 32),
                new Sprite(effectsTexture, new Rectangle(48, 48, 16, 32)).SetOrigin(8, 32)
                ).SetFrameRate(2);

            var geysers = level.Geysers.Select(x => new Geyser(x, geyserAnimation)).ToList();
            
            foreach (var g in geysers)
            {
                Scene.AddGameObject(g);
            }
        }

        public void AddSnowball(Snowball ball)
        {
            Scene.AddGameObject(ball);
        }

        public void AddCharacter(Character character)
        {
            Scene.AddGameObject(character);
        }

        public void RemoveCharacter(Character character)
        {
            Scene.RemoveGameObject(character);
        }

        public void RemoveAllCharacters()
        {
            foreach(var character in Characters)
            {
                Scene.RemoveGameObject(character);
            }
        }

        public void ResetIceBlocks()
        {
            foreach(var block in Level.Blocks)
            {
                block.State = new IceBlockIdleState(block);
            }
            _iceBlockBehaviour = new List<IceBlockBehaviour>() {
                new RandomSinkIceBlockBehaviour(),
                new TimedSinkIceBlockBehaviour(),
                new TimedDriftIceBlockBehaviour(),
            };
            Scene.Sync();
        }

        public void TryBonkCharacters()
        {
            // Bonks!
            foreach (var (a, b) in Characters.Combinations())
            {
                if (!a.CanCollide) continue;
                if (!b.CanCollide) continue;

                var p1 = a.Position;
                var p2 = b.Position;

                var dir = p2 - p1;
                var dist = dir.Length();

                if (dist > 8) continue;
                if (dist == 0)
                {
                    dir = new Vector2(1, 0);
                }
                else
                {
                    dir /= dist;
                }


                //Unstuck
                float penetration = (8 - dist) / 2;
                
                a.Position -= dir * penetration;
                b.Position += dir * penetration;

                var totalVelocity = Math.Abs(Vector2.Dot(a.Physics.Velocity, dir)) + Math.Abs(Vector2.Dot(b.Physics.Velocity, dir));
                var bonkVelocity = Math.Max(16, totalVelocity);

                var bonkA = BonkCharacters(a, b, bonkVelocity, -dir);
                var bonkB = BonkCharacters(b, a, bonkVelocity, dir);

                if(bonkA.Bonking) a.Bonk(bonkA.Velocity, bonkA.StunTime);
                if(bonkB.Bonking) b.Bonk(bonkB.Velocity, bonkB.StunTime);

                if(bonkA.Bonking || bonkB.Bonking) b.Sound.PlayBonk();
            }

            foreach (var character in Characters)
            {
                if (!character.CanCollide) continue;

                foreach (var snowball in _snowballs)
                {
                    if (character.Player == snowball.Info) continue;

                    var p1 = character.Position;
                    var p2 = snowball.Position;

                    var dir = p2 - p1;
                    var dist = dir.Length();

                    if (dist > 8) continue;

                    snowball.Collided = true;

                    character.Bonk(snowball.Velocity * 0.6f, 0.8f);
                    character.Sound.PlaySnowHit();
                }
            }

            foreach (var character in Characters)
            {
                if (!character.CanCollide) continue;

                foreach (var geyser in _geysers)
                {
                    if (!geyser.Erupting) continue;

                    var p1 = character.Position;
                    var p2 = geyser.Position;

                    var dir = p2 - p1;
                    var dist = dir.Length();

                    if (dist > 8) continue;
                    if (dist == 0) continue;

                    character.Bonk(-dir / dist * 64);
                    character.Bounce.Velocity = 128;
                    character.Sound.PlaySnowHit(); // TODO Geyser sounds and stuff
                }
            }
        }

        private (bool Bonking, Vector2 Velocity, float StunTime) BonkCharacters(Character self, Character other, float bonkVelocity, Vector2 direction)
        {
            // Advantage
            if (self.IsSliding && (other.IsGathering || other.IsWalking || self.IsBonking))
            {
                return (true, bonkVelocity * 0.3f * direction, 1f);
            }

            // Disadvantage
            else if ((self.IsGathering || self.IsWalking || self.IsBonking) && other.IsSliding)
            {
                return (true, bonkVelocity * (self.IsGathering ? 1.5f : 0.7f) * direction, 0.3f);
            }

            // Both bad :)
            else if (self.IsSliding && other.IsSliding)
            {
                return (true, bonkVelocity * 0.5f * direction, 1);
            }

            // No bonks
            else
            {
                // Divide equally
                return (false, bonkVelocity * 0.5f * direction, 0);
            }
        }

        public List<Character> TryDrownCharacters()
        {
            var result = new List<Character>();

            foreach(var p in Characters.Where(x => !x.IsDrowning))
            {
                var block = Level.GetIceBlockForPosition(p.Position);
                
                if(block == null || !block.Solid && p.Height < 1)
                {
                    p.Drown();
                    result.Add(p);
                }
            }

            return result;
        }

        // Extension methods maybe?
        public Vector2 FindApplicableSpawnLocation()
        {
            return Level.Spawns.Where(pos => {
                var block = Level.GetIceBlockForPosition(pos);

                if (block == null) return false;

                return block.IsIdle;
            }).RandomOrDefault();
        }

        public Vector2 FindRandomSpawnPoint()
        {
            return Random.NextPointsInPolygon(Level.Blocks.Where(x => x.IsIdle).Random().Polygon, 1).First();
        }

        public Character SpawnCharacter(Vector2 spawnLocation, PlayerInfo player)
        {
            var character = new Character(this, player, spawnLocation);
            character.Physics = character.Physics.SetFacing(-spawnLocation);

            AddCharacter(character);

            // TODO this is kinda ugly :)
            Scene.Sync();

            return character;
        }

        public void PlaceCharactersOnGround()
        {
            foreach (var penguin in Characters)
            {
                penguin.PlaceOnGround();
            }
        }

        public void Update(float delta)
        {
            UpdateLevel(delta);

            Scene.Update(delta);
            UpdateCharacters(delta);
        }
        public void UpdateLevel(float delta)
        {
            foreach(var behaviour in _iceBlockBehaviour)
            {
                behaviour.Update(Level.Blocks, delta);
            }

            foreach (var block in Level.Blocks)
            {
                block.Update(delta);
            }
        }

        public void UpdateCharacters(float delta)
        {
            foreach (var (character, input) in Characters.Select(x => (x, InputService.GetInputForPlayer(x.Player))).ToArray())
            {
                character.Update(input, delta);
            }
        }
        
        public void DrawWorld(Graphics2D graphics)
        {
            graphics.Clear(LevelGraphics.Settings.WaterColor);

            LevelGraphics.DrawWorld(graphics);

            SceneGraphics.Draw(graphics);
        }

        public void DrawPlayerIndicators(Graphics2D graphics)
        {
            foreach (var character in Characters)
            {
                if(character.Lifetime < 2)
                {
                    DrawIndicatorFor(graphics, character);
                }
            }
        }

        public void DrawIndicatorFor(Graphics2D graphics, Character character)
        {
            graphics.DrawSprite(UIGraphics.IndicatorOutline, character.Position - new Vector2(0, 32 + character.Bounce.Height), 0, GraphicsHelper.YToDepth(character.Position.Y));
            graphics.DrawSprite(UIGraphics.Indicator, character.Position - new Vector2(0, 32 + character.Bounce.Height), 0, GraphicsHelper.YToDepth(character.Position.Y), character.Player.Color);
        }

        public void Destroy()
        {
            Scene.Destroy();
            LevelGraphics.Dispose();
        }
    }
}
