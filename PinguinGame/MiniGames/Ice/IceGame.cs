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
using TinyGames.Engine.Graphics;
using TinyGames.Engine.Util;

namespace PinguinGame.MiniGames.Ice
{
    public class IceGame
    {
        public Camera Camera { get; set; }
        public IEnumerable<Character> Characters => _characters;
        public IEnumerable<PlayerInfo> Players => Fight.Players;
        public Fight Fight { get; private set; }

        public IceLevel Level { get; private set; }
        public Camera LevelCamera { get; private set; }
        public IceLevelGraphics LevelGraphics { get; private set; }
        public SnowballGraphics SnowballGraphics { get; set; }
        public IceGameUIGraphics UIGraphics { get; set; }
        public IceGameEffects Effects { get; set; }

        public IMiniGameInputService<CharacterInput> InputService { get; set; }
        public IUISoundService UISoundService { get; set; }
        public IScreenService ScreenService { get; set; }

        public ParticleSystem ParticleSystem { get; set; }

        private List<Character> _characters;
        private List<Snowball> _snowballs;
        private List<Geyser> _geysers;
        private List<IceBlockBehaviour> _iceBlockBehaviour;

        public ContentManager Content { get; set; }

        public IceGame(ContentManager content, GraphicsDevice device, IceLevel level, PlayerInfo[] players, IMiniGameInputService<CharacterInput> inputService, IUISoundService uiSoundService, IScreenService screenservice) // Probably should have a levelservice or something
        {
            Content = content;
            _characters = new List<Character>();
            _snowballs = new List<Snowball>();

            UISoundService = uiSoundService;
            ScreenService = screenservice;
            UIGraphics = new IceGameUIGraphics(content);
            Effects = new IceGameEffects(content);

            InputService = inputService;

            Fight = new Fight(players);

            Level = level;
            LevelCamera = new Camera(256, 1);
            LevelGraphics = new IceLevelGraphics(content, device, level, new IceLevelGraphicsSettings());
            SnowballGraphics = new SnowballGraphics() { 
                Indicator = new Sprite(content.Load<Texture2D>("Sprites/Ice/IceGameplayElements"), new Rectangle(0, 0, 8, 8)).CenterOrigin(),
                Sprite = new Sprite(content.Load<Texture2D>("Sprites/Ice/IceGameplayElements"), new Rectangle(8, 0, 8, 8)).CenterOrigin(),
                Shadow = new Sprite(content.Load<Texture2D>("Sprites/Ice/IceGameplayElements"), new Rectangle(8, 8, 8, 8)).CenterOrigin(),
            };

            _iceBlockBehaviour = new List<IceBlockBehaviour>() { 
                new RandomSinkIceBlockBehaviour(),
                new TimedSinkIceBlockBehaviour(),
                new TimedDriftIceBlockBehaviour(),
            };

            ParticleSystem = new ParticleSystem();
            var effectsTexture = content.Load<Texture2D>("Sprites/Effects");
            var geyserAnimation = new Animation(
                //new Sprite(content.Load<Texture2D>("Sprites/Characters/PenguinSheet"), new Rectangle(0, 0, 16, 16)).CenterOrigin(),
                //new Sprite(content.Load<Texture2D>("Sprites/Characters/PenguinSheet"), new Rectangle(0, 0, 16, 16)).CenterOrigin(),
                //new Sprite(content.Load<Texture2D>("Sprites/Characters/PenguinSheet"), new Rectangle(0, 0, 16, 16)).CenterOrigin(),
                //new Sprite(content.Load<Texture2D>("Sprites/Characters/PenguinSheet"), new Rectangle(0, 0, 16, 16)).CenterOrigin()
                new Sprite(effectsTexture, new Rectangle(0, 48, 16, 32)).SetOrigin(8, 32),
                new Sprite(effectsTexture, new Rectangle(16, 48, 16, 32)).SetOrigin(8, 32),
                new Sprite(effectsTexture, new Rectangle(32, 48, 16, 32)).SetOrigin(8, 32),
                new Sprite(effectsTexture, new Rectangle(48, 48, 16, 32)).SetOrigin(8, 32)
                ).SetFrameRate(2);
            
            _geysers = level.Geysers.Select(x => new Geyser(x, ParticleSystem, geyserAnimation)).ToList();
        }

        public void AddSnowball(Snowball ball)
        {
            _snowballs.Add(ball);
        }

        public void AddCharacter(Character character)
        {
            _characters.Add(character);
        }

