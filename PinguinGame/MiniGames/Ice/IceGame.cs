using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PinguinGame.Audio;
using PinguinGame.Graphics;
using PinguinGame.Input;
using PinguinGame.MiniGames.Generic;
using PinguinGame.MiniGames.Ice.Behaviours;
using PinguinGame.MiniGames.Ice.CharacterStates;
using PinguinGame.Player;
using PinguinGame.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using TinyGames.Engine.Collections;
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
        public IEnumerable<CharacterGameObject> Characters => Scene.FindGameObjectsOfType<CharacterGameObject>();
        public PlayerInfo[] Players { get; private set; }

        public IceLevel Level { get; private set; }
        public IceLevelGraphics LevelGraphics { get; private set; }

        public IMiniGameInputService<CharacterInput> InputService { get; set; }
        public IUISoundService UISoundService { get; set; }
        public IScreenService ScreenService { get; set; }

        private List<IceBlockBehaviour> _iceBlockBehaviour;

        public ContentManager Content { get; set; }
        
        public Scene Scene { get; set; }
        public SceneGraphics SceneGraphics { get; set; }
        public CharacterCollisionBehaviour CharacterCollisions { get; set; }

        public IceGame(IServiceProvider services, LevelInfo level, PlayerInfo[] players, IMiniGameInputService<CharacterInput> inputService, IUISoundService uiSoundService, IScreenService screenservice)
        {
            var graphicsDeviceService = services.GetService<IGraphicsDeviceService>();            
            
            Players = players;

            Content = new ContentManager(services);
            Content.RootDirectory = "Content";

            UISoundService = uiSoundService;
            ScreenService = screenservice;

            InputService = inputService;

            Level = Content.LoadIceLevel(level.File);
            LevelGraphics = new IceLevelGraphics(Content, graphicsDeviceService.GraphicsDevice, Level, new IceLevelGraphicsSettings());

            _iceBlockBehaviour = new List<IceBlockBehaviour>() { 
                new RandomSinkIceBlockBehaviour(),
                new TimedSinkIceBlockBehaviour(),
                new TimedDriftIceBlockBehaviour(),
            };

            CharacterCollisions = new CharacterCollisionBehaviour();

            Scene = new Scene(services);

            SceneGraphics = Scene.AddBehaviour(new SceneGraphics());
            Scene.AddBehaviour(new ParticleSystem());
            Scene.AddBehaviour(new Walkables(Level));
            Scene.AddBehaviour(new IceGameEffects(Content));
            Scene.AddBehaviour(new IceGameUIGraphics(Content));
            Scene.AddBehaviour(new IceGameGraphics(Content));
            Scene.AddBehaviour(CharacterCollisions);
            Scene.AddBehaviour(new CharacterIndicatorBehaviour());


            // This is loading the Geysers, not really needed.
            foreach (var g in Level.Geysers.Select(x => new Geyser(x)))
            {
                Scene.AddGameObject(g);
            }

            foreach (var bridge in Level.Bridges.Select(x => new Bridge(x.Position, x.Size)))
            {
                Scene.AddGameObject(bridge);
            }

            foreach (var grass in Level.Grass.Select(x => new GrassGameObject(x)))
            {
                Scene.AddGameObject(grass);
            }
            foreach (var grass in Level.Trees.Select(x => new TreeGameObject(x)))
            {
                Scene.AddGameObject(grass);
            }

            // Initialize the scene with all objects :)
            Scene.Init();
        }

        public void AddSnowball(SnowballGameObject ball)
        {
            Scene.AddGameObject(ball);
        }

        public void AddCharacter(CharacterGameObject character)
        {
            Scene.AddGameObject(character);
        }

        public void RemoveCharacter(CharacterGameObject character)
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
            return new Random().NextPointsInPolygon(Level.Blocks.Where(x => x.IsIdle).Random().Polygon, 1).First();
        }

        public CharacterGameObject SpawnCharacter(Vector2 spawnLocation, PlayerInfo player)
        {
            var character = new CharacterGameObject(player, spawnLocation);
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
            graphics.Clear(Color.White);

            LevelGraphics.DrawWorld(graphics);

            SceneGraphics.Draw(graphics);

            // Draw the water
            graphics.SetBlendMode(BlendMode.Multiply);
            graphics.DrawRectangleFlat(Camera.Bounds.Translated(new Vector2(0, -32)), -32, LevelGraphics.Settings.WaterColor);
            graphics.DrawRectangleFlat(Camera.Bounds, 0, LevelGraphics.Settings.WaterColor);
            graphics.SetBlendMode(BlendMode.Normal);
        }


        public void Destroy()
        {
            Scene.Destroy();
            LevelGraphics.Dispose();
            Content.Unload();
        }
    }
}