        public void RemoveCharacter(Character character)
        {
            character.Destroy();
            _characters.Remove(character);
        }

        public void RemoveAllCharacters()
        {
            foreach (var character in _characters) character.Destroy();

            _characters.Clear();
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
                    if (character.Player == snowball.Player) continue;

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

            foreach(var p in _characters.Where(x => !x.IsDrowning))
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

        // Extension methods maybe?
        public Vector2 FindApplicableSpawnLocation()
        {
            return Level.Spawns.Where(pos => {
                var block = Level.GetIceBlockForPosition(pos);

                if (block == null) return false;

                return block.IsIdle;
            }).RandomOrDefault();
        }

        public Character SpawnCharacter(Vector2 spawnLocation, PlayerInfo player)
        {
            var graphics = player.CharacterInfo.Graphics;

            var sound = CharacterSound.CreateCharacterSound(Content);

            var character = new Character(this, player, graphics, sound, spawnLocation);
            character.Physics = character.Physics.SetFacing(-spawnLocation);

            AddCharacter(character);

            return character;
        }

        public void PlaceCharactersOnGround()
        {
            foreach (var penguin in _characters)
            {
                IceBlock block = Level.GetIceBlockForPosition(penguin.Position);
                float height = 0;

                if (block != null && block.Solid)
                {
                    height = block.Height;
                }

                if (penguin.IsDrowning)
                {
                    height = 0;
                }

                if (penguin.GroundHeight > height)
                {
                    penguin.Bounce.Height += penguin.GroundHeight - height;
                }

                penguin.GroundHeight = height;
                penguin.Grounded = block != null && block.Solid;
            }
        }
        public void MoveCharactersWithGround(float delta)
        {
            foreach (var penguin in _characters)
            {
                IceBlock block = Level.GetIceBlockForPosition(penguin.Position);

                if (block == null) continue;

                penguin.Position += block.Velocity * delta;
            }
        }

        public void Update(float delta)
        {
            UpdateLevel(delta);
            UpdateSnowballs(delta);
            UpdatePenguins(delta);
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

            foreach(var geyser in _geysers)
            {
                geyser.Update(delta);
            }

            ParticleSystem.Update(delta);
        }

        public void UpdateSnowballs(float delta)
        {
            _snowballs = _snowballs.Where(x => x.Update(delta)).ToList();
        }

        public void UpdatePenguins(float delta)
        {
            foreach (var (penguin, input) in _characters.Select(x => (x, InputService.GetInputForPlayer(x.Player))))
            {
                penguin.Update(input, delta);
            }

            PlaceCharactersOnGround();
            MoveCharactersWithGround(delta);
        }
        
        public void DrawWorld(Graphics2D graphics)
        {
            graphics.Clear(LevelGraphics.Settings.WaterColor);

            LevelGraphics.DrawWorld(graphics);
            //graphics.DrawTexture(LevelGraphics.RenderTarget, new Vector2(-64, -64), new Vector2(128, 128));

            //var penguins = _penguins.OrderBy(x => x.Physics.Position.Y);

            foreach (var snowball in _snowballs)
            {
                IceBlock block = Level.GetIceBlockForPosition(snowball.Position);

                float height = 0;

                if (block != null && block.Solid)
                {
                    height = block.Height;
                }

                graphics.DrawSprite(SnowballGraphics.Shadow, snowball.Position - new Vector2(0, height), snowball.Angle, GraphicsHelper.YToDepth(snowball.Position.Y));
            }

            foreach (var penguin in _characters)
            {
                penguin.Draw(graphics, penguin.Graphics);
            }

            foreach(var snowball in _snowballs)
            {
                graphics.DrawSprite(SnowballGraphics.Sprite, snowball.Position - new Vector2(0, snowball.Height), snowball.Angle, GraphicsHelper.YToDepth(snowball.Position.Y));
            }

            foreach (var penguin in _characters)
            {
                if (!penguin.SnowballGathering.HasSnowball) continue;

                graphics.DrawSprite(SnowballGraphics.Indicator, penguin.Position - new Vector2(0, 24 + penguin.Bounce.Height), 0, GraphicsHelper.YToDepth(penguin.Position.Y));
            }
            ParticleSystem.Draw(graphics);
        }

        public void DrawPlayerIndicators(Graphics2D graphics)
        {
            foreach (var character in _characters)
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
            // TODO just have the full character graphics and stuff unload, instead of this
            LevelGraphics.Dispose();
        }
    }
}
